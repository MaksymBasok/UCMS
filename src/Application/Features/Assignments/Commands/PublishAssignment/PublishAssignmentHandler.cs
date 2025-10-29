using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Assignments.Dtos;

namespace UCMS.Application.Features.Assignments.Commands.PublishAssignment;

public sealed class PublishAssignmentHandler : IRequestHandler<PublishAssignmentCommand, AssignmentDto>
{
    private readonly IAssignmentRepository _repo;

    public PublishAssignmentHandler(IAssignmentRepository repo)
    {
        _repo = repo;
    }

    public async Task<AssignmentDto> Handle(PublishAssignmentCommand request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException("Assignment not found");

        entity.Publish();
        await _repo.UpdateAsync(entity, ct);

        return AssignmentDto.From(entity);
    }
}
