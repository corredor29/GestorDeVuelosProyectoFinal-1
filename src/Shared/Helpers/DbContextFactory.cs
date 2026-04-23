using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Shared.Helpers;

// Esta fábrica crea AppDbContext manualmente para escenarios donde todavía no estamos
// dentro del contenedor de dependencias.
public class DbContextFactory
{
    public static AppDbContext Create()
    {
        // Levantamos configuración local igual que en runtime para no depender solo
        // de variables de entorno.
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
        string? connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION")
                    ?? config.GetConnectionString("MySqlDB");

        if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("No se encontró una cadena de conexión válida.");
        // Validamos versión mínima del servidor antes de construir el contexto.
        var detectedVersion = MySqlVersionResolver.DetectVersion(connectionString);
        var minVersion = new Version(8, 0, 0);
        if (detectedVersion < minVersion)
                throw new NotSupportedException($"Versión de MySQL no soportada: {detectedVersion}. Requiere {minVersion} o superior.");

        var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(connectionString, new MySqlServerVersion(detectedVersion))
                .Options;
        // Devolvemos el contexto listo para usarse fuera de DI.
        return new AppDbContext(options); 
        
    }
}
