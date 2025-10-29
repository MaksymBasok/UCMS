using Microsoft.AspNetCore.Mvc;
using UCMS.Application.Features.Students.Exceptions;

namespace UCMS.Api.Modules.Errors;

public static class StudentErrorFactory
{
    public static ObjectResult ToObjectResult(this StudentException error)
    {
        return new ObjectResult(new { error = error.Message })
        {
            StatusCode = error switch
            {
                StudentAlreadyExistsException => StatusCodes.Status409Conflict,
                StudentNotFoundException => StatusCodes.Status404NotFound,
                StudentValidationException => StatusCodes.Status400BadRequest,
                StudentUnexpectedException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}
