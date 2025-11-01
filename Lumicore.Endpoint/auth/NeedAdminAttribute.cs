using System;
using Lumicore.Domain.user;
using Lumicore.Endpoint.middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lumicore.Endpoint.auth;

/// <summary>
/// Attribute to restrict access to administrators only.
/// Returns 401 when no authenticated user and 403 when authenticated but not admin.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class NeedAdminAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Items.TryGetValue(HttpContextItemKeys.ConnectedUser, out var value) || value is not User user)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!user.IsAdmin)
        {
            context.Result = new ForbidResult();
        }
    }
}
