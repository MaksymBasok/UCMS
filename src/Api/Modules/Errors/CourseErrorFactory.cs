using Microsoft.AspNetCore.Mvc;
using UCMS.Application.Features.Courses.Exceptions;

namespace UCMS.Api.Modules.Errors;

public static class CourseErrorFactory
{
    public static ObjectResult ToObjectResult(this CourseException error)
    {
        return new ObjectResult(new { error = error.Message })
        {
            StatusCode = error switch
            {
                CourseAlreadyExistsException => StatusCodes.Status409Conflict,
                CourseNotFoundException => StatusCodes.Status404NotFound,
                CourseValidationException => StatusCodes.Status400BadRequest,
                CourseUnexpectedException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}
