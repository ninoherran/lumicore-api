using Lumicore.Infra;

namespace Lumicore.Domain.user.repository;

public class UserRepository
{
    public virtual void Add(User user)
    {
        var statement = "INSERT INTO users (id, email, fullname, password, is_admin) Values($1, $2, $3, $4, $5)";
        
        _ = new PgCommand().ExecuteNonQuery(statement, PgParam.Guid(user.Id), PgParam.Text(user.Email), PgParam.Text(user.FullName), PgParam.Text(user.PasswordHash), PgParam.Bool(user.IsAdmin));
    }

    public virtual async Task<bool> HasAnyUser()
    {
        var statement = "SELECT CASE WHEN EXISTS(SELECT 1 FROM users LIMIT 1) THEN 0 ELSE 1 END";
        var result = (int)await new PgCommand().ExecuteSacalar(statement);
        
        return result != 1;
    }
}