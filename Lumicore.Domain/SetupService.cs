using Lumicore.Domain.core.ioc;
using Lumicore.Domain.core.errors;
using Lumicore.Domain.user;

namespace Lumicore.Domain;

public class SetupService
{
    public async Task<bool> IsInit()
    {
        return await Locator.UserRepository().HasAnyUser();
    }

    public async Task Init(string email, string firstname, string lastname, string password)
    {
        var hasAnyUser = await Locator.UserRepository().HasAnyUser();

        if (hasAnyUser)
            throw DomainErrors.SetupAlreadyCompleted();
        
        Locator.UserService().CreateUserAdmin(email, firstname, lastname, password);
    }
}