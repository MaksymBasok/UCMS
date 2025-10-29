using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Assignments.Dtos;

namespace UCMS.Application.Features.Assignments.Commands.CloseAssignment;

public sealed class CloseAssignmentHandler : IRequestHandler<CloseAssignmentCommand, AssignmentDto>
{
    private readonly IAssignmentRepository _repo;

    public CloseAssignmentHandler(IAssignmentRepository repo)
    {
        _repo = repo;
    }

    public async Task<AssignmentDto> Handle(CloseAssignmentCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException("Assignment not found");

        entity.Close();
        await _repo.UpdateAsync(entity, ct);

        return AssignmentDto.From(entity);
    }
}
