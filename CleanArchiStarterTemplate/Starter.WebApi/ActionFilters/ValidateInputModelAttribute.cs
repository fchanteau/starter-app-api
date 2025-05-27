using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Starter.WebApi.ActionFilters;

public class ValidateInputModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if(!context.ModelState.IsValid)
        {
            var problemDetailsFactory = context.HttpContext.RequestServices.GetService<ProblemDetailsFactory>()!;

            var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                context.HttpContext,
                context.ModelState,
                statusCode: 400,
                title: "ValidationFailed");
            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = 400
            };
        }
    }
}
