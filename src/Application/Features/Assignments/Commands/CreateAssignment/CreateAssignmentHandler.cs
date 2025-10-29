using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Features.Assignments.Dtos;
using UCMS.Domain.Assignments;

namespace UCMS.Application.Features.Assignments.Commands.CreateAssignment;

public sealed class CreateAssignmentHandler : IRequestHandler<CreateAssignmentCommand, AssignmentDto>
{
    private readonly IAssignmentRepository _repo;

    public CreateAssignmentHandler(IAssignmentRepository repo)
    {
        _repo = repo;
    }

    public async Task<AssignmentDto> Handle(CreateAssignmentCommand request, CancellationToken ct)
    {
        var assignment = Assignment.New(
            Guid.NewGuid(),
            request.CourseId,
            request.Title,
            request.Description,
            request.DueDate);

        await _repo.AddAsync(assignment, ct);

        return AssignmentDto.From(assignment);
    }
}
