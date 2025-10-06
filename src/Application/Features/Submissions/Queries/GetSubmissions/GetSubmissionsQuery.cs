using MediatR;
using UCMS.Application.Features.Submissions.Dtos;

namespace UCMS.Application.Features.Submissions.Queries.GetSubmissions;
public sealed record GetSubmissionsQuery() : IRequest<IReadOnlyList<SubmissionDto>>;
