namespace Lumicore.Domain.core.errors;

/// <summary>
/// Centralized factory for creating domain exceptions with standardized messages and optional details.
/// Prefer using these helpers instead of instantiating <see cref="DomainException"/> directly.
/// </summary>
public static class DomainErrors
{
    public static DomainException UserNotWhitelisted(string email)
        => new DomainException(ErrorCode.UserNotWhitelisted, "User not whitelisted", new { email });

    public static DomainException InvalidPassword()
        => new DomainException(ErrorCode.InvalidPassword, "Invalid password");

    public static DomainException UserNotFound(object? details = null)
        => new DomainException(ErrorCode.UserNotFound, "User not found", details);

    public static DomainException SetupAlreadyCompleted()
        => new DomainException(ErrorCode.Conflict, "Setup already completed");

    // Generic helpers (optional, for future use)
    public static DomainException Validation(string message, object? details = null)
        => new DomainException(ErrorCode.ValidationError, message, details);

    public static DomainException Forbidden(string message, object? details = null)
        => new DomainException(ErrorCode.Forbidden, message, details);

    public static DomainException Unauthorized(string message, object? details = null)
        => new DomainException(ErrorCode.Unauthorized, message, details);

    public static DomainException Unexpected(string message, object? details = null)
        => new DomainException(ErrorCode.UnexpectedError, message, details);
}
