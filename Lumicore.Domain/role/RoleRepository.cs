using Lumicore.Infra;

namespace Lumicore.Domain.role;

public class RoleRepository
{
    public void Save(Role role)
    {
        var statement = "INSERT INTO roles (id, name) Values($1, $2)";
        new PgCommand().ExecuteNonQuery(statement, PgParam.Guid(role.Id), PgParam.Text(role.Name));
    }

    public async Task<IEnumerable<Role>> GetAll()
    {
        var statement = "SELECT (id, name) From role";
        var results = await new PgCommand().ExecuteDataTable(statement);
        
        return results.Select(row => new Role((Guid)row[0], (string)row[1]));
    }
}