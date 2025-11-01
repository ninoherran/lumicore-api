using System;

namespace Lumicore.Domain.core.errors;

public class DomainException : Exception
{
    public ErrorCode Code { get; }
    public object? Details { get; }

    public DomainException(ErrorCode code, string message, object? details = null, Exception? innerException = null)
        : base(message, innerException)
    {
        Code = code;
        Details = details;
    }

    public override string ToString()
    {
        return $"{Code}: {Message}";
    }
}
