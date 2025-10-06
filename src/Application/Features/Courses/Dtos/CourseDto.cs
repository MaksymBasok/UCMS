using UCMS.Domain.Courses;
namespace UCMS.Application.Features.Courses.Dtos;
public sealed record CourseDto(Guid Id, string Code, string Title, string Description, int Credits)
{
    public static CourseDto From(Course c) => new(c.Id, c.Code, c.Title, c.Description, c.Credits);
}
