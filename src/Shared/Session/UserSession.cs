namespace GestorDeVuelosProyectoFinal.src.Shared.Session;

// Esta clase deja una sesión sencilla en memoria para la consola.
// No busca reemplazar un sistema de auth real, solo guardar el usuario activo actual.
public class UserSession
{
    public int UserId { get; private set; }
    public int RoleId { get; private set; }
    public string RoleName { get; private set; } = null!;
    public bool IsAdmin => RoleName == "Admin";

    private static UserSession? _current;
    public static UserSession? Current => _current;

    public static void Login(int userId, int roleId, string roleName)
    {
        // Reemplazamos la sesión actual por una nueva cuando el login fue exitoso.
        _current = new UserSession
        {
            UserId = userId,
            RoleId = roleId,
            RoleName = roleName
        };
    }

    public static void Logout() => _current = null;
}
