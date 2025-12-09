using MediatR;
using UCMS.Application.Features.Enrollments.Dtos;

namespace UCMS.Application.Features.Enrollments.Queries.GetEnrollmentsByStudent;

public sealed record GetEnrollmentsByStudentQuery(Guid StudentId) : IRequest<IReadOnlyList<EnrollmentDto>>;
