using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Courses;
using UCMS.Application.Features.Courses.Commands.CreateCourse;
using UCMS.Application.Features.Courses.Commands.UpdateCourse;
using UCMS.Application.Features.Courses.Dtos;
using UCMS.Domain.Courses;
using Xunit;

namespace Api.Tests.Integration.Courses;

[Collection("Integration")]
public sealed class CoursesControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private const string BaseRoute = "/api/courses";

    private Course _firstCourse = null!;
    private Course _secondCourse = null!;

    public CoursesControllerTests(IntegrationTestWebFactory factory)
        : base(factory)
    {
    }

    public async Task InitializeAsync() => await ResetDatabaseAsync();

    public async Task DisposeAsync()
    {
        Context.Courses.RemoveRange(Context.Courses);
        await SaveChangesAsync();
    }

    [Fact]
    public async Task ShouldGetCourseById()
    {
        await ResetDatabaseAsync();

        var response = await Client.GetAsync($"{BaseRoute}/{_firstCourse.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<CourseDto>();
        dto.Id.Should().Be(_firstCourse.Id);
        dto.Code.Should().Be(_firstCourse.Code);
        dto.Title.Should().Be(_firstCourse.Title);
        dto.Description.Should().Be(_firstCourse.Description);
        dto.Credits.Should().Be(_firstCourse.Credits);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenCourseDoesNotExist()
    {
        await ResetDatabaseAsync();

        var response = await Client.GetAsync($"{BaseRoute}/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldCreateCourse()
    {
        await ResetDatabaseAsync();

        var request = new CreateCourseCommand("INT-200", "New Course", "New course description", 4);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var dto = await response.ToResponseModel<CourseDto>();
        dto.Code.Should().Be(request.Code);

        var created = await Context.Courses.FindAsync(dto.Id);
        created.Should().NotBeNull();
        created!.Title.Should().Be(request.Title);
        created.Description.Should().Be(request.Description);
        created.Credits.Should().Be(request.Credits);
    }

    [Fact]
    public async Task ShouldNotCreateDuplicateCourse()
    {
        await ResetDatabaseAsync();

        var request = new CreateCourseCommand(_firstCourse.Code, "Another", "Another description", 5);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ShouldNotCreateCourseWithInvalidData()
    {
        await ResetDatabaseAsync();

        var request = new CreateCourseCommand(" ", string.Empty, string.Empty, 0);

        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldUpdateCourse()
    {
        await ResetDatabaseAsync();

        var request = new UpdateCourseCommand(_firstCourse.Id, "Updated", "Updated description", 7);

        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstCourse.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await Context.Courses.FindAsync(_firstCourse.Id);
        updated!.Title.Should().Be(request.Title);
        updated.Description.Should().Be(request.Description);
        updated.Credits.Should().Be(request.Credits);
    }

    [Fact]
    public async Task ShouldNotUpdateWithInvalidData()
    {
        await ResetDatabaseAsync();

        var request = new UpdateCourseCommand(_firstCourse.Id, string.Empty, string.Empty, 0);

        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstCourse.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenUpdatingNonExistingCourse()
    {
        await ResetDatabaseAsync();

        var request = new UpdateCourseCommand(Guid.NewGuid(), "Name", "Description", 5);

        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{request.Id}", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldDeleteCourse()
    {
        await ResetDatabaseAsync();

        var response = await Client.DeleteAsync($"{BaseRoute}/{_firstCourse.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var exists = await Context.Courses.AnyAsync(x => x.Id == _firstCourse.Id);
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenDeletingNonExistingCourse()
    {
        await ResetDatabaseAsync();

        var response = await Client.DeleteAsync($"{BaseRoute}/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task ResetDatabaseAsync()
    {
        Context.Courses.RemoveRange(Context.Courses);
        await SaveChangesAsync();

        _firstCourse = CourseData.FirstCourse();
        _secondCourse = CourseData.SecondCourse();

        await Context.Courses.AddRangeAsync(_firstCourse, _secondCourse);
        await SaveChangesAsync();
    }
}
