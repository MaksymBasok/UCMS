using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.CourseSchedules;
using Tests.Data.Courses;
using UCMS.Application.Features.CourseSchedules.Commands.CreateCourseSchedule;
using UCMS.Application.Features.CourseSchedules.Commands.DeactivateCourseSchedule;
using UCMS.Application.Features.CourseSchedules.Commands.UpdateCourseSchedule;
using UCMS.Application.Features.CourseSchedules.Dtos;
using UCMS.Domain.Courses;
using UCMS.Domain.Schedules;
using Xunit;

namespace Api.Tests.Integration.CourseSchedules;

[Collection("Integration")]
public sealed class CourseSchedulesControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private const string BaseRoute = "/api/course-schedules";

    private Course _course = null!;
    private CourseSchedule _firstSchedule = null!;
    private CourseSchedule _secondSchedule = null!;

    public CourseSchedulesControllerTests(IntegrationTestWebFactory factory)
        : base(factory)
    {
    }

    public async Task InitializeAsync() => await ResetDatabaseAsync();

    public async Task DisposeAsync()
    {
        Context.CourseSchedules.RemoveRange(Context.CourseSchedules);
        Context.Courses.RemoveRange(Context.Courses);
        await SaveChangesAsync();
    }

    [Fact]
    public async Task ShouldCreateCourseSchedule()
    {
        await ResetDatabaseAsync();

        var command = new CreateCourseScheduleCommand(
            _course.Id,
            "New Topic",
            CourseScheduleFrequency.Daily,
            DateTime.UtcNow.AddDays(2));

        var response = await Client.PostAsJsonAsync(BaseRoute, command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var dto = await response.ToResponseModel<CourseScheduleDto>();
        dto.Topic.Should().Be(command.Topic);
        dto.CourseId.Should().Be(_course.Id);
    }

    [Fact]
    public async Task ShouldReturnBadRequestForPastSchedule()
    {
        await ResetDatabaseAsync();

        var command = new CreateCourseScheduleCommand(
            _course.Id,
            "Past",
            CourseScheduleFrequency.Weekly,
            DateTime.UtcNow.AddHours(-2));

        var response = await Client.PostAsJsonAsync(BaseRoute, command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldGetAllSchedules()
    {
        await ResetDatabaseAsync();

        var response = await Client.GetAsync(BaseRoute);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.ToResponseModel<IReadOnlyList<CourseScheduleDto>>();
        items.Should().HaveCount(2);
    }

    [Fact]
    public async Task ShouldGetSchedulesByCourse()
    {
        await ResetDatabaseAsync();

        var response = await Client.GetAsync($"{BaseRoute}/course/{_course.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var items = await response.ToResponseModel<IReadOnlyList<CourseScheduleDto>>();
        items.Should().AllSatisfy(dto => dto.CourseId.Should().Be(_course.Id));
    }

    [Fact]
    public async Task ShouldUpdateCourseSchedule()
    {
        await ResetDatabaseAsync();

        var command = new UpdateCourseScheduleCommand(
            _firstSchedule.Id,
            "Updated Topic",
            CourseScheduleFrequency.Quarterly,
            DateTime.UtcNow.AddDays(15));

        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstSchedule.Id}", command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<CourseScheduleDto>();
        dto.Topic.Should().Be(command.Topic);
        dto.Frequency.Should().Be(command.Frequency);

        var stored = await Context.CourseSchedules.AsNoTracking().FirstAsync(x => x.Id == _firstSchedule.Id);
        stored.Frequency.Should().Be(CourseScheduleFrequency.Quarterly);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenUpdatingMissingSchedule()
    {
        await ResetDatabaseAsync();

        var command = new UpdateCourseScheduleCommand(Guid.NewGuid(), "Updated", CourseScheduleFrequency.Weekly, DateTime.UtcNow.AddDays(5));

        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{command.Id}", command);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldDeactivateCourseSchedule()
    {
        await ResetDatabaseAsync();

        var response = await Client.DeleteAsync($"{BaseRoute}/{_firstSchedule.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var schedule = await Context.CourseSchedules.AsNoTracking().FirstAsync(x => x.Id == _firstSchedule.Id);
        schedule.IsActive.Should().BeFalse();
    }

    private async Task ResetDatabaseAsync()
    {
        Context.CourseSchedules.RemoveRange(Context.CourseSchedules);
        Context.Courses.RemoveRange(Context.Courses);
        await SaveChangesAsync();

        _course = CourseData.FirstCourse();
        _firstSchedule = CourseScheduleData.FirstSchedule(_course.Id);
        _secondSchedule = CourseScheduleData.SecondSchedule(_course.Id);

        await Context.Courses.AddAsync(_course);
        await Context.CourseSchedules.AddRangeAsync(_firstSchedule, _secondSchedule);
        await SaveChangesAsync();
    }
}
