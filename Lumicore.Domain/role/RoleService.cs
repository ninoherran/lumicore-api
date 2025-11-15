using Lumicore.Domain.core.ioc;
using Lumicore.Domain.organization;

namespace Lumicore.Domain.role;

public class RoleService
{
    public void Create(string name)
    {
        var role = new Role(name);
        Locator.RoleRepository().Save(role);
    }

    public Task<IEnumerable<Role>> GetAll()
    {
        return Locator.RoleRepository().GetAll();
    }
}