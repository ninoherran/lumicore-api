using Lumicore.Domain.role;
using Lumicore.Domain.user;
using Lumicore.Domain.user.repository;

namespace Lumicore.Domain.core.ioc;

public interface IInjector
{
    SetupService SetupService();
    UserRepository UserRepository();
    UserService UserService();
    RoleService RoleService();
    RoleRepository RoleRepository();
}