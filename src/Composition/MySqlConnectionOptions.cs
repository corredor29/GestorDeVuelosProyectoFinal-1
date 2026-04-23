using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GestorDeVuelosProyectoFinal.src.Shared.Helpers;

namespace GestorDeVuelosProyectoFinal.Composition;

// Este helper centraliza cómo resolvemos la conexión a MySQL para que todos los
// DbContext usen la misma lógica y no haya diferencias raras entre diseño y runtime.
internal static class MySqlConnectionOptions
{
    public static string ResolveConnectionString(IConfiguration configuration)
    {
        // Preferimos appsettings.json para evitar sorpresas por variables de entorno
        // residuales en la máquina del desarrollador.
        var cs = configuration.GetConnectionString("MySqlDB")
                 ?? Environment.GetEnvironmentVariable("MYSQL_CONNECTION");

        if (string.IsNullOrWhiteSpace(cs))
            throw new InvalidOperationException("No se encontró una cadena de conexión válida.");

        return cs;
    }

    public static void UseGestorMySql(DbContextOptionsBuilder options, IConfiguration configuration)
    {
        var connectionString = ResolveConnectionString(configuration);
        // Antes de abrir la conexión validamos la versión del servidor para evitar
        // errores más confusos después en tiempo de ejecución.
        var detectedVersion = MySqlVersionResolver.DetectVersion(connectionString);
        var minVersion = new Version(8, 0, 0);
        if (detectedVersion < minVersion)
            throw new NotSupportedException($"Versión de MySQL no soportada: {detectedVersion}. Requiere {minVersion} o superior.");

        options.UseMySql(connectionString, new MySqlServerVersion(detectedVersion));
    }
}
