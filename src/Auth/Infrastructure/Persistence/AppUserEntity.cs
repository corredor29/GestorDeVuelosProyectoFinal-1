namespace GestorDeVuelosProyectoFinal.Auth.Infrastructure.Persistence;

// Esta entidad representa una cuenta simple de acceso para la consola.
// Aquí no guardamos datos personales completos, solo lo necesario para login.
public sealed class AppUserEntity
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    /// <summary>Admin o User</summary>
    public string Role { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
