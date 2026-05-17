using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace WebApi.Controllers;

[ApiController]
public abstract class ApiController : ControllerBase
{
    protected ActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok();
        }

        return MapFailure(result);
    }

    protected ActionResult HandleResult<TValue>(Result<TValue> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return MapFailure(result);
    }

    private ActionResult MapFailure(Result result)
    {
        return result.Error.Type switch
        {
            ErrorType.NotFound => NotFound(CreateProblemDetails("Not Found", StatusCodes.Status404NotFound, result.Error)),
            ErrorType.Conflict => Conflict(CreateProblemDetails("Conflict", StatusCodes.Status409Conflict, result.Error)),
            ErrorType.Validation => BadRequest(CreateProblemDetails("Validation Failure", StatusCodes.Status400BadRequest, result.Error, GetErrors(result.Error))),
            ErrorType.Problem => BadRequest(CreateProblemDetails("Problem", StatusCodes.Status400BadRequest, result.Error)),
            _ => BadRequest(CreateProblemDetails("Bad Request", StatusCodes.Status400BadRequest, result.Error))
        };
    }

    private static Error[] GetErrors(Error error) =>
        error is ValidationError validationError ? validationError.Errors : [];

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null)
    {
        var problemDetails = new ProblemDetails
        {
            Title = title,
            Status = status,
            Detail = error.Description,
            Extensions = { { "code", error.Code } }
        };

        if (errors is not null && errors.Length > 0)
        {
            problemDetails.Extensions.Add("errors", errors);
        }

        return problemDetails;
    }
}
