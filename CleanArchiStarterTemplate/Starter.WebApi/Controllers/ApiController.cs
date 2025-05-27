using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Starter.WebApi.Controllers;

public class ApiController : ControllerBase
{
    protected ActionResult Problem(List<Error> errors)
    {
        if(errors.Count is 0)
        {
            return Problem();
        }

        //if (errors.All (error => error.Type == ErrorType.Validation))
        //{
        //    return ValidationProblem (errors);
        //}

        return Problem(errors[0]);
    }

    private ObjectResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status403Forbidden,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Problem(statusCode: statusCode, title: error.Code, detail: error.Description);
    }
}
