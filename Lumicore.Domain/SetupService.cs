using Lumicore.Domain.core.ioc;
using Lumicore.Domain.user;

namespace Lumicore.Domain;

public class SetupService
{
    public async Task<bool> IsInit()
    {
        return await Locator.UserRepository().HasAnyUser();
    }

    public async Task Init(string email, string fullname, string password)
    {
        var hasAnyUser = await Locator.UserRepository().HasAnyUser();
        if (hasAnyUser)
            throw new Exception("Setup already completed");
        
        var user = new User(email, fullname, password);
        Locator.UserRepository().Add(user);
    }
}