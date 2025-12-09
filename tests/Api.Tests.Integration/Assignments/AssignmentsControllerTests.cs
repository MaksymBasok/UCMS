using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Assignments;
using Tests.Data.Courses;
using UCMS.Application.Features.Assignments.Commands.CloseAssignment;
using UCMS.Application.Features.Assignments.Commands.CreateAssignment;
using UCMS.Application.Features.Assignments.Commands.PublishAssignment;
using UCMS.Application.Features.Assignments.Commands.UpdateAssignment;
using UCMS.Application.Features.Assignments.Dtos;
using UCMS.Domain.Assignments;
using UCMS.Domain.Courses;
using Xunit;

namespace Api.Tests.Integration.Assignments;

[Collection("Integration")]
public sealed class AssignmentsControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private const string BaseRoute = "/api/assignments";

    private Course _course = null!;
    private Assignment _firstAssignment = null!;
    private Assignment _secondAssignment = null!;

    public AssignmentsControllerTests(IntegrationTestWebFactory factory)
        : base(factory)
    {
    }

    public async Task InitializeAsync() => await ResetDatabaseAsync();

    public async Task DisposeAsync()
    {
        Context.Submissions.RemoveRange(Context.Submissions);
        Context.Assignments.RemoveRange(Context.Assignments);
        Context.Courses.RemoveRange(Context.Courses);
        await SaveChangesAsync();
    }

    [Fact]
    public async Task ShouldGetAssignments()
    {
        await ResetDatabaseAsync();

        var response = await Client.GetAsync(BaseRoute);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.ToResponseModel<IReadOnlyList<AssignmentDto>>();
        items.Should().HaveCount(2);
        items.Select(x => x.Id).Should().Contain(new[] { _firstAssignment.Id, _secondAssignment.Id });
    }

    [Fact]
    public async Task ShouldGetAssignmentById()
    {
        await ResetDatabaseAsync();

        var response = await Client.GetAsync($"{BaseRoute}/{_firstAssignment.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<AssignmentDto>();
        dto.Id.Should().Be(_firstAssignment.Id);
        dto.Title.Should().Be(_firstAssignment.Title);
        dto.Description.Should().Be(_firstAssignment.Description);
        dto.DueDate.Should().BeCloseTo(_firstAssignment.DueDate, TimeSpan.FromSeconds(1));
        dto.Status.Should().Be(AssignmentStatus.Draft.ToString());
    }

    [Fact]
    public async Task ShouldCreateAssignment()
    {
        await ResetDatabaseAsync();

        var command = new CreateAssignmentCommand(
            _course.Id,
            "New Assignment",
            "New assignment description",
            DateTime.UtcNow.AddDays(5));

        var response = await Client.PostAsJsonAsync(BaseRoute, command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<AssignmentDto>();
        dto.Title.Should().Be(command.Title);

        var created = await Context.Assignments.FindAsync(dto.Id);
        created.Should().NotBeNull();
        created!.Description.Should().Be(command.Description);
    }

    [Fact]
    public async Task ShouldReturnBadRequestWhenCreatingInvalidAssignment()
    {
        await ResetDatabaseAsync();

        var command = new CreateAssignmentCommand(_course.Id, string.Empty, string.Empty, DateTime.UtcNow.AddHours(-1));

        var response = await Client.PostAsJsonAsync(BaseRoute, command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldUpdateAssignment()
    {
        await ResetDatabaseAsync();

        var command = new UpdateAssignmentCommand(
            _firstAssignment.Id,
            "Updated Title",
            "Updated description",
            DateTime.UtcNow.AddDays(8));

        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstAssignment.Id}", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await Context.Assignments.FindAsync(_firstAssignment.Id);
        updated!.Title.Should().Be(command.Title);
        updated.Description.Should().Be(command.Description);
        updated.DueDate.Should().BeCloseTo(command.DueDate, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenUpdatingMissingAssignment()
    {
        await ResetDatabaseAsync();

        var command = new UpdateAssignmentCommand(Guid.NewGuid(), "Updated", "Updated", DateTime.UtcNow.AddDays(4));

        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{command.Id}", command);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldPublishAssignment()
    {
        await ResetDatabaseAsync();

        var response = await Client.PatchAsync($"{BaseRoute}/{_firstAssignment.Id}/publish", null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<AssignmentDto>();
        dto.Status.Should().Be(AssignmentStatus.Published.ToString());
    }

    [Fact]
    public async Task ShouldCloseAssignmentAfterPublish()
    {
        await ResetDatabaseAsync();

        var publishResponse = await Client.PatchAsync($"{BaseRoute}/{_firstAssignment.Id}/publish", null);
        publishResponse.EnsureSuccessStatusCode();

        var response = await Client.PatchAsync($"{BaseRoute}/{_firstAssignment.Id}/close", null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<AssignmentDto>();
        dto.Status.Should().Be(AssignmentStatus.Closed.ToString());
    }

    [Fact]
    public async Task ShouldDeleteAssignment()
    {
        await ResetDatabaseAsync();

        var response = await Client.DeleteAsync($"{BaseRoute}/{_firstAssignment.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var exists = await Context.Assignments.AnyAsync(x => x.Id == _firstAssignment.Id);
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenDeletingMissingAssignment()
    {
        await ResetDatabaseAsync();

        var response = await Client.DeleteAsync($"{BaseRoute}/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task ResetDatabaseAsync()
    {
        Context.Submissions.RemoveRange(Context.Submissions);
        Context.Assignments.RemoveRange(Context.Assignments);
        Context.Courses.RemoveRange(Context.Courses);
        await SaveChangesAsync();

        _course = CourseData.FirstCourse();
        _firstAssignment = AssignmentData.FirstAssignment(_course.Id);
        _secondAssignment = AssignmentData.SecondAssignment(_course.Id);

        await Context.Courses.AddAsync(_course);
        await Context.Assignments.AddRangeAsync(_firstAssignment, _secondAssignment);
        await SaveChangesAsync();
    }
}
