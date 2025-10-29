using Microsoft.AspNetCore.Mvc;

namespace Lumicore.Endpoint.controller;

[ApiController]
[Route("[controller]")]
public class SetupController : ControllerBase
{
    [HttpGet("is-init")]
    public IActionResult IsInit()
    {
        return BadRequest();
    }
}