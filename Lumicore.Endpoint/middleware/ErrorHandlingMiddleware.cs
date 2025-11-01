using System.Net;
using System.Text.Json;
using Lumicore.Domain.core.errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Lumicore.Endpoint.middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;

    public ErrorHandlingMiddleware(RequestDelegate next, IHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException dex)
        {
            await WriteDomainExceptionAsync(context, dex);
        }
        catch (Exception ex)
        {
            await WriteUnexpectedAsync(context, ex);
        }
    }

    private async Task WriteDomainExceptionAsync(HttpContext context, DomainException ex)
    {
        var status = MapStatusCode(ex.Code);
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";

        var payload = new
        {
            code = ex.Code.ToString(),
            message = ex.Message,
            details = ex.Details
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }

    private async Task WriteUnexpectedAsync(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var payload = new
        {
            code = ErrorCode.UnexpectedError.ToString(),
            message = _env.IsDevelopment() ? ex.Message : "An unexpected error occurred.",
            traceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }

    private static HttpStatusCode MapStatusCode(ErrorCode code)
    {
        return code switch
        {
            ErrorCode.ValidationError => HttpStatusCode.BadRequest,
            ErrorCode.Unauthorized => HttpStatusCode.Unauthorized,
            ErrorCode.InvalidPassword => HttpStatusCode.Unauthorized,
            ErrorCode.Forbidden => HttpStatusCode.Forbidden,
            ErrorCode.UserNotWhitelisted => HttpStatusCode.Forbidden,
            ErrorCode.UserNotFound => HttpStatusCode.NotFound,
            ErrorCode.Conflict => HttpStatusCode.Conflict,
            _ => HttpStatusCode.InternalServerError
        };
    }
}
