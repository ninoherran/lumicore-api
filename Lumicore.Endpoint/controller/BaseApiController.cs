using Lumicore.Domain.user;
using Lumicore.Endpoint.middleware;
using Microsoft.AspNetCore.Mvc;

namespace Lumicore.Endpoint.controller;

public abstract class BaseApiController : ControllerBase
{
    protected User? ConnectedUser
    {
        get
        {
            if (HttpContext.Items.TryGetValue(HttpContextItemKeys.ConnectedUser, out var value) && value is User u)
                return u;
            return null;
        }
    }

    protected bool HasConnectedUser => ConnectedUser is not null;

    protected User RequireConnectedUser()
    {
        var user = ConnectedUser;
        if (user is null)
            throw new UnauthorizedAccessException("User is not authenticated");
        return user;
    }
}