using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GestorDeVuelosProyectoFinal.Composition;

/// <summary>
/// Garantiza credenciales demo coherentes con appsettings.json en la tabla users (AppDbContext).
/// Esto evita el clásico desfase: credenciales "correctas" pero hash distinto / DB distinta.
/// </summary>
internal static class AuthCredentialRepair
{
    public static async Task EnsureDemoCredentialsAsync(
        AppDbContext db,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        // Leemos las credenciales demo desde configuración para poder alinearlas
        // con lo que la persona ve en appsettings.
        var adminUser = configuration["Auth:DefaultAdmin:Username"] ?? "admin";
        var adminPass = configuration["Auth:DefaultAdmin:Password"] ?? "Admin123!";
        var demoUser = configuration["Auth:DefaultUser:Username"] ?? "usuario";
        var demoPass = configuration["Auth:DefaultUser:Password"] ?? "User123!";

        var adminRoleId = await EnsureRoleAsync(db, "Admin", cancellationToken);
        var clientRoleId = await EnsureRoleAsync(db, "Client", cancellationToken);

        await EnsureUserAsync(db, adminUser, adminPass, adminRoleId, cancellationToken);
        await EnsureUserAsync(db, demoUser, demoPass, clientRoleId, cancellationToken);
    }

    private static async Task<int> EnsureRoleAsync(AppDbContext db, string name, CancellationToken cancellationToken)
    {
        var role = await db.SystemRoles.FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
        if (role is not null)
            return role.Id;

        // Si el rol no existe todavía, lo creamos para no depender de una siembra previa.
        role = new SystemRolesEntity { Name = name, Description = $"Rol '{name}'" };
        db.SystemRoles.Add(role);
        await db.SaveChangesAsync(cancellationToken);
        return role.Id;
    }

    private static async Task EnsureUserAsync(
        AppDbContext db,
        string username,
        string password,
        int roleId,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var u = username.Trim();

        var user = await db.Users.FirstOrDefaultAsync(x => x.Username == u, cancellationToken);
        if (user is null)
        {
            // Cuando no existe, lo creamos con la contraseña hasheada y el rol esperado.
            db.Users.Add(new UsersEntity
            {
                Username = u,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Person_Id = null,
                Role_Id = roleId,
                IsActive = true,
                LastAccess = null,
                CreatedAt = now,
                UpdatedAt = now
            });

            await db.SaveChangesAsync(cancellationToken);
            return;
        }

        user.Role_Id = roleId;
        user.IsActive = true;

        // Si el hash quedó viejo o la contraseña configurada cambió, lo regeneramos.
        var needsRehash = string.IsNullOrWhiteSpace(user.PasswordHash)
                          || !user.PasswordHash.StartsWith("$2", StringComparison.Ordinal)
                          || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

        if (needsRehash)
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

        user.UpdatedAt = now;
        await db.SaveChangesAsync(cancellationToken);
    }
}
