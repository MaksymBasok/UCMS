using LanguageExt;
using MediatR;
using UCMS.Application.Features.Students.Exceptions;

using Unit = LanguageExt.Unit;

namespace UCMS.Application.Features.Students.Commands.DeleteStudent;

public sealed record DeleteStudentCommand(Guid Id) : IRequest<Either<StudentException, Unit>>;
