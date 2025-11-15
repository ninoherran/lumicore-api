using Lumicore.Domain.role;
using Lumicore.Domain.user;
using Lumicore.Domain.user.repository;

namespace Lumicore.Domain.core.ioc;

public class ProdInjector : IInjector
{
    public SetupService SetupService() => new();
    public UserRepository UserRepository() => new();
    public UserService UserService() => new();
    public RoleService RoleService() => new();
    public RoleRepository RoleRepository() => new ();
}