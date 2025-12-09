using MediatR;
using UCMS.Application.Abstractions.Repositories;

namespace UCMS.Application.Features.Assignments.Commands.DeleteAssignment;

public sealed class DeleteAssignmentHandler : IRequestHandler<DeleteAssignmentCommand, Unit>
{
    private readonly IAssignmentRepository _repo;

    public DeleteAssignmentHandler(IAssignmentRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(DeleteAssignmentCommand request, CancellationToken ct)
    {
        var assignment = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException("Assignment not found");

        await _repo.RemoveAsync(assignment, ct);

        return Unit.Value;
    }
}
