using Lumicore.Domain.core.ioc;
using Lumicore.Domain.core.errors;

namespace Lumicore.Domain.user;

public class UserService
{
    public User CreateUserAdmin(string email, string firstname, string lastname, string password)
    {
        var user = new User(email, firstname, lastname, password) { IsAdmin = true };
        Locator.UserRepository().Add(user);

        return user;
    }
    
    public async Task<User> CreateUser(string email, string firstname, string lastname, string password)
    {
        var isUserWhitelisted = await Locator.UserRepository().IsUserWhitelisted(email);
        if (!isUserWhitelisted)
            throw DomainErrors.UserNotWhitelisted(email);
        
        var user = new User(email, firstname, lastname, password);
        Locator.UserRepository().Add(user);

        return user;
    }

    public async Task<User> Authenticate(string email, string password)
    {
        var user = await Locator.UserRepository().GetByEmail(email);
        
        user.CheckPassword(password);
        
        return user;
    }

    public void AddToWhiteList(string email)
    {
        Locator.UserRepository().AddToWhiteList(email);
    }
}