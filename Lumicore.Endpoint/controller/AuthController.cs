using Lumicore.Domain.core.ioc;
using Lumicore.Endpoint.auth;
using Lumicore.Endpoint.controller.dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lumicore.Endpoint.controller;

[ApiController]
[Route("[controller]")]
public class AuthController(IJwtTokenFactory tokenFactory) : BaseApiController
{
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Email et mot de passe sont requis.");

        var user = await Locator.UserService().Authenticate(request.Email, request.Password);

        var token = tokenFactory.CreateToken(user);

        return Ok(new{ token});
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
    {
        var user = await Locator.UserService().CreateUser(userRegisterDto.Email, userRegisterDto.Firstname, userRegisterDto.Lastname, userRegisterDto.Password);
        
        return Ok(user.Id);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Get()
    {
        var user = new UserDto()
        {
            Id = ConnectedUser.Id.ToString(),
            Email = ConnectedUser.Email,
            FirstName = ConnectedUser.FirstName,
            LastName = ConnectedUser.LastName,
            IsAdmin = ConnectedUser.IsAdmin
        };
        
        return Ok(user);   
    }
}
