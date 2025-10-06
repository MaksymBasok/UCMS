using MediatR;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Features.Submissions.Dtos;

namespace UCMS.Application.Features.Submissions.Queries.GetSubmissionById;
public sealed class GetSubmissionByIdHandler : IRequestHandler<GetSubmissionByIdQuery, SubmissionDto?>
{
    private readonly ISubmissionQueries _q; public GetSubmissionByIdHandler(ISubmissionQueries q) => _q = q;
    public Task<SubmissionDto?> Handle(GetSubmissionByIdQuery r, CancellationToken ct) => _q.GetByIdAsync(r.Id, ct);
}
