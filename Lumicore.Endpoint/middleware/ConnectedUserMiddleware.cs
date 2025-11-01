using System.Security.Claims;
using Lumicore.Domain.core.ioc;
using Microsoft.AspNetCore.Authorization;

namespace Lumicore.Endpoint.middleware;

public class ConnectedUserMiddleware
{
    private readonly RequestDelegate _next;

    public ConnectedUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var allowsAnonymous = endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null;
        var hasAuthorize = endpoint?.Metadata.GetMetadata<IAuthorizeData>() != null;
        
        if (!hasAuthorize || allowsAnonymous)
        {
            await _next(context);
            return;
        }

        if (context.User?.Identity is { IsAuthenticated: true })
        {
            try
            {
                var sub = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrWhiteSpace(sub) && Guid.TryParse(sub, out var id))
                {
                    var user = await Locator.UserRepository().GetById(id);
                    context.Items[HttpContextItemKeys.ConnectedUser] = user;
                }
            }
            catch
            {
                // Do not block the pipeline; if the user cannot be loaded, leave it empty.
            }
        }

        await _next(context);
    }
}

internal static class HttpContextItemKeys
{
    public const string ConnectedUser = "ConnectedUser";
}