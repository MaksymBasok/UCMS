using MediatR;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Application.Abstractions;
using UCMS.Application.Features.Submissions.Commands.CreateSubmission;
using UCMS.Application.Features.Submissions.Dtos;
using UCMS.Domain.Submissions;

public sealed class CreateSubmissionHandler
    : IRequestHandler<CreateSubmissionCommand, SubmissionDto>
{
    private readonly ISubmissionRepository _repo;
    private readonly IStudentRepository _studentRepo;
    private readonly IAssignmentRepository _assignmentRepo;
    private readonly IUnitOfWork _uow;

    public CreateSubmissionHandler(
        ISubmissionRepository repo,
        IStudentRepository studentRepo,
        IAssignmentRepository assignmentRepo,
        IUnitOfWork uow)
    {
        _repo = repo;
        _studentRepo = studentRepo;
        _assignmentRepo = assignmentRepo;
        _uow = uow;
    }

    public async Task<SubmissionDto> Handle(CreateSubmissionCommand r, CancellationToken ct)
    {
        if (await _studentRepo.GetByIdAsync(r.StudentId, ct) is null)
            throw new KeyNotFoundException($"Student {r.StudentId} not found");


        if (await _assignmentRepo.GetByIdAsync(r.AssignmentId, ct) is null)
            throw new KeyNotFoundException($"Assignment {r.AssignmentId} not found");

        var submission = Submission.New(Guid.NewGuid(), r.AssignmentId, r.StudentId,
                                        r.ContentUrl, r.SubmittedAtUtc);
        await _repo.AddAsync(submission, ct);
        await _uow.SaveChangesAsync(ct);
        return SubmissionDto.From(submission);
    }
}
