using Lumicore.Domain.core.ioc;
using Lumicore.Endpoint.controller.dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lumicore.Endpoint.controller;

[ApiController]
[Route("[controller]")]
public class SetupController : BaseApiController
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
    public async Task<IActionResult> Init([FromBody] UserRegisterDto userRegisterDto)
    {
        await Locator.SetupService().Init(userRegisterDto.Email, userRegisterDto.Firstname, userRegisterDto.Lastname, userRegisterDto.Password);
        return Ok();
    }

    [Authorize]
    [HttpGet]
    public IActionResult Get()
    {
        Console.WriteLine(ConnectedUser?.Email);
        
        return Ok();
    }
}