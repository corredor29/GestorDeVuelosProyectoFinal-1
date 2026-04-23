using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using GestorDeVuelosProyectoFinal.src.Shared.Session;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Permissions.UI;

public class PermissionsConsoleUI
{
    private readonly IPermissionsService _service;

    public PermissionsConsoleUI(IPermissionsService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Permisos").Color(Color.Green));

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Selecciona una opción:[/]")
                    .AddChoices(GetMenuOptions()));

            switch (option)
            {
                case " Listar permisos":
                    await ListPermissionsAsync();
                    break;
                case " Buscar por ID":
                    await GetPermissionByIdAsync();
                    break;
                case "  Crear permiso":
                    await CreatePermissionAsync();
                    break;
                case "  Actualizar permiso":
                    await UpdatePermissionAsync();
                    break;
                case "  Eliminar permiso":
                    await DeletePermissionAsync();
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
            " Listar permisos",
            " Buscar por ID"
        };

    if (UserSession.Current?.IsAdmin == true)
        {
            options.Add(" Crear permiso");
            options.Add("  Actualizar permiso");
            options.Add("  Eliminar permiso");
        }

        options.Add(" Volver");
        return options;
    }

    private async Task ListPermissionsAsync()
    {
        var permissions = await _service.GetAllAsync();

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[green]ID[/]")
            .AddColumn("[green]Nombre[/]")
            .AddColumn("[green]Descripción[/]");

        foreach (var permission in permissions)
        {
            table.AddRow(
                permission.Id.Value.ToString(),
                permission.Name.Value,
                permission.Description.Value ?? "[grey]Sin descripción[/]"
            );
        }

        AnsiConsole.Write(table);
    }

    private async Task GetPermissionByIdAsync()
    {
        AnsiConsole.MarkupLine("[bold green] Buscar permiso por ID[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null) return;

        try
        {
            var permission = await _service.GetByIdAsync(id.Value);

            if (permission is null)
            {
                AnsiConsole.MarkupLine("[red] Permiso no encontrado.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[green]Campo[/]")
                .AddColumn("[green]Valor[/]");

            table.AddRow("ID", permission.Id.Value.ToString());
            table.AddRow("Nombre", permission.Name.Value);
            table.AddRow("Descripción", permission.Description.Value ?? "Sin descripción");

            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
        }
    }

    private async Task CreatePermissionAsync()
    {
        AnsiConsole.MarkupLine("[bold green] Crear nuevo permiso[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null) return;
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("[yellow]Nombre (ej: CREATE_FLIGHT):[/]");
        if (name is null) return;
        var description = ConsoleMenuHelpers.PromptStringWithInitialOrBack("[yellow]Descripción:[/]", string.Empty, allowEmpty: true);

        await AnsiConsole.Status()
            .StartAsync("Creando permiso...", async ctx =>
            {
                try
                {
                    var permission = await _service.CreateAsync(
                        id.Value,
                        name,
                        string.IsNullOrWhiteSpace(description) ? null : description
                    );
                    AnsiConsole.MarkupLine($"[green]Permiso '[bold]{permission.Name.Value}[/]' creado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
                }
            });
    }

    private async Task UpdatePermissionAsync()
    {
        AnsiConsole.MarkupLine("[bold yellow]  Actualizar permiso[/]");

        var existing = await PromptPermissionSelectionAsync("[yellow]Seleccione el permiso a actualizar:[/]");
        if (existing is null)
            return;

        AnsiConsole.MarkupLine($"[grey]Nombre actual: {existing.Name.Value}[/]");
        AnsiConsole.MarkupLine($"[grey]Descripción actual: {existing.Description.Value ?? "Sin descripción"}[/]");

        var newName = ConsoleMenuHelpers.PromptStringWithInitialOrBack("[yellow]Nuevo nombre:[/]", existing.Name.Value, allowEmpty: false);
        if (newName is null) return;
        var newDescription = ConsoleMenuHelpers.PromptStringWithInitialOrBack("[yellow]Nueva descripción:[/]", existing.Description.Value ?? string.Empty, allowEmpty: true);

        await AnsiConsole.Status()
            .StartAsync("Actualizando permiso...", async ctx =>
            {
                try
                {
                    var updated = await _service.UpdateAsync(
                        existing.Id.Value,
                        string.IsNullOrWhiteSpace(newName) ? null : newName,
                        string.IsNullOrWhiteSpace(newDescription) ? null : newDescription
                    );
                    AnsiConsole.MarkupLine($"[green]Permiso '[bold]{updated.Name.Value}[/]' actualizado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
                }
            });
    }

    private async Task DeletePermissionAsync()
    {
        AnsiConsole.MarkupLine("[bold red]  Eliminar permiso[/]");

        var existing = await PromptPermissionSelectionAsync("[yellow]Seleccione el permiso a eliminar:[/]");
        if (existing is null) return;

        var confirm = AnsiConsole.Confirm($"[red]¿Estás seguro de eliminar el permiso '{Markup.Escape(existing.Name.Value)}'?[/]");

        if (!confirm)
        {
            AnsiConsole.MarkupLine("[grey]Operación cancelada.[/]");
            return;
        }

        await AnsiConsole.Status()
            .StartAsync("Eliminando permiso...", async ctx =>
            {
                try
                {
                    var deleted = await _service.DeleteAsync(existing.Id.Value);

                    if (deleted)
                        AnsiConsole.MarkupLine("[green]Permiso eliminado correctamente.[/]");
                    else
                        AnsiConsole.MarkupLine("[red] Permiso no encontrado.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
                }
            });
    }

    private async Task<dynamic?> PromptPermissionSelectionAsync(string title)
    {
        var items = (await _service.GetAllAsync()).OrderBy(x => x.Name.Value).ToList();
        if (!items.Any())
        {
            AnsiConsole.MarkupLine("[grey]No hay permisos registrados.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(items.Select(FormatPermissionChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return items.First(x => FormatPermissionChoice(x) == selected);
    }

    private static string FormatPermissionChoice(dynamic item) =>
        $"{item.Id.Value} · {Markup.Escape(item.Name.Value)}";
}
