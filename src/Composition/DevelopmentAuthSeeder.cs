using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GestorDeVuelosProyectoFinal.Composition;

/// <summary>
/// Crea/actualiza usuarios demo en AppDbContext (tabla users),
/// alineados con las credenciales configuradas en appsettings.json.
/// </summary>
internal sealed class DevelopmentAuthSeeder
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;

    public DevelopmentAuthSeeder(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        // Estas credenciales salen de configuración para que el proyecto se pueda mover
        // entre máquinas sin tocar el código fuente.
        var adminUser = _configuration["Auth:DefaultAdmin:Username"] ?? "admin";
        var adminPass = _configuration["Auth:DefaultAdmin:Password"] ?? "Admin123!";
        var demoUser = _configuration["Auth:DefaultUser:Username"] ?? "usuario";
        var demoPass = _configuration["Auth:DefaultUser:Password"] ?? "User123!";

        var adminRoleId = await EnsureRoleAsync("Admin", cancellationToken);
        var userRoleId = await EnsureRoleAsync("User", cancellationToken);
        var clientRoleId = await EnsureRoleAsync("Client", cancellationToken);

        await EnsureUserAsync(adminUser, adminPass, adminRoleId, cancellationToken);

        // El usuario demo lo dejamos como Client para que entre directo al portal.
        await EnsureUserAsync(demoUser, demoPass, clientRoleId, cancellationToken);
    }

    private async Task<int> EnsureRoleAsync(string name, CancellationToken cancellationToken)
    {
        var role = await _db.SystemRoles.FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
        if (role is not null)
            return role.Id;

        // Creamos el rol si todavía no existe en la base.
        role = new SystemRolesEntity { Name = name, Description = $"Rol '{name}'" };
        _db.SystemRoles.Add(role);
        await _db.SaveChangesAsync(cancellationToken);
        return role.Id;
    }

    private async Task EnsureUserAsync(string username, string password, int roleId, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

        if (user is null)
        {
            // Si el usuario no existe, lo registramos desde cero.
            _db.Users.Add(new UsersEntity
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Person_Id = null,
                Role_Id = roleId,
                IsActive = true,
                LastAccess = null,
                CreatedAt = now,
                UpdatedAt = now
            });

            await _db.SaveChangesAsync(cancellationToken);
            return;
        }

        // Mantener datos existentes pero alinear rol/estado/clave demo si hace falta.
        user.Role_Id = roleId;
        user.IsActive = true;

        // Si la contraseña configurada cambió, re-hasheamos.
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

        user.UpdatedAt = now;
        await _db.SaveChangesAsync(cancellationToken);
    }
}
