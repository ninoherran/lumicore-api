using Lumicore.Domain.core.ioc;
using Lumicore.Endpoint.auth;
using Lumicore.Endpoint.controller.dto;
using Lumicore.Endpoint.controller.user.dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lumicore.Endpoint.controller.user;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : BaseApiController
{
    [NeedAdmin]
    [HttpPost("whitelist")]
    public IActionResult Post([FromBody] WhitelistDto whitelistDto)
    {
        Locator.UserService().AddToWhiteList(whitelistDto.Email);
        
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<IActionResult> Get([FromBody] UserRegisterDto userRegisterDto)
    {
        var user = await Locator.UserService().CreateUser(userRegisterDto.Email, userRegisterDto.Firstname, userRegisterDto.Lastname, userRegisterDto.Password);
        
        return Ok(user.Id);
    }
}