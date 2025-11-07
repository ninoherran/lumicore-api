using Lumicore.Domain.core.errors;

namespace Lumicore.Domain.user;

public class User(Guid id, string email, string firstName, string lastName, string passwordHash, bool isAdmin)
{
    public User(string email, string firstName, string lastName, string password) : this(Guid.NewGuid(), email, firstName, lastName, BCrypt.Net.BCrypt.HashPassword(password, 13), true)
    {
    }

    public Guid Id { get; set; } = id;
    public string Email { get; set; } = email;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string PasswordHash { get; set; } = passwordHash;
    public bool IsAdmin { get; set; } = isAdmin;

    public Guid RoleId { get; set; }

    public void CheckPassword(string password)
    {
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        if (!isPasswordValid)
            throw DomainErrors.InvalidPassword();
    }
}