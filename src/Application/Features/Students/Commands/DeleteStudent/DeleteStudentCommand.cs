using MediatR;
using UCMS.Application.Features.Students.Exceptions;

namespace UCMS.Application.Features.Students.Commands.DeleteStudent;

public sealed record DeleteStudentCommand(Guid Id)
    : IRequest<LanguageExt.Either<StudentException, LanguageExt.Unit>>;
