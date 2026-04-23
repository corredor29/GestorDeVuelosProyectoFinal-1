using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightRoles.ui;

public sealed class FlightRolesMenu : IModuleUI
{
    private readonly IFlightRolesService _service;

    public FlightRolesMenu(IFlightRolesService service)
    {
        _service = service;
    }

    public string Key => "flight-crew-roles";
    public string Title => "Roles de tripulación";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Roles de tripulación [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Listar todos",
                        "Buscar por ID",
                        "Buscar por nombre",
                        "Crear rol",
                        "Actualizar rol",
                        "Eliminar por ID",
                        "Eliminar por nombre",
                        "Volver"));

            switch (option)
            {
                case "Listar todos":
                    await ListAsync(cancellationToken);
                    break;
                case "Buscar por ID":
                    await SearchByIdAsync(cancellationToken);
                    break;
                case "Buscar por nombre":
                    await SearchByNameAsync(cancellationToken);
                    break;
                case "Crear rol":
                    await CreateAsync(cancellationToken);
                    break;
                case "Actualizar rol":
                    await UpdateAsync(cancellationToken);
                    break;
                case "Eliminar por ID":
                    await DeleteByIdAsync(cancellationToken);
                    break;
                case "Eliminar por nombre":
                    await DeleteByNameAsync(cancellationToken);
                    break;
                case "Volver":
                    return;
            }
        }
    }

    private async Task ListAsync(CancellationToken cancellationToken)
    {
        RenderTable(await _service.GetAllAsync(cancellationToken), "Roles de tripulación");
        Pause();
    }

    private async Task SearchByIdAsync(CancellationToken cancellationToken)
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value, cancellationToken);
        RenderTable(item is null ? Array.Empty<FlightRole>() : new[] { item }, $"ID {id.Value}");
        Pause();
    }

    private async Task SearchByNameAsync(CancellationToken cancellationToken)
    {
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre:");
        if (name is null)
            return;

        var item = await _service.GetByNameAsync(name, cancellationToken);
        RenderTable(item is null ? Array.Empty<FlightRole>() : new[] { item }, name);
        Pause();
    }

    private async Task CreateAsync(CancellationToken cancellationToken)
    {
        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Registrar rol de tripulación"))
            return;

        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre del rol:");
        if (name is null)
            return;

        try
        {
            await _service.CreateAsync(name, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Rol creado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task UpdateAsync(CancellationToken cancellationToken)
    {
        var current = await PromptFlightRoleSelectionAsync("Seleccione el rol a actualizar:", cancellationToken);
        if (current is null)
            return;

        var name = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nuevo nombre:", current.Name.Value);
        if (name is null)
            return;

        try
        {
            await _service.UpdateAsync(current.Id!.Value, name, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Rol actualizado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteByIdAsync(CancellationToken cancellationToken)
    {
        var current = await PromptFlightRoleSelectionAsync("Seleccione el rol a eliminar:", cancellationToken);
        if (current is null)
            return;

        if (!AnsiConsole.Confirm("¿Confirmas la eliminación?", false))
        {
            Pause();
            return;
        }

        try
        {
            await _service.DeleteByIdAsync(current.Id!.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Rol eliminado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteByNameAsync(CancellationToken cancellationToken)
    {
        var current = await PromptFlightRoleSelectionAsync("Seleccione el rol a eliminar:", cancellationToken);
        if (current is null)
            return;

        if (!AnsiConsole.Confirm("¿Confirmas la eliminación?", false))
        {
            Pause();
            return;
        }

        try
        {
            await _service.DeleteByNameAsync(current.Name.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Rol eliminado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private static void RenderTable(IEnumerable<FlightRole> items, string title)
    {
        var list = items.ToList();
        if (list.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay registros para mostrar.[/]");
            return;
        }

        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold grey]ID[/]")
            .AddColumn("[bold grey]Nombre[/]");

        foreach (var item in list)
        {
            table.AddRow(
                item.Id?.Value.ToString() ?? "-",
                item.Name.Value);
        }

        AnsiConsole.Write(table);
    }

    private async Task<FlightRole?> PromptFlightRoleSelectionAsync(string title, CancellationToken cancellationToken)
    {
        var roles = (await _service.GetAllAsync(cancellationToken))
            .Where(r => r.Id != null)
            .OrderBy(r => r.Name.Value)
            .ToList();
        if (roles.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay roles registrados.[/]");
            Pause();
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(roles.Select(FormatFlightRoleChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return roles.First(r => FormatFlightRoleChoice(r) == selected);
    }

    private static string FormatFlightRoleChoice(FlightRole role) =>
        $"{role.Id?.Value ?? 0} · {Markup.Escape(role.Name.Value)}";

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Presiona Enter para continuar...[/]").AllowEmpty());
    }
}
