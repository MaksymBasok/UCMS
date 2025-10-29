namespace UCMS.Application.Features.Assignments.Commands.CloseAssignment;
using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Assignments.Dtos;

public sealed class CloseAssignmentHandler : IRequestHandler<CloseAssignmentCommand, AssignmentDto>
{
    private readonly IAssignmentRepository _repo; private readonly IUnitOfWork _uow;
    public CloseAssignmentHandler(IAssignmentRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<AssignmentDto> Handle(CloseAssignmentCommand r, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(r.Id, ct) ?? throw new KeyNotFoundException("Assignment not found");
        entity.Close();
        await _uow.SaveChangesAsync(ct);
        return AssignmentDto.From(entity);
    }
}
