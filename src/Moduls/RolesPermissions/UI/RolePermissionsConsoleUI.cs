using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Application.Interfaces;
using RolePermissionAggregate = GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.Aggregate.RolePermission;
using GestorDeVuelosProyectoFinal.src.Shared.Session;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.UI;

public class RolePermissionsConsoleUI
{
    private readonly IRolePermissionsService _service;

    public RolePermissionsConsoleUI(IRolePermissionsService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Permisos por Rol").Color(Color.Magenta1));

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Selecciona una opción:[/]")
                    .AddChoices(GetMenuOptions()));

            switch (option)
            {
                case " Listar permisos por rol":
                    await ListByRoleAsync();
                    break;
                case " Buscar por ID":
                    await GetByIdAsync();
                    break;
                case " Asignar permiso a rol":
                    await CreateAsync();
                    break;
                case "  Eliminar asignación":
                    await DeleteAsync();
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
            " Listar permisos por rol",
            " Buscar por ID"
        };

        if (UserSession.Current?.IsAdmin == true)
        {
            options.Add(" Asignar permiso a rol");
            options.Add("  Eliminar asignación");
        }

        options.Add("🔙 Volver");
        return options;
    }

    private async Task ListByRoleAsync()
    {
        AnsiConsole.MarkupLine("[bold green] Listar permisos por rol[/]");

        var roleId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID del rol:[/]");
        if (roleId is null)
            return;

        try
        {
            var permissions = await _service.GetByRoleIdAsync(roleId.Value);

            var list = permissions.ToList();

            if (!list.Any())
            {
                AnsiConsole.MarkupLine("[grey]No hay permisos asignados a este rol.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[magenta1]ID[/]")
                .AddColumn("[magenta1]Rol ID[/]")
                .AddColumn("[magenta1]Permiso ID[/]");

            foreach (var rp in list)
            {
                table.AddRow(
                    rp.Id.Value.ToString(),
                    rp.RoleId.Value.ToString(),
                    rp.PermissionId.Value.ToString()
                );
            }

            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
        }
    }

    private async Task GetByIdAsync()
    {
        AnsiConsole.MarkupLine("[bold green] Buscar asignación por ID[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null)
            return;

        try
        {
            var result = await _service.GetByIdAsync(id.Value);

            if (result is null)
            {
                AnsiConsole.MarkupLine("[red] Asignación no encontrada.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[magenta1]Campo[/]")
                .AddColumn("[magenta1]Valor[/]");

            table.AddRow("ID", result.Id.Value.ToString());
            table.AddRow("Rol ID", result.RoleId.Value.ToString());
            table.AddRow("Permiso ID", result.PermissionId.Value.ToString());

            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
        }
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold green] Asignar permiso a rol[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null)
            return;
        var roleId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID del rol:[/]");
        if (roleId is null)
            return;
        var permissionId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID del permiso:[/]");
        if (permissionId is null)
            return;

        await AnsiConsole.Status()
            .StartAsync("Asignando permiso...", async ctx =>
            {
                try
                {
                    var result = await _service.CreateAsync(id.Value, roleId.Value, permissionId.Value);
                    AnsiConsole.MarkupLine($"[green]Permiso '[bold]{result.PermissionId.Value}[/]' asignado al rol '[bold]{result.RoleId.Value}[/]' correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
                }
            });
    }

    private async Task DeleteAsync()
    {
        AnsiConsole.MarkupLine("[bold red]  Eliminar asignación[/]");

        var existing = await PromptRolePermissionSelectionAsync("[yellow]Seleccione la asignación a eliminar:[/]");
        if (existing is null)
            return;

        var confirm = AnsiConsole.Confirm($"[red]¿Estás seguro de eliminar la asignación {existing.Id.Value} (Rol {existing.RoleId.Value} / Permiso {existing.PermissionId.Value})?[/]");

        if (!confirm)
        {
            AnsiConsole.MarkupLine("[grey]Operación cancelada.[/]");
            return;
        }

        await AnsiConsole.Status()
            .StartAsync("Eliminando asignación...", async ctx =>
            {
                try
                {
                    var deleted = await _service.DeleteAsync(existing.Id.Value);

                    if (deleted)
                        AnsiConsole.MarkupLine("[green]Asignación eliminada correctamente.[/]");
                    else
                        AnsiConsole.MarkupLine("[red] Asignación no encontrada.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
                }
            });
    }

    private async Task<RolePermissionAggregate?> PromptRolePermissionSelectionAsync(string title)
    {
        var roleId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID del rol:[/]");
        if (roleId is null)
            return null;

        var items = (await _service.GetByRoleIdAsync(roleId.Value)).OrderBy(x => x.RoleId.Value).ThenBy(x => x.PermissionId.Value).ToList();
        if (!items.Any())
        {
            AnsiConsole.MarkupLine("[grey]No hay asignaciones registradas.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(items.Select(FormatRolePermissionChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return items.First(x => FormatRolePermissionChoice(x) == selected);
    }

    private static string FormatRolePermissionChoice(RolePermissionAggregate item) =>
        $"{item.Id.Value} · Rol {item.RoleId.Value} · Permiso {item.PermissionId.Value}";
}
