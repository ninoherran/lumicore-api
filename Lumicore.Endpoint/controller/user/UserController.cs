using Lumicore.Domain.core.ioc;
using Lumicore.Endpoint.auth;
using Lumicore.Endpoint.controller.dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lumicore.Endpoint.controller.user;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : BaseApiController
{
    [NeedAdmin]
    [HttpGet("whitelist")]
    public async Task<IActionResult> GetWhitelist()
    {
        var whiteList = await Locator.UserService().GetWhiteList();
        
        return Ok(whiteList);
    }

    [NeedAdmin]
    [HttpDelete("whitelist")]
    public IActionResult Delete([FromQuery] string email)
    {
        Locator.UserService().DeleteFromWhitelist(email);
        
        return Ok();
    }
    
    
    [NeedAdmin]
    [HttpPost("whitelist")]
    public IActionResult AddToWhitelist([FromQuery] string email)
    {
        Locator.UserService().AddToWhiteList(email);
        
        return Ok();
    }

    [NeedAdmin]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = (await Locator.UserService().GetAll()).Select(user => new UserDto()
        {
            Id = user.Id.ToString(),
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsAdmin = user.IsAdmin,
        });
        return Ok(users);
    }
}