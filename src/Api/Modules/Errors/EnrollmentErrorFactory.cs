using Microsoft.AspNetCore.Mvc;
using UCMS.Application.Features.Enrollments.Exceptions;

namespace UCMS.Api.Modules.Errors;

public static class EnrollmentErrorFactory
{
    public static ObjectResult ToObjectResult(this EnrollmentException error)
    {
        return new ObjectResult(new { error = error.Message })
        {
            StatusCode = error switch
            {
                EnrollmentAlreadyExistsException => StatusCodes.Status409Conflict,
                EnrollmentNotFoundException => StatusCodes.Status404NotFound,
                EnrollmentValidationException => StatusCodes.Status400BadRequest,
                EnrollmentUnexpectedException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}
