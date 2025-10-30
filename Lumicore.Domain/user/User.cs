namespace Lumicore.Domain.user;

public class User
{
    public User(string email, string fullname, string password)
    {
        Id = Guid.NewGuid();
        Email = email;
        FullName = fullname;
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, 13);
        IsAdmin = true;
    }

    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string PasswordHash { get; set; }
    public bool IsAdmin { get; set; }
    
    public Guid RoleId { get; set; }
}