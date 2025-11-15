using System.Reflection;
using DbUp;
using Lumicore.Infra;

string? envPath = "../../../../Lumicore.Endpoint/.env";
if (!string.IsNullOrWhiteSpace(envPath))
{
    try
    {
        DotEnv.Load(envPath!, overwrite: false);
        Console.WriteLine($"Loaded environment from {Path.GetFullPath(envPath!)}");
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to load .env: {ex.Message}");
    }
}
else
{
    Console.WriteLine("No .env file found automatically. You can pass a path as the first argument.");
}

var connectionString = Environment.GetEnvironmentVariable("DB_STRING");
if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.Error.WriteLine("Environment variable 'POSTGRES_CONNECTION_STRING' is not set.");
    return 2;
}

try
{
    // Ensure database exists (creates it if it doesn't). Requires sufficient privileges.

    var upgrader = DeployChanges.To
        .PostgresqlDatabase(connectionString)
        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
        .LogToNowhere() // we'll use custom console below
        .Build();

    var result = upgrader.PerformUpgrade();

    if (!result.Successful)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(result.Error);
        Console.ResetColor();
        return -1;
    }

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Success!");
    Console.ResetColor();
    return 0;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Migration failed: {ex}");
    return -2;
}

static string? FindEnvUpwards()
{
    try
    {
        var dir = AppContext.BaseDirectory;
        for (int i = 0; i < 6; i++)
        {
            var candidate = Path.Combine(dir, ".env");
            if (File.Exists(candidate)) return candidate;
            var parent = Directory.GetParent(dir);
            if (parent == null) break;
            dir = parent.FullName;
        }
    }
    catch { /* ignore */ }
    return null;
}