using LanguageExt;
using MediatR;
using UCMS.Application.Abstractions.Repositories;

namespace UCMS.Application.Features.Assignments.Commands.CloseAssignment;

public sealed class CloseAssignmentHandler : IRequestHandler<CloseAssignmentCommand, Unit>
{
    private readonly IAssignmentRepository _repo;

    public CloseAssignmentHandler(IAssignmentRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(CloseAssignmentCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.Id, ct) ?? throw new KeyNotFoundException("Assignment not found");
        entity.Close();

        await _repo.UpdateAsync(entity, ct);

        return Unit.Default;
    }
}
