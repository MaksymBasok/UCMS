namespace UCMS.Application.Features.Assignments.Commands.UpdateAssignment;
using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Assignments.Dtos;

public sealed class UpdateAssignmentHandler : IRequestHandler<UpdateAssignmentCommand, AssignmentDto>
{
    private readonly IAssignmentRepository _repo; private readonly IUnitOfWork _uow;
    public UpdateAssignmentHandler(IAssignmentRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<AssignmentDto> Handle(UpdateAssignmentCommand r, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(r.Id, ct) ?? throw new KeyNotFoundException("Assignment not found");
        entity.Update(r.Title, r.Description, r.DueDate);
        await _uow.SaveChangesAsync(ct);
        return AssignmentDto.From(entity);
    }
}
