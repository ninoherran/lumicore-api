using Lumicore.Domain.core.ioc;
using Lumicore.Endpoint.controller.dto;
using Microsoft.AspNetCore.Mvc;

namespace Lumicore.Endpoint.controller;

[ApiController]
[Route("[controller]")]
public class SetupController : ControllerBase
{
    [HttpGet("is-init")]
    public async Task<IActionResult> IsInit()
    {
        var firstUserExists = await Locator.SetupService().IsInit();

        if (!firstUserExists)
            return BadRequest();

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Init([FromBody] SetupDto setupDto)
    {
        try
        {
            await Locator.SetupService().Init(setupDto.Email, setupDto.Fullname, setupDto.Password);
            return Ok();
        }catch
        {
            return BadRequest();
        }
    }
}