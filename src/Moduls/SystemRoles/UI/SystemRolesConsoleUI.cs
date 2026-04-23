using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using GestorDeVuelosProyectoFinal.src.Shared.Session;
namespace GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.UI;

public class SystemRolesConsoleUI
{
    private readonly ISystemRolesService _service;

    public SystemRolesConsoleUI(ISystemRolesService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Roles del Sistema").Color(Color.Blue));

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Selecciona una opción:[/]")
                    .AddChoices(GetMenuOptions()));

            switch (option)
            {
                case " Listar roles":
                    await ListRolesAsync();
                    break;
                case " Buscar por ID":
                    await GetRoleByIdAsync();
                    break;
                case " Crear rol":
                    await CreateRoleAsync();
                    break;
                case "  Actualizar rol":
                    await UpdateRoleAsync();
                    break;
                case "  Eliminar rol":
                    await DeleteRoleAsync();
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
            " Listar roles",
            " Buscar por ID"
        };

        if (UserSession.Current?.IsAdmin == true)
        {
            options.Add(" Crear rol");
            options.Add("  Actualizar rol");
            options.Add("  Eliminar rol");
        }

        options.Add("🔙 Volver");
        return options;
    }

    private async Task ListRolesAsync()
    {
        var roles = await _service.GetAllAsync();

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[blue]ID[/]")
            .AddColumn("[blue]Nombre[/]")
            .AddColumn("[blue]Descripción[/]");

        foreach (var role in roles)
        {
            table.AddRow(
                role.Id.Value.ToString(),
                role.Name.Value,
                role.Description.Value ?? "[grey]Sin descripción[/]"
            );
        }

        AnsiConsole.Write(table);
    }

    private async Task CreateRoleAsync()
    {
        AnsiConsole.MarkupLine("[bold green]Crear nuevo rol[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null) return;
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("[yellow]Nombre:[/]");
        if (name is null) return;
        var description = ConsoleMenuHelpers.PromptStringWithInitialOrBack("[yellow]Descripción:[/]", string.Empty, allowEmpty: true);

        await AnsiConsole.Status()
            .StartAsync("Creando rol...", async ctx =>
            {
                try
                {
                    var role = await _service.CreateAsync(
                        id.Value,
                        name,
                        string.IsNullOrWhiteSpace(description) ? null : description
                    );
                    AnsiConsole.MarkupLine($"[green]Rol '[bold]{role.Name.Value}[/]' creado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
                }
            });
    }

    private async Task GetRoleByIdAsync()
    {
        AnsiConsole.MarkupLine("[bold green]Buscar rol por ID[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null) return;

        try
        {
            var role = await _service.GetByIdAsync(id.Value);

            if (role is null)
            {
                AnsiConsole.MarkupLine("[red] Rol no encontrado.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[blue]Campo[/]")
                .AddColumn("[blue]Valor[/]");

            table.AddRow("ID", role.Id.Value.ToString());
            table.AddRow("Nombre", role.Name.Value);
            table.AddRow("Descripción", role.Description.Value ?? "Sin descripción");

            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
        }
    }

    private async Task UpdateRoleAsync()
    {
        AnsiConsole.MarkupLine("[bold yellow]  Actualizar rol[/]");

        var existing = await PromptRoleSelectionAsync("[yellow]Seleccione el rol a actualizar:[/]");
        if (existing is null)
            return;

        AnsiConsole.MarkupLine($"[grey]Nombre actual: {existing.Name.Value}[/]");
        AnsiConsole.MarkupLine($"[grey]Descripción actual: {existing.Description.Value ?? "Sin descripción"}[/]");

        var newName = ConsoleMenuHelpers.PromptStringWithInitialOrBack("[yellow]Nuevo nombre:[/]", existing.Name.Value, allowEmpty: false);
        if (newName is null) return;
        var newDescription = ConsoleMenuHelpers.PromptStringWithInitialOrBack("[yellow]Nueva descripción:[/]", existing.Description.Value ?? string.Empty, allowEmpty: true);

        await AnsiConsole.Status()
            .StartAsync("Actualizando rol...", async ctx =>
            {
                try
                {
                    var updated = await _service.UpdateAsync(
                        existing.Id.Value,
                        string.IsNullOrWhiteSpace(newName) ? null : newName,
                        string.IsNullOrWhiteSpace(newDescription) ? null : newDescription
                    );
                    AnsiConsole.MarkupLine($"[green]Rol '[bold]{updated.Name.Value}[/]' actualizado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
                }
            });
    }

    private async Task DeleteRoleAsync()
    {
        AnsiConsole.MarkupLine("[bold red]  Eliminar rol[/]");

        var existing = await PromptRoleSelectionAsync("[yellow]Seleccione el rol a eliminar:[/]");
        if (existing is null) return;

        var confirm = AnsiConsole.Confirm($"[red]¿Estás seguro de eliminar el rol '{Markup.Escape(existing.Name.Value)}'?[/]");

        if (!confirm)
        {
            AnsiConsole.MarkupLine("[grey]Operación cancelada.[/]");
            return;
        }

        await AnsiConsole.Status()
            .StartAsync("Eliminando rol...", async ctx =>
            {
                try
                {
                    var deleted = await _service.DeleteAsync(existing.Id.Value);

                    if (deleted)
                        AnsiConsole.MarkupLine("[green]Rol eliminado correctamente.[/]");
                    else
                        AnsiConsole.MarkupLine("[red] Rol no encontrado.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red] Error: {ex.Message}[/]");
                }
            });
    }

    private async Task<dynamic?> PromptRoleSelectionAsync(string title)
    {
        var items = (await _service.GetAllAsync()).OrderBy(x => x.Name.Value).ToList();
        if (!items.Any())
        {
            AnsiConsole.MarkupLine("[grey]No hay roles registrados.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(items.Select(FormatRoleChoice).Append("🔙 Volver").ToList()));

        if (selected == "🔙 Volver")
            return null;

        return items.First(x => FormatRoleChoice(x) == selected);
    }

    private static string FormatRoleChoice(dynamic item) =>
        $"{item.Id.Value} · {Markup.Escape(item.Name.Value)}";
}
