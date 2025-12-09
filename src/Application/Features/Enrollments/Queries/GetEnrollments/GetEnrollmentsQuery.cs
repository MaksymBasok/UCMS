using MediatR;
using UCMS.Application.Features.Enrollments.Dtos;

namespace UCMS.Application.Features.Enrollments.Queries.GetEnrollments;

public sealed record GetEnrollmentsQuery : IRequest<IReadOnlyList<EnrollmentDto>>;
