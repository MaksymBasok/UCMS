using Microsoft.AspNetCore.Mvc;
using UCMS.Application.Features.CourseSchedules.Exceptions;

namespace UCMS.Api.Modules.Errors;

public static class CourseScheduleErrorFactory
{
    public static ObjectResult ToObjectResult(this CourseScheduleException error)
    {
        return new ObjectResult(new { error = error.Message })
        {
            StatusCode = error switch
            {
                CourseScheduleNotFoundException => StatusCodes.Status404NotFound,
                CourseScheduleValidationException => StatusCodes.Status400BadRequest,
                CourseScheduleUnexpectedException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}
