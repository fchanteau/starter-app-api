using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Starter.WebApi.Controllers;
[Route("api/test")]
[ApiController]
public class TestController(IConfiguration configuration) : ApiController
{
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [HttpGet]
    public IActionResult Get()
    {
        return true.ToErrorOr().Match(
            _ => Ok("Success"),
            Problem
        );
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [HttpGet("config")]
    public IActionResult GetConfigValue()
    {
        var configValue = configuration["ConfigSection:Value"];

        if(string.IsNullOrEmpty(configValue))
        {
            return BadRequest("No config found");

        }
        return Ok(configValue);
    }
}
