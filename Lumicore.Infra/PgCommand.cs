using System.Data;
using Npgsql;

namespace Lumicore.Infra;

public class PgCommand
{
    private readonly string connectionString =
        "Host=ep-broad-river-abth5ezq-pooler.eu-west-2.aws.neon.tech; Username=neondb_owner; Password=npg_nUFvuarpO9G6; Database=neondb;";

    public async Task ExecuteNonQuery(string statement, params PgParam[] args)
    {
        await using var dataSource = NpgsqlDataSource.Create(connectionString);
        await using var command = dataSource.CreateCommand(statement);

        foreach (var arg in args)
        {
            var parm = new NpgsqlParameter();
            parm.DbType = arg.Type;
            parm.Value = arg.Value;
            command.Parameters.Add(parm);
        }

        await command.ExecuteNonQueryAsync();
    }

    public async Task<object[]> ExecuteDataRow(string statement)
    {
        await using var dataSource = NpgsqlDataSource.Create(connectionString);
        await using var command = dataSource.CreateCommand(statement);
        await using var reader = await command.ExecuteReaderAsync();
        object[] result = new object[reader.FieldCount];

        while (reader.Read())
        {
            for (var i = 0; i < reader.FieldCount; i++ )
            {
                result[i] = reader[i];
            }
        }

        return result;
    }
    

    public async Task<object> ExecuteSacalar(string statement)
    {
        await using var dataSource = NpgsqlDataSource.Create(connectionString);
        await using var command = dataSource.CreateCommand(statement);

        return await command.ExecuteScalarAsync();
    }
}

public class PgParam
{
    public DbType Type { get; set; }
    public object Value { get; set; }

    public static PgParam Int(int value) => new() { Type = DbType.Int32, Value = value };
    public static PgParam Text(string value) => new() { Type = DbType.String, Value = value };
    public static PgParam Guid(Guid value) => new() { Type = DbType.Guid, Value = value };

    public static PgParam Bool(bool boolean) => new() { Type = DbType.Boolean, Value = boolean };
}