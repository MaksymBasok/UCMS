using MediatR;
using UCMS.Application.Features.Submissions.Dtos;

namespace UCMS.Application.Features.Submissions.Queries.GetSubmissionById;
public sealed record GetSubmissionByIdQuery(Guid Id) : IRequest<SubmissionDto?>;
