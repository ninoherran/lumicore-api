using System.Data;
using Npgsql;

namespace Lumicore.Infra;

public class PgCommand
{
    private readonly string _connectionString = DotEnv.Get("DB_STRING");

    public async void ExecuteNonQuery(string statement, params PgParam[] args)
    {
        await using var dataSource = NpgsqlDataSource.Create(_connectionString);
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

    public async Task<object[]> ExecuteDataRow(string statement, params PgParam[] args)
    {
        await using var dataSource = NpgsqlDataSource.Create(_connectionString);
        await using var initialCommand = dataSource.CreateCommand(statement);
        AddParams(initialCommand, args, out var command);

        await using var reader = await command.ExecuteReaderAsync();

        object[] result = new object[reader.FieldCount];
        if (await reader.ReadAsync())
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                result[i] = reader[i];
            }
        }


        return result;
    }

    public async Task<object?> ExecuteScalar(string statement, params PgParam[] args)
    {
        await using var dataSource = NpgsqlDataSource.Create(_connectionString);
        await using var command = dataSource.CreateCommand(statement);
        foreach (var arg in args)
        {
            var parm = new NpgsqlParameter { DbType = arg.Type, Value = arg.Value };
            command.Parameters.Add(parm);
        }

        return await command.ExecuteScalarAsync();
    }

    private void AddParams(NpgsqlCommand command, PgParam[] args, out NpgsqlCommand finalCommand)
    {
        foreach (var arg in args)
        {
            var parm = new NpgsqlParameter { DbType = arg.Type, Value = arg.Value };
            command.Parameters.Add(parm);
        }

        finalCommand = command;
    }

    public async Task<ICollection<object[]>> ExecuteDataTable(string statement, params PgParam[] args)
    {
        await using var dataSource = NpgsqlDataSource.Create(_connectionString);
        await using var initialCommand = dataSource.CreateCommand(statement);
        AddParams(initialCommand, args, out var command);

        await using var reader = await command.ExecuteReaderAsync();

        var result = new List<object[]>();

        while (await reader.ReadAsync())
        {
            object[] row = new object[reader.FieldCount];
            for (var i = 0; i < reader.FieldCount; i++)
            {
                row[i] = reader[i];
            }
            result.Add(row);
        }

        return result;
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