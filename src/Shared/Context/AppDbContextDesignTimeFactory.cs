using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using GestorDeVuelosProyectoFinal.src.Shared.Helpers;

namespace GestorDeVuelosProyectoFinal.src.Shared.Context;

// Esta fábrica existe para que EF Core pueda crear el contexto en tiempo de diseño,
// por ejemplo al generar migraciones desde consola.
public class AppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();

        // Repetimos la carga de configuración porque aquí no tenemos todavía
        // el contenedor de dependencias levantado.
        var config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION")
                               ?? config.GetConnectionString("MySqlDB");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("No se encontró una cadena de conexión válida.");
        }

        // Validamos versión mínima del servidor para evitar migraciones sobre una base no soportada.
        var detectedVersion = MySqlVersionResolver.DetectVersion(connectionString);
        var minVersion = new Version(8, 0, 0);

        if (detectedVersion < minVersion)
        {
            throw new NotSupportedException(
                $"Versión de MySQL no soportada: {detectedVersion}. Requiere {minVersion} o superior."
            );
        }

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, new MySqlServerVersion(detectedVersion))
            .Options;

        return new AppDbContext(options);
    }
}
