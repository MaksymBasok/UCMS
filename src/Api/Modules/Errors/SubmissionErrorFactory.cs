using Microsoft.AspNetCore.Mvc;
using UCMS.Application.Features.Submissions.Exceptions;

namespace UCMS.Api.Modules.Errors;

public static class SubmissionErrorFactory
{
    public static ObjectResult ToObjectResult(this SubmissionException error)
    {
        return new ObjectResult(new { error = error.Message })
        {
            StatusCode = error switch
            {
                SubmissionAlreadyExistsException => StatusCodes.Status409Conflict,
                SubmissionNotFoundException => StatusCodes.Status404NotFound,
                SubmissionValidationException => StatusCodes.Status400BadRequest,
                SubmissionUnexpectedException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}
