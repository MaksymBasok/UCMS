using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Courses;
using Tests.Data.Enrollments;
using Tests.Data.Students;
using UCMS.Application.Features.Enrollments.Commands.CompleteEnrollment;
using UCMS.Application.Features.Enrollments.Commands.CreateEnrollment;
using UCMS.Application.Features.Enrollments.Commands.DropEnrollment;
using UCMS.Application.Features.Enrollments.Dtos;
using UCMS.Domain.Courses;
using UCMS.Domain.Enrollments;
using UCMS.Domain.Students;
using Xunit;

namespace Api.Tests.Integration.Enrollments;

[Collection("Integration")]
public sealed class EnrollmentsControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private const string BaseRoute = "/api/enrollments";

    private Course _firstCourse = null!;
    private Course _secondCourse = null!;
    private Student _firstStudent = null!;
    private Student _secondStudent = null!;
    private Enrollment _activeEnrollment = null!;
    private Enrollment _completedEnrollment = null!;

    public EnrollmentsControllerTests(IntegrationTestWebFactory factory)
        : base(factory)
    {
    }

    public async Task InitializeAsync() => await ResetDatabaseAsync();

    public async Task DisposeAsync()
    {
        Context.Enrollments.RemoveRange(Context.Enrollments);
        Context.Students.RemoveRange(Context.Students);
        Context.Courses.RemoveRange(Context.Courses);
        await SaveChangesAsync();
    }

    [Fact]
    public async Task ShouldEnrollStudent()
    {
        await ResetDatabaseAsync();

        var command = new CreateEnrollmentCommand(_secondStudent.Id, _firstCourse.Id);

        var response = await Client.PostAsJsonAsync(BaseRoute, command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var dto = await response.ToResponseModel<EnrollmentDto>();
        dto.StudentId.Should().Be(command.StudentId);
        dto.Status.Should().Be(EnrollmentStatus.Active);
    }

    [Fact]
    public async Task ShouldNotEnrollDuplicate()
    {
        await ResetDatabaseAsync();

        var command = new CreateEnrollmentCommand(_activeEnrollment.StudentId, _activeEnrollment.CourseId);

        var response = await Client.PostAsJsonAsync(BaseRoute, command);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ShouldFilterEnrollmentsByStudent()
    {
        await ResetDatabaseAsync();

        var response = await Client.GetAsync($"{BaseRoute}?studentId={_activeEnrollment.StudentId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.ToResponseModel<IReadOnlyList<EnrollmentDto>>();
        items.Should().ContainSingle(x => x.Id == _activeEnrollment.Id);
    }

    [Fact]
    public async Task ShouldGetEnrollmentById()
    {
        await ResetDatabaseAsync();

        var response = await Client.GetAsync($"{BaseRoute}/{_activeEnrollment.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<EnrollmentDto>();
        dto.Id.Should().Be(_activeEnrollment.Id);
        dto.Status.Should().Be(_activeEnrollment.Status);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenEnrollmentMissing()
    {
        await ResetDatabaseAsync();

        var response = await Client.GetAsync($"{BaseRoute}/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldCompleteEnrollment()
    {
        await ResetDatabaseAsync();

        var response = await Client.PatchAsync($"{BaseRoute}/{_activeEnrollment.Id}/complete", null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<EnrollmentDto>();
        dto.Status.Should().Be(EnrollmentStatus.Completed);
        var stored = await Context.Enrollments.AsNoTracking().FirstAsync(x => x.Id == _activeEnrollment.Id);
        stored.Status.Should().Be(EnrollmentStatus.Completed);
    }

    [Fact]
    public async Task ShouldReturnBadRequestWhenCompletingNonActiveEnrollment()
    {
        await ResetDatabaseAsync();

        var response = await Client.PatchAsync($"{BaseRoute}/{_completedEnrollment.Id}/complete", null);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldDropEnrollment()
    {
        await ResetDatabaseAsync();

        var response = await Client.PatchAsync($"{BaseRoute}/{_activeEnrollment.Id}/drop", null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<EnrollmentDto>();
        dto.Status.Should().Be(EnrollmentStatus.Dropped);
    }

    [Fact]
    public async Task ShouldReturnBadRequestWhenDroppingCompletedEnrollment()
    {
        await ResetDatabaseAsync();

        var response = await Client.PatchAsync($"{BaseRoute}/{_completedEnrollment.Id}/drop", null);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenDroppingMissingEnrollment()
    {
        await ResetDatabaseAsync();

        var response = await Client.PatchAsync($"{BaseRoute}/{Guid.NewGuid()}/drop", null);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task ResetDatabaseAsync()
    {
        await EnsureDatabaseAsync();

        Context.Enrollments.RemoveRange(Context.Enrollments);
        Context.Students.RemoveRange(Context.Students);
        Context.Courses.RemoveRange(Context.Courses);
        await SaveChangesAsync();

        _firstCourse = CourseData.FirstCourse();
        _secondCourse = CourseData.SecondCourse();
        _firstStudent = StudentData.FirstStudent();
        _secondStudent = StudentData.SecondStudent();
        _activeEnrollment = EnrollmentData.ActiveEnrollment(_firstStudent.Id, _firstCourse.Id);
        _completedEnrollment = EnrollmentData.CompletedEnrollment(_secondStudent.Id, _secondCourse.Id);

        await Context.Courses.AddRangeAsync(_firstCourse, _secondCourse);
        await Context.Students.AddRangeAsync(_firstStudent, _secondStudent);
        await Context.Enrollments.AddRangeAsync(_activeEnrollment, _completedEnrollment);
        await SaveChangesAsync();
    }
}
