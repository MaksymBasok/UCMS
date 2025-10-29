using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Assignments.Dtos;

namespace UCMS.Application.Features.Assignments.Commands.UpdateAssignment;

public sealed class UpdateAssignmentHandler : IRequestHandler<UpdateAssignmentCommand, AssignmentDto>
{
    private readonly IAssignmentRepository _repo;

    public UpdateAssignmentHandler(IAssignmentRepository repo)
    {
        _repo = repo;
    }

    public async Task<AssignmentDto> Handle(UpdateAssignmentCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.Id, ct) ?? throw new KeyNotFoundException("Assignment not found");
        entity.Update(request.Title, request.Description, request.DueDate);

        await _repo.UpdateAsync(entity, ct);

        return AssignmentDto.From(entity);
    }
}
