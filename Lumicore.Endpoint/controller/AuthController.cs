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

        return Ok(token);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Get()
    {
        var user = new MeDto()
        {
            Id = ConnectedUser.Id.ToString(),
            Email = ConnectedUser.Email,
            Firstname = ConnectedUser.Firstname,
            Lastname = ConnectedUser.Lastname
        };
        
        return Ok(user);   
    }
}
