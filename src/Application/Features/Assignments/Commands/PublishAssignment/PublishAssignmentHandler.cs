namespace UCMS.Application.Features.Assignments.Commands.PublishAssignment;
using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Assignments.Dtos;

public sealed class PublishAssignmentHandler : IRequestHandler<PublishAssignmentCommand, AssignmentDto>
{
    private readonly IAssignmentRepository _repo; private readonly IUnitOfWork _uow;
    public PublishAssignmentHandler(IAssignmentRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<AssignmentDto> Handle(PublishAssignmentCommand r, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(r.Id, ct) ?? throw new KeyNotFoundException("Assignment not found");
        entity.Publish();
        await _uow.SaveChangesAsync(ct);
        return AssignmentDto.From(entity);
    }
}
