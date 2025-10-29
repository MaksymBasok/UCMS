using LanguageExt;
using MediatR;
using UCMS.Application.Abstractions.Repositories;

namespace UCMS.Application.Features.Assignments.Commands.PublishAssignment;

public sealed class PublishAssignmentHandler : IRequestHandler<PublishAssignmentCommand, Unit>
{
    private readonly IAssignmentRepository _repo;

    public PublishAssignmentHandler(IAssignmentRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(PublishAssignmentCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.Id, ct) ?? throw new KeyNotFoundException("Assignment not found");
        entity.Publish();

        await _repo.UpdateAsync(entity, ct);

        return Unit.Default;
    }
}
