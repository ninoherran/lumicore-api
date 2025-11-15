using Lumicore.Domain.core.ioc;
using Lumicore.Endpoint.auth;
using Microsoft.AspNetCore.Mvc;

namespace Lumicore.Endpoint.controller.role;

[ApiController]
[Route("[controller]")]
public class RoleController : BaseApiController
{
    [NeedAdmin]
    [HttpPost]
    public async Task<IActionResult> CreateRole([FromQuery] string name)
    {
        Locator.RoleService().Create(name);
        
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await Locator.RoleService().GetAll();
        
        return Ok(roles);
    }
}