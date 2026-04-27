using Spectre.Console;
using BCrypt.Net;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Session;

namespace GestorDeVuelosProyectoFinal.src.Shared.UI;

public class LoginConsoleUI
{
    private readonly IUsersService _usersService;
    private readonly ISessionsService _sessionsService;
    private readonly ISystemRolesService _rolesService;

    public LoginConsoleUI(
        IUsersService usersService,
        ISessionsService sessionsService,
        ISystemRolesService rolesService)
    {
        _usersService = usersService;
        _sessionsService = sessionsService;
        _rolesService = rolesService;
    }

    public async Task<bool> ShowAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("Gestor de Vuelos").Color(Color.Blue));
        AnsiConsole.Write(new FigletText("Acceso").Color(Color.Cyan1));

        var attempts = 0;
        const int maxAttempts = 3;

        while (attempts < maxAttempts)
        {
            AnsiConsole.WriteLine();

            var continuar = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[grey]¿Qué deseas hacer?[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices("Iniciar sesión", "Volver al menú anterior"));

            if (continuar == "Volver al menú anterior")
                return false;

            var username = AnsiConsole.Ask<string>("[yellow]Usuario (0 = volver):[/]").Trim();

            if (username == "0")
                return false;

            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("[yellow]Contraseña:[/]")
                    .Secret());

            var success = false;

            await AnsiConsole.Status()
                .StartAsync("Autenticando...", async ctx =>
                {
                    try
                    {
                        // 1. Buscar el usuario por username.
                        var user = await _usersService.GetByUsernameAsync(username);

                        if (user is null)
                        {
                            AnsiConsole.MarkupLine("[red] Usuario o contraseña incorrectos.[/]");
                            return;
                        }

                        // 2. Validar que la cuenta siga activa.
                        if (!user.IsActive.Value)
                        {
                            AnsiConsole.MarkupLine("[red] Tu cuenta está desactivada. Contacta al administrador.[/]");
                            return;
                        }

                        // 3. Comparar la contraseña usando BCrypt.
                        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash.Value))
                        {
                            AnsiConsole.MarkupLine("[red] Usuario o contraseña incorrectos.[/]");
                            return;
                        }

                        // 4. Resolver el nombre del rol para mostrarlo y guardarlo en sesión.
                        var role = await _rolesService.GetByIdAsync(user.RolId.Value);
                        var roleName = role?.Name.Value ?? "Desconocido";

                        // 5. Registrar la sesión en la tabla de sesiones.
                        var ipAddress = GetLocalIpAddress();
                        await _sessionsService.CreateAsync(0, user.Id.Value, ipAddress);

                        // 6. Actualizar el último acceso del usuario.
                        user.UpdateLastAccess();
                        await _usersService.UpdateAsync(user.Id.Value, null, null);

                        // 7. Dejar la sesión cargada en memoria para el resto de menús.
                        UserSession.Login(user.Id.Value, user.RolId.Value, roleName);

                        AnsiConsole.MarkupLine($"[green] Bienvenido '[bold]{user.Username.Value}[/]' - Rol: [bold]{roleName}[/][/]");

                        success = true;
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
                    }
                });

            if (success)
                return true;

            attempts++;

            if (attempts < maxAttempts)
                AnsiConsole.MarkupLine($"[yellow] Intento {attempts}/{maxAttempts}. Intenta de nuevo.[/]");
        }

        AnsiConsole.MarkupLine("[red]❌ Demasiados intentos fallidos. Saliendo del sistema...[/]");
        return false;
    }

    public async Task LogoutAsync()
    {
        if (UserSession.Current is null) return;

        await AnsiConsole.Status()
            .StartAsync("Cerrando sesión...", async ctx =>
            {
                try
                {
                    var sessions = await _sessionsService
                        .GetActiveSessionsByUserIdAsync(UserSession.Current.UserId);

                    foreach (var session in sessions)
                        await _sessionsService.CloseSessionAsync(session.Id.Value);

                    UserSession.Logout();

                    AnsiConsole.MarkupLine("[green] Sesión cerrada correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red] Error al cerrar sesión: {ex.Message}[/]");
                }
            });
    }

    private static string? GetLocalIpAddress()
    {
        try
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            var ip = host.AddressList
                .FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            return ip?.ToString();
        }
        catch
        {
            return null;
        }
    }
}
