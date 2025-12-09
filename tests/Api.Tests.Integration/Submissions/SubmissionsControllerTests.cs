using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Assignments;
using Tests.Data.Courses;
using Tests.Data.Students;
using Tests.Data.Submissions;
using UCMS.Application.Features.Submissions.Commands.CancelSubmission;
using UCMS.Application.Features.Submissions.Commands.CompleteSubmission;
using UCMS.Application.Features.Submissions.Commands.CreateSubmission;
using UCMS.Application.Features.Submissions.Commands.StartSubmission;
using UCMS.Application.Features.Submissions.Commands.UpdateSubmission;
using UCMS.Application.Features.Submissions.Dtos;
using UCMS.Domain.Assignments;
using UCMS.Domain.Courses;
using UCMS.Domain.Students;
using UCMS.Domain.Submissions;
using Xunit;

namespace Api.Tests.Integration.Submissions;

[Collection("Integration")]
public sealed class SubmissionsControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private const string BaseRoute = "/api/submissions";

    private Course _course = null!;
    private Assignment _assignment = null!;
    private Student _firstStudent = null!;
    private Student _secondStudent = null!;
    private Student _thirdStudent = null!;
    private Submission _openSubmission = null!;
    private Submission _completedSubmission = null!;

    public SubmissionsControllerTests(IntegrationTestWebFactory factory)
        : base(factory)
    {
    }

    public async Task InitializeAsync() => await ResetDatabaseAsync();

    public async Task DisposeAsync()
    {
        Context.Submissions.RemoveRange(Context.Submissions);
        Context.Assignments.RemoveRange(Context.Assignments);
        Context.Students.RemoveRange(Context.Students);
        Context.Courses.RemoveRange(Context.Courses);
        await SaveChangesAsync();
    }

    [Fact]
    public async Task ShouldCreateSubmission()
    {
        await ResetDatabaseAsync();

        var command = new CreateSubmissionCommand(
            _assignment.Id,
            _thirdStudent.Id,
            "https://example.com/submissions/new",
            DateTime.UtcNow);

        var response = await Client.PostAsJsonAsync(BaseRoute, command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var dto = await response.ToResponseModel<SubmissionDto>();
        dto.AssignmentId.Should().Be(_assignment.Id);
        dto.StudentId.Should().Be(command.StudentId);
    }

    [Fact]
    public async Task ShouldNotCreateDuplicateSubmission()
    {
        await ResetDatabaseAsync();

        var command = new CreateSubmissionCommand(
            _openSubmission.AssignmentId,
            _openSubmission.StudentId,
            "https://example.com/submissions/new",
            DateTime.UtcNow);

        var response = await Client.PostAsJsonAsync(BaseRoute, command);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ShouldGetSubmissions()
    {
        await ResetDatabaseAsync();

        var response = await Client.GetAsync(BaseRoute);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.ToResponseModel<IReadOnlyList<SubmissionDto>>();
        items.Should().HaveCount(2);
    }

    [Fact]
    public async Task ShouldGetSubmissionById()
    {
        await ResetDatabaseAsync();

        var response = await Client.GetAsync($"{BaseRoute}/{_openSubmission.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<SubmissionDto>();
        dto.Id.Should().Be(_openSubmission.Id);
        dto.Status.Should().Be(_openSubmission.Status);
    }

    [Fact]
    public async Task ShouldUpdateSubmission()
    {
        await ResetDatabaseAsync();

        var command = new UpdateSubmissionCommand(_openSubmission.Id, "https://example.com/submissions/updated");

        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_openSubmission.Id}", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<SubmissionDto>();
        dto.ContentUrl.Should().Be(command.ContentUrl);
    }

    [Fact]
    public async Task ShouldReturnBadRequestWhenUpdatingCompletedSubmission()
    {
        await ResetDatabaseAsync();

        var command = new UpdateSubmissionCommand(_completedSubmission.Id, "https://example.com/submissions/blocked");

        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_completedSubmission.Id}", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldStartSubmission()
    {
        await ResetDatabaseAsync();

        var response = await Client.PatchAsync($"{BaseRoute}/{_openSubmission.Id}/start", null);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var stored = await Context.Submissions.AsNoTracking().FirstAsync(x => x.Id == _openSubmission.Id);
        stored.Status.Should().Be(SubmissionStatus.InProgress);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenStartingMissingSubmission()
    {
        await ResetDatabaseAsync();

        var response = await Client.PatchAsync($"{BaseRoute}/{Guid.NewGuid()}/start", null);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldCompleteSubmission()
    {
        await ResetDatabaseAsync();

        var startResponse = await Client.PatchAsync($"{BaseRoute}/{_openSubmission.Id}/start", null);
        startResponse.EnsureSuccessStatusCode();

        var command = new CompleteSubmissionCommand(_openSubmission.Id, "Well done", 98);

        var response = await Client.PostAsJsonAsync($"{BaseRoute}/{_openSubmission.Id}/complete", command);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var stored = await Context.Submissions.AsNoTracking().FirstAsync(x => x.Id == _openSubmission.Id);
        stored.Status.Should().Be(SubmissionStatus.Completed);
        stored.Grade.Should().Be(command.Grade);
    }

    [Fact]
    public async Task ShouldCancelSubmission()
    {
        await ResetDatabaseAsync();

        var response = await Client.PostAsync($"{BaseRoute}/{_openSubmission.Id}/cancel", null);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var stored = await Context.Submissions.AsNoTracking().FirstAsync(x => x.Id == _openSubmission.Id);
        stored.Status.Should().Be(SubmissionStatus.Cancelled);
    }

    [Fact]
    public async Task ShouldDeleteSubmission()
    {
        await ResetDatabaseAsync();

        var response = await Client.DeleteAsync($"{BaseRoute}/{_openSubmission.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var exists = await Context.Submissions.AnyAsync(x => x.Id == _openSubmission.Id);
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenDeletingMissingSubmission()
    {
        await ResetDatabaseAsync();

        var response = await Client.DeleteAsync($"{BaseRoute}/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task ResetDatabaseAsync()
    {
        await EnsureDatabaseAsync();

        Context.Submissions.RemoveRange(Context.Submissions);
        Context.Assignments.RemoveRange(Context.Assignments);
        Context.Students.RemoveRange(Context.Students);
        Context.Courses.RemoveRange(Context.Courses);
        await SaveChangesAsync();

        _course = CourseData.FirstCourse();
        _assignment = AssignmentData.FirstAssignment(_course.Id);
        _firstStudent = StudentData.FirstStudent();
        _secondStudent = StudentData.SecondStudent();
        _thirdStudent = StudentData.ThirdStudent();
        _openSubmission = SubmissionData.OpenSubmission(_assignment.Id, _firstStudent.Id);
        _completedSubmission = SubmissionData.CompletedSubmission(_assignment.Id, _secondStudent.Id);

        await Context.Courses.AddAsync(_course);
        await Context.Assignments.AddAsync(_assignment);
        await Context.Students.AddRangeAsync(_firstStudent, _secondStudent, _thirdStudent);
        await Context.Submissions.AddRangeAsync(_openSubmission, _completedSubmission);
        await SaveChangesAsync();
    }
}
