using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Starter.Application.Features.Users.Queries;
using Starter.Contracts.Users;

namespace Starter.WebApi.Controllers;
[Route("api/test")]
[ApiController]
public class TestController(IMediator mediator, ILogger<TestController> logger) : ApiController
{
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserResponse>))]
    [HttpGet]
    public async Task<ActionResult> GetUsers()
    {
        return await mediator.Send(new GetUsersQuery())
            .Match(response =>
                Ok(response.Users.Select(u => new UserResponse(u.UserId, u.Username))),
                Problem);
    }

    [HttpGet("test-logs")]
    public IActionResult TestLogs()
    {
        var testId = Guid.NewGuid().ToString("N")[..8];

        logger.LogDebug("DEBUG - Test logs {TestId}", testId);
        logger.LogInformation("INFO - Test logs {TestId}", testId);
        logger.LogWarning("WARNING - Test logs {TestId}", testId);
        logger.LogError("ERROR - Test logs {TestId}", testId);

        return Ok(new
        {
            Message = "Logs testing send",
            TestId = testId
        });
    }
}
