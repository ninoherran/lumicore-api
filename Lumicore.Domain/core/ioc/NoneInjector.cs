using Lumicore.Domain.user.repository;

namespace Lumicore.Domain.core.ioc;

public class NoneInjector : IInjector
{
    public SetupService SetupService()
    {
        throw new NotImplementedException();
    }

    public UserRepository UserRepository()
    {
        throw new NotImplementedException();
    }
}