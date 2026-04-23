using GestorDeVuelosProyectoFinal.Auth.Infrastructure;
using GestorDeVuelosProyectoFinal.Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GestorDeVuelosProyectoFinal.Auth.Application;

// Este servicio maneja lo más básico del acceso:
// sembrar usuarios demo, validar login y registrar cuentas nuevas.
public sealed class AuthService : IAuthService
{
    private const string DefaultAdminUsername = "admin";
    private const string DefaultUserUsername = "usuario";
    private const string DefaultAdminPassword = "Admin123!";
    private const string DefaultUserPassword = "User123!";

    private readonly AuthDbContext _db;
    private readonly IConfiguration _configuration;

    public AuthService(AuthDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public async Task EnsureSeededAsync(CancellationToken cancellationToken = default)
    {
        // Si no hay datos configurados, usamos credenciales demo por defecto.
        var adminUser = _configuration["Auth:DefaultAdmin:Username"] ?? DefaultAdminUsername;
        var adminPass = _configuration["Auth:DefaultAdmin:Password"] ?? DefaultAdminPassword;
        var demoUser = _configuration["Auth:DefaultUser:Username"] ?? DefaultUserUsername;
        var demoPass = _configuration["Auth:DefaultUser:Password"] ?? DefaultUserPassword;

        await EnsureUserAsync(adminUser, adminPass, "Admin", cancellationToken);
        await EnsureUserAsync(demoUser, demoPass, "User", cancellationToken);
    }

    private async Task EnsureUserAsync(string username, string plainPassword, string role, CancellationToken cancellationToken)
    {
        var exists = await _db.AppUsers.AsNoTracking().AnyAsync(u => u.Username == username, cancellationToken);
        if (exists)
            return;

        // La contraseña siempre se guarda hasheada, no en texto plano.
        _db.AppUsers.Add(new AppUserEntity
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword),
            Role = role,
            CreatedAt = DateTime.UtcNow
        });
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> TryLoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var user = await _db.AppUsers.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        if (user is null)
            return false;

        // Si el hash no coincide devolvemos false y la UI decide qué mostrar.
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return false;

        ApplicationSession.UserId = user.Id;
        ApplicationSession.Username = user.Username;
        ApplicationSession.Role = user.Role;
        return true;
    }

    public async Task RegisterUserAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        if (await _db.AppUsers.AnyAsync(u => u.Username == username, cancellationToken))
            throw new InvalidOperationException($"El usuario '{username}' ya existe.");

        // Las cuentas creadas aquí salen como usuario normal.
        _db.AppUsers.Add(new AppUserEntity
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = "User",
            CreatedAt = DateTime.UtcNow
        });
        await _db.SaveChangesAsync(cancellationToken);
    }
}
