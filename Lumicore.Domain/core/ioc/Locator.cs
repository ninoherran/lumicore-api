using Lumicore.Domain.role;
using Lumicore.Domain.user;
using Lumicore.Domain.user.repository;

namespace Lumicore.Domain.core.ioc;

public static class Locator
{
    private static IInjector _injector { get; set; } = new NoneInjector();

    public static SetupService SetupService() => _injector.SetupService();
    public static UserRepository UserRepository() => _injector.UserRepository();
    public static UserService UserService() => _injector.UserService();


    public static void Load(IInjector injector)
    {
        _injector = injector;
    }

    public static RoleService RoleService() => _injector.RoleService();

    public static RoleRepository RoleRepository() => _injector.RoleRepository();
}