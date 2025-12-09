using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Students;
using UCMS.Application.Features.Students.Commands.CreateStudent;
using UCMS.Application.Features.Students.Commands.UpdateStudent;
using UCMS.Application.Features.Students.Dtos;
using UCMS.Domain.Students;
using Xunit;

namespace Api.Tests.Integration.Students;

[Collection("Integration")]
public sealed class StudentsControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private const string BaseRoute = "/api/students";

    private Student _firstStudent = null!;
    private Student _secondStudent = null!;

    public StudentsControllerTests(IntegrationTestWebFactory factory)
        : base(factory)
    {
    }

    public async Task InitializeAsync() => await ResetDatabaseAsync();

    public async Task DisposeAsync()
    {
        Context.Students.RemoveRange(Context.Students);
        await SaveChangesAsync();
    }

    [Fact]
    public async Task ShouldGetStudentById()
    {
        // Arrange
        await ResetDatabaseAsync();

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{_firstStudent.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.ToResponseModel<StudentDto>();
        dto.Id.Should().Be(_firstStudent.Id);
        dto.StudentNumber.Should().Be(_firstStudent.StudentNumber);
        dto.FullName.Should().Be(_firstStudent.FullName);
        dto.Email.Should().Be(_firstStudent.Email);
        dto.GroupId.Should().Be(_firstStudent.GroupId);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenStudentDoesNotExist()
    {
        // Arrange
        await ResetDatabaseAsync();
        var nonExistingId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"{BaseRoute}/{nonExistingId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldCreateStudent()
    {
        // Arrange
        await ResetDatabaseAsync();
        var request = new CreateStudentCommand("STU-100", "Alice Johnson", "alice.johnson@example.com", Guid.NewGuid());

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var dto = await response.ToResponseModel<StudentDto>();
        dto.StudentNumber.Should().Be(request.StudentNumber);

        var created = await Context.Students.FirstAsync(x => x.Id == dto.Id);
        created!.FullName.Should().Be(request.FullName);
        created.Email.Should().Be(request.Email);
        created.GroupId.Should().Be(request.GroupId);
    }

    [Fact]
    public async Task ShouldNotCreateDuplicateStudent()
    {
        // Arrange
        await ResetDatabaseAsync();
        var request = new CreateStudentCommand(_firstStudent.StudentNumber, "Other", "other@example.com", Guid.NewGuid());

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ShouldNotCreateStudentWithInvalidData()
    {
        // Arrange
        await ResetDatabaseAsync();
        var request = new CreateStudentCommand(" ", string.Empty, "invalid", Guid.Empty);

        // Act
        var response = await Client.PostAsJsonAsync(BaseRoute, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldUpdateStudent()
    {
        // Arrange
        await ResetDatabaseAsync();
        var request = new UpdateStudentCommand(_firstStudent.Id, "Updated Name", "updated@example.com", Guid.NewGuid());

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstStudent.Id}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await Context.Students.AsNoTracking().FirstAsync(x => x.Id == _firstStudent.Id);
        updated.FullName.Should().Be(request.FullName);
        updated.Email.Should().Be(request.Email);
        updated.GroupId.Should().Be(request.GroupId);
    }

    [Fact]
    public async Task ShouldNotUpdateWithInvalidData()
    {
        // Arrange
        await ResetDatabaseAsync();
        var request = new UpdateStudentCommand(_firstStudent.Id, string.Empty, "invalid", Guid.Empty);

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{_firstStudent.Id}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenUpdatingNonExistingStudent()
    {
        // Arrange
        await ResetDatabaseAsync();
        var request = new UpdateStudentCommand(Guid.NewGuid(), "Name", "name@example.com", Guid.NewGuid());

        // Act
        var response = await Client.PutAsJsonAsync($"{BaseRoute}/{request.Id}", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldDeleteStudent()
    {
        // Arrange
        await ResetDatabaseAsync();

        // Act
        var response = await Client.DeleteAsync($"{BaseRoute}/{_firstStudent.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var exists = await Context.Students.AnyAsync(x => x.Id == _firstStudent.Id);
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldReturnNotFoundWhenDeletingNonExistingStudent()
    {
        // Arrange
        await ResetDatabaseAsync();
        var nonExistingId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"{BaseRoute}/{nonExistingId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task ResetDatabaseAsync()
    {
        Context.Students.RemoveRange(Context.Students);
        await SaveChangesAsync();

        _firstStudent = StudentData.FirstStudent();
        _secondStudent = StudentData.SecondStudent();

        await Context.Students.AddRangeAsync(_firstStudent, _secondStudent);
        await SaveChangesAsync();
    }
}
