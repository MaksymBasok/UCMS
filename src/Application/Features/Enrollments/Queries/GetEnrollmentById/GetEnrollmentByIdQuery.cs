using MediatR;
using UCMS.Application.Features.Enrollments.Dtos;

namespace UCMS.Application.Features.Enrollments.Queries.GetEnrollmentById;

public sealed record GetEnrollmentByIdQuery(Guid Id) : IRequest<EnrollmentDto?>;
