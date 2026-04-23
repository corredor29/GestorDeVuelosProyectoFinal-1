using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Session;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.UI;

public class UsersConsoleUI
{
    private readonly IUsersService _service;

    public UsersConsoleUI(IUsersService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Usuarios").Color(Color.Cyan1));

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Selecciona una opción:[/]")
                    .AddChoices(GetMenuOptions()));

            switch (option)
            {
                case " Listar todos los usuarios":
                    await ListAllAsync();
                    break;
                case " Listar usuarios activos":
                    await ListActiveAsync();
                    break;
                case " Listar usuarios inactivos":
                    await ListInactiveAsync();
                    break;
                case " Buscar por ID":
                    await GetByIdAsync();
                    break;
                case " Buscar por nombre de usuario":
                    await GetByUsernameAsync();
                    break;
                case " Crear usuario":
                    await CreateAsync();
                    break;
                case "  Actualizar usuario":
                    await UpdateAsync();
                    break;
                case " Activar/Desactivar usuario":
                    await ToggleActiveAsync();
                    break;
                case "  Eliminar usuario":
                    await DeleteAsync();
                    break;
                case " Volver":
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
            " Listar todos los usuarios",
            " Listar usuarios activos",
            " Listar usuarios inactivos",
            " Buscar por ID",
            " Buscar por nombre de usuario"
        };

        if (UserSession.Current?.IsAdmin == true)
        {
            options.Add(" Crear usuario");
            options.Add("  Actualizar usuario");
            options.Add(" Activar/Desactivar usuario");
            options.Add("  Eliminar usuario");
        }

        options.Add(" Volver");
        return options;
    }

    private async Task ListAllAsync()
    {
        var users = await _service.GetAllAsync();
        RenderTable(users);
    }

    private async Task ListActiveAsync()
    {
        var users = await _service.GetActiveUsersAsync();
        RenderTable(users);
    }

    private async Task ListInactiveAsync()
    {
        var users = await _service.GetInactiveUsersAsync();
        RenderTable(users);
    }

    private void RenderTable(IEnumerable<Domain.Aggregate.User> users)
    {
        var list = users.ToList();

        if (!list.Any())
        {
            AnsiConsole.MarkupLine("[grey]No hay usuarios para mostrar.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[cyan1]ID[/]")
            .AddColumn("[cyan1]Usuario[/]")
            .AddColumn("[cyan1]Rol ID[/]")
            .AddColumn("[cyan1]Persona ID[/]")
            .AddColumn("[cyan1]Activo[/]")
            .AddColumn("[cyan1]Último acceso[/]");

        foreach (var user in list)
        {
            table.AddRow(
                user.Id.Value.ToString(),
                user.Username.Value,
                user.RolId.Value.ToString(),
                user.PersonId?.Value.ToString() ?? "[grey]No aplica[/]",
                user.IsActive.Value ? "[green]SI[/]" : "[red]NO[/]",
                user.LastAccess?.ToString("yyyy-MM-dd HH:mm") ?? "[grey]Nunca[/]"
            );
        }

        AnsiConsole.Write(table);
    }

    private async Task GetByIdAsync()
    {
        AnsiConsole.MarkupLine("[bold cyan1] Buscar usuario por ID[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null)
            return;

        try
        {
            var user = await _service.GetByIdAsync(id.Value);

            if (user is null)
            {
                AnsiConsole.MarkupLine("[red] Usuario no encontrado.[/]");
                return;
            }

            RenderDetail(user);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
        }
    }

    private async Task GetByUsernameAsync()
    {
        AnsiConsole.MarkupLine("[bold cyan1] Buscar usuario por nombre de usuario[/]");

        var username = ConsoleMenuHelpers.PromptRequiredStringOrBack("[yellow]Usuario:[/]");
        if (username is null)
            return;

        try
        {
            var user = await _service.GetByUsernameAsync(username);

            if (user is null)
            {
                AnsiConsole.MarkupLine("[red] Usuario no encontrado.[/]");
                return;
            }

            RenderDetail(user);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
        }
    }

    private void RenderDetail(Domain.Aggregate.User user)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[cyan1]Campo[/]")
            .AddColumn("[cyan1]Valor[/]");

        table.AddRow("ID", user.Id.Value.ToString());
        table.AddRow("Usuario", user.Username.Value);
        table.AddRow("Rol ID", user.RolId.Value.ToString());
        table.AddRow("Persona ID", user.PersonId?.Value.ToString() ?? "No aplica");
        table.AddRow("Activo", user.IsActive.Value ? " Sí" : " No");
        table.AddRow("Último acceso", user.LastAccess?.ToString("yyyy-MM-dd HH:mm") ?? "Nunca");

        AnsiConsole.Write(table);
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold green] Crear nuevo usuario[/]");

        var username = ConsoleMenuHelpers.PromptRequiredStringOrBack("[yellow]Usuario:[/]");
        if (username is null)
            return;

        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("[yellow]Contraseña:[/]")
                .Secret());
        var roleId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID del rol:[/]");
        if (roleId is null)
            return;

        var personId = ConsoleMenuHelpers.PromptIntOrBack("[yellow]ID de persona (vacío = omitir):[/]");

        await AnsiConsole.Status()
            .StartAsync("Creando usuario...", async ctx =>
            {
                try
                {
                    var user = await _service.CreateAsync(0, username, password, roleId.Value, personId);
                    AnsiConsole.MarkupLine($"[green]Usuario '[bold]{user.Username.Value}[/]' creado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
                }
            });
    }

    private async Task UpdateAsync()
    {
        AnsiConsole.MarkupLine("[bold yellow]  Actualizar usuario[/]");

        var existing = await PromptUserSelectionAsync("[yellow]Seleccione el usuario a actualizar:[/]");
        if (existing is null)
            return;

        AnsiConsole.MarkupLine($"[grey]Usuario actual: {existing.Username.Value}[/]");
        AnsiConsole.MarkupLine($"[grey]Rol actual: {existing.RolId.Value}[/]");

        var newPassword = AnsiConsole.Prompt(
            new TextPrompt<string>("[yellow]Nueva contraseña (Enter para mantener):[/]")
                .AllowEmpty()
                .Secret());

        var newRoleId = ConsoleMenuHelpers.PromptIntOrBack("[yellow]Nuevo ID de rol (vacío = mantener):[/]");

        await AnsiConsole.Status()
            .StartAsync("Actualizando usuario...", async ctx =>
            {
                try
                {
                    var updated = await _service.UpdateAsync(
                        existing.Id.Value,
                        string.IsNullOrWhiteSpace(newPassword) ? null : newPassword,
                        newRoleId
                    );
                    AnsiConsole.MarkupLine($"[green]Usuario '[bold]{updated.Username.Value}[/]' actualizado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
                }
            });
    }

    private async Task ToggleActiveAsync()
    {
        AnsiConsole.MarkupLine("[bold yellow] Activar/Desactivar usuario[/]");

        var existing = await PromptUserSelectionAsync("[yellow]Seleccione el usuario:[/]");
        if (existing is null)
            return;

        var action = existing.IsActive.Value ? "desactivar" : "activar";
        var confirm = AnsiConsole.Confirm($"[yellow]¿Estás seguro de {action} al usuario '[bold]{existing.Username.Value}[/]'?[/]");

        if (!confirm)
        {
            AnsiConsole.MarkupLine("[grey]Operación cancelada.[/]");
            return;
        }

        await AnsiConsole.Status()
            .StartAsync($"{action.ToUpper()} usuario...", async ctx =>
            {
                try
                {
                    var updated = await _service.ToggleActiveAsync(existing.Id.Value);
                    var status = updated.IsActive.Value ? "activado" : "desactivado";
                    AnsiConsole.MarkupLine($"[green]Usuario '[bold]{updated.Username.Value}[/]' {status} correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
                }
            });
    }

    private async Task DeleteAsync()
    {
        AnsiConsole.MarkupLine("[bold red]  Eliminar usuario[/]");

        var existing = await PromptUserSelectionAsync("[yellow]Seleccione el usuario a eliminar:[/]");
        if (existing is null)
            return;

        var confirm = AnsiConsole.Confirm($"[red]¿Estás seguro de eliminar el usuario '{Markup.Escape(existing.Username.Value)}'?[/]");

        if (!confirm)
        {
            AnsiConsole.MarkupLine("[grey]Operación cancelada.[/]");
            return;
        }

        await AnsiConsole.Status()
            .StartAsync("Eliminando usuario...", async ctx =>
            {
                try
                {
                    var deleted = await _service.DeleteAsync(existing.Id.Value);

                    if (deleted)
                        AnsiConsole.MarkupLine("[green]Usuario eliminado correctamente.[/]");
                    else
                        AnsiConsole.MarkupLine("[red] Usuario no encontrado.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
                }
            });
    }

    private async Task<Domain.Aggregate.User?> PromptUserSelectionAsync(string title)
    {
        var users = (await _service.GetAllAsync()).OrderBy(x => x.Username.Value).ToList();
        if (!users.Any())
        {
            AnsiConsole.MarkupLine("[grey]No hay usuarios registrados.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(users.Select(FormatUserChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return users.First(x => FormatUserChoice(x) == selected);
    }

    private static string FormatUserChoice(Domain.Aggregate.User user) =>
        $"{user.Id.Value} · {Markup.Escape(user.Username.Value)}";
}
