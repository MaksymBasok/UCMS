using MediatR;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.Submissions.Dtos;

namespace UCMS.Application.Features.Submissions.Queries.GetSubmissions;
public sealed class GetSubmissionsHandler : IRequestHandler<GetSubmissionsQuery, IReadOnlyList<SubmissionDto>>
{
    private readonly ISubmissionQueries _q; public GetSubmissionsHandler(ISubmissionQueries q) => _q = q;
    public Task<IReadOnlyList<SubmissionDto>> Handle(GetSubmissionsQuery r, CancellationToken ct) => _q.GetAllAsync(ct);
}
