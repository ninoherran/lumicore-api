using Lumicore.Domain.user.repository;

namespace Lumicore.Domain.core.ioc;

public interface IInjector
{
    SetupService SetupService();
    UserRepository UserRepository();
}