using GestorDeVuelosProyectoFinal.Auth.Infrastructure;
using GestorDeVuelosProyectoFinal.Auth.ui;
using GestorDeVuelosProyectoFinal.Composition;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Este archivo arranca toda la aplicación:
// prepara configuración, dependencias, migraciones y datos demo antes de mostrar el menú.
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

static string? TryGetMySqlDatabaseName(string connectionString)
{
    try
    {
        // Intentamos sacar el nombre de la base directo de la cadena de conexión
        // para mostrar un mensaje más claro en consola al arrancar.
        var parts = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var p in parts)
        {
            var idx = p.IndexOf('=');
            if (idx <= 0) continue;
            var key = p[..idx].Trim();
            var val = p[(idx + 1)..].Trim();
            if (key.Equals("database", StringComparison.OrdinalIgnoreCase)
                || key.Equals("initial catalog", StringComparison.OrdinalIgnoreCase))
                return val;
        }
    }
    catch
    {
        // ignore
    }

    return null;
}

var services = new ServiceCollection();
services.AddGestorSqlModules(configuration);

using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();

var appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();

Console.WriteLine("[Arranque] Aplicando migraciones de la base principal...");
await appDb.Database.MigrateAsync();

// El contexto de autenticación se migra aparte porque maneja sus propias tablas.
Console.WriteLine("[Arranque] Aplicando migraciones de autenticacion...");
await using var authDb = new AuthDbContextDesignTimeFactory().CreateDbContext(Array.Empty<string>());
await authDb.Database.MigrateAsync();

var resolvedCs = MySqlConnectionOptions.ResolveConnectionString(configuration);
var dbName = TryGetMySqlDatabaseName(resolvedCs);
Console.WriteLine(dbName is null
    ? "[Arranque] MySQL conectado (no se pudo inferir el nombre de la base desde la cadena)."
    : $"[Arranque] MySQL · base detectada: {dbName}");

await AuthCredentialRepair.EnsureDemoCredentialsAsync(appDb, configuration);

await DevelopmentDataInitializer.EnsureDevelopmentDataAsync(scope.ServiceProvider);

Console.WriteLine("[Arranque] Migraciones y datos iniciales listos. Abriendo menu...");
ConsoleWelcome.ShowSplash();

var entry = scope.ServiceProvider.GetRequiredService<AuthEntryMenu>();
await entry.RunAsync();

