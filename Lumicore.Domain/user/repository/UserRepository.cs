using Lumicore.Infra;
using Lumicore.Domain.core.errors;

namespace Lumicore.Domain.user.repository;

public class UserRepository
{
    public virtual void Add(User user)
    {
        var statement = "INSERT INTO users (id, email, firstname, lastname, password, is_admin) Values($1, $2, $3, $4, $5, $6)";
        
        new PgCommand().ExecuteNonQuery(statement, PgParam.Guid(user.Id), PgParam.Text(user.Email), PgParam.Text(user.Firstname), PgParam.Text(user.Lastname), PgParam.Text(user.PasswordHash), PgParam.Bool(user.IsAdmin));
    }

    public virtual async Task<bool> HasAnyUser()
    {
        var statement = "SELECT CASE WHEN EXISTS(SELECT 1 FROM users LIMIT 1) THEN 0 ELSE 1 END";
        var result = (int)(await new PgCommand().ExecuteScalar(statement) ?? 0);
        
        return result != 1;
    }

    public virtual async Task<User> GetByEmail(string email)
    {
        const string statement = "SELECT id, email, firstname, lastname, password, is_admin FROM users WHERE email = $1 LIMIT 1";
        var row = await new PgCommand().ExecuteDataRow(statement, PgParam.Text(email));
        
        return CreateUserFromRow(row);
    }

    public async Task<User> GetById(Guid id)
    {
        const string statement = "SELECT id, email, firstname, lastname, password, is_admin FROM users WHERE id = $1 LIMIT 1";
        var row = await new PgCommand().ExecuteDataRow(statement, PgParam.Guid(id));
        
        return CreateUserFromRow(row);
    }

    private User CreateUserFromRow(object[] row)
    {
        if (row == null || row.Length == 0 || row[0] is null)
            throw DomainErrors.UserNotFound();

        return new User((Guid)row[0], (string)row[1], (string)row[2], (string)row[3], (string)row[4], (bool)row[5]);
    }

    public virtual void AddToWhiteList(string email)
    {
        var statement = "INSERT INTO whitelist (email) Values($1)";
        new PgCommand().ExecuteNonQuery(statement, PgParam.Text(email));
    }

    public virtual async Task<bool> IsUserWhitelisted(string email)
    {
        var statement = "SELECT CASE WHEN EXISTS(SELECT 1 FROM whitelist where email = $1) THEN 0 ELSE 1 END";
        var result = (int)(await new PgCommand().ExecuteScalar(statement, PgParam.Text(email)) ?? 0);
        
        return result != 1;
    }
}