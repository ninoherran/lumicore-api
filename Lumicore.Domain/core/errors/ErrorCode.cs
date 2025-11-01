namespace Lumicore.Domain.core.errors;

public enum ErrorCode
{
    ValidationError = 1000,
    Unauthorized = 1001,
    Forbidden = 1003,
    UserNotFound = 1404,
    UserNotWhitelisted = 1405,
    InvalidPassword = 1406,
    Conflict = 1609,
    UnexpectedError = 1500
}
