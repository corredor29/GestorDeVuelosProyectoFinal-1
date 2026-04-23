using GestorDeVuelosProyectoFinal.src.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GestorDeVuelosProyectoFinal.Auth.Infrastructure;

// Esta fábrica permite crear el contexto de auth cuando EF necesita trabajar
// en tiempo de diseño sin depender del arranque completo de la app.
public sealed class AuthDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();

        // Cargamos la configuración manualmente porque aquí todavía no existe DI.
        var config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION")
                               ?? config.GetConnectionString("MySqlDB");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("No se encontró una cadena de conexión válida.");

        // También validamos versión mínima aquí para que el comportamiento sea
        // consistente con el contexto principal.
        var detectedVersion = MySqlVersionResolver.DetectVersion(connectionString);
        var minVersion = new Version(8, 0, 0);

        if (detectedVersion < minVersion)
        {
            throw new NotSupportedException(
                $"Versión de MySQL no soportada: {detectedVersion}. Requiere {minVersion} o superior.");
        }

        var options = new DbContextOptionsBuilder<AuthDbContext>()
            .UseMySql(connectionString, new MySqlServerVersion(detectedVersion))
            .Options;

        return new AuthDbContext(options);
    }
}
