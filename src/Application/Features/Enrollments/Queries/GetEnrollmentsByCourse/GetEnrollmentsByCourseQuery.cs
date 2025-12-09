using MediatR;
using UCMS.Application.Features.Enrollments.Dtos;

namespace UCMS.Application.Features.Enrollments.Queries.GetEnrollmentsByCourse;

public sealed record GetEnrollmentsByCourseQuery(Guid CourseId) : IRequest<IReadOnlyList<EnrollmentDto>>;
