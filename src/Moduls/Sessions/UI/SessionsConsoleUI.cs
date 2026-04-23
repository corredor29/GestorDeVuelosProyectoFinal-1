using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Session;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Sessions.UI;

public class SessionsConsoleUI
{
    private readonly ISessionsService _service;

    public SessionsConsoleUI(ISessionsService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Sesiones").Color(Color.Gold1));

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Selecciona una opción:[/]")
                    .AddChoices(GetMenuOptions()));

            switch (option)
            {
                case " Ver mis sesiones activas":
                    await ListMyActiveSessionsAsync();
                    break;
                case " Buscar sesión por ID":
                    await GetByIdAsync();
                    break;
                case " Cerrar sesión por ID":
                    await CloseSessionAsync();
                    break;
                case " Ver sesiones activas de usuario":
                    await ListActiveByUserAsync();
                    break;
                case "🔙 Volver":
                    return;
            }

            AnsiConsole.WriteLine();
            AnsiConsole.Markup("[grey]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }
    }

    private IEnumerable<string> GetMenuOptions()
    {
        var options = new List<string>
        {
            " Ver mis sesiones activas",
            " Buscar sesión por ID",
            " Cerrar sesión por ID"
        };

        if (UserSession.Current?.IsAdmin == true)
        {
            options.Add(" Ver sesiones activas de usuario");
        }

        options.Add("🔙 Volver");
        return options;
    }

    private async Task ListMyActiveSessionsAsync()
    {
        if (UserSession.Current is null)
        {
            AnsiConsole.MarkupLine("[red] No hay sesión activa.[/]");
            return;
        }

        try
        {
            var sessions = await _service.GetActiveSessionsByUserIdAsync(UserSession.Current.UserId);
            RenderTable(sessions);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
        }
    }

    private async Task ListActiveByUserAsync()
    {
        AnsiConsole.MarkupLine("[bold green] Ver sesiones activas de usuario[/]");

        var userId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID del usuario:[/]");
        if (userId is null) return;

        try
        {
            var sessions = await _service.GetActiveSessionsByUserIdAsync(userId.Value);
            RenderTable(sessions);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
        }
    }

    private void RenderTable(IEnumerable<Domain.Aggregate.Session> sessions)
    {
        var list = sessions.ToList();

        if (!list.Any())
        {
            AnsiConsole.MarkupLine("[grey]No hay sesiones activas.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[gold1]ID[/]")
            .AddColumn("[gold1]Usuario ID[/]")
            .AddColumn("[gold1]IP[/]")
            .AddColumn("[gold1]Iniciada[/]")
            .AddColumn("[gold1]Cerrada[/]")
            .AddColumn("[gold1]Activa[/]");

        foreach (var session in list)
        {
            table.AddRow(
                session.Id.Value.ToString(),
                session.UsuarioId.Value.ToString(),
                session.IpOrigen.Value ?? "[grey]No disponible[/]",
                session.IniciadaEn.ToString("yyyy-MM-dd HH:mm"),
                session.CerradaEn?.ToString("yyyy-MM-dd HH:mm") ?? "[grey]En curso[/]",
                session.Activa.IsActive ? "[green]SI[/]" : "[red]NO[/]"
            );
        }

        AnsiConsole.Write(table);
    }

    private async Task GetByIdAsync()
    {
        AnsiConsole.MarkupLine("[bold green] Buscar sesión por ID[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null) return;

        try
        {
            var session = await _service.GetByIdAsync(id.Value);

            if (session is null)
            {
                AnsiConsole.MarkupLine("[red] Sesión no encontrada.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[gold1]Campo[/]")
                .AddColumn("[gold1]Valor[/]");

            table.AddRow("ID", session.Id.Value.ToString());
            table.AddRow("Usuario ID", session.UsuarioId.Value.ToString());
            table.AddRow("IP", session.IpOrigen.Value ?? "No disponible");
            table.AddRow("Iniciada", session.IniciadaEn.ToString("yyyy-MM-dd HH:mm"));
            table.AddRow("Cerrada", session.CerradaEn?.ToString("yyyy-MM-dd HH:mm") ?? "En curso");
            table.AddRow("Activa", session.Activa.IsActive ? " Sí" : " No");

            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
        }
    }

    private async Task CloseSessionAsync()
    {
        AnsiConsole.MarkupLine("[bold red] Cerrar sesión[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID de la sesión a cerrar:[/]");
        if (id is null) return;

        var confirm = AnsiConsole.Confirm($"[red]¿Estás seguro de cerrar la sesión con ID {id.Value}?[/]");

        if (!confirm)
        {
            AnsiConsole.MarkupLine("[grey]Operación cancelada.[/]");
            return;
        }

        await AnsiConsole.Status()
            .StartAsync("Cerrando sesión...", async ctx =>
            {
                try
                {
                    var closed = await _service.CloseSessionAsync(id.Value);

                    if (closed)
                        AnsiConsole.MarkupLine("[green] Sesión cerrada correctamente.[/]");
                    else
                        AnsiConsole.MarkupLine("[red] Sesión no encontrada.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
                }
            });
    }
}
