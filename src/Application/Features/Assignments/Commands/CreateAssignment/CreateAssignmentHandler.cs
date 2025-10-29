namespace UCMS.Application.Features.Assignments.Commands.CreateAssignment;
using MediatR;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Assignments.Dtos;
using UCMS.Domain.Assignments;

public sealed class CreateAssignmentHandler : IRequestHandler<CreateAssignmentCommand, AssignmentDto>
{
    private readonly IAssignmentRepository _repo;
    private readonly IUnitOfWork _uow;
    public CreateAssignmentHandler(IAssignmentRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<AssignmentDto> Handle(CreateAssignmentCommand r, CancellationToken ct)
    {
        var entity = Assignment.New(Guid.NewGuid(), r.CourseId, r.Title, r.Description, r.DueDate);
        await _repo.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return AssignmentDto.From(entity);
    }
}
