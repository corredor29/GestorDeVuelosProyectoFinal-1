using System;
using MySqlConnector;


namespace GestorDeVuelosProyectoFinal.src.Shared.Helpers;

// Este helper abre una conexión corta solo para averiguar la versión real del servidor.
// Se usa antes de construir los DbContext para validar compatibilidad.
public class MySqlVersionResolver
{
    public static Version DetectVersion(string connectionString)
    {
        using var conn = new MySqlConnection(connectionString);
        conn.Open();
        var raw = conn.ServerVersion;
        // Algunos servidores devuelven sufijos tipo "-MySQL" o "-MariaDB",
        // así que nos quedamos solo con la parte numérica.
        var clean = raw.Split('-')[0];
        return Version.Parse(clean);
    }
}
