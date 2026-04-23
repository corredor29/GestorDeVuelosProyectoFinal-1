using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Application.Interfaces;
using DomainFlightStatus = GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.Aggregate.FlightStatus;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.UI;

public sealed class FlightStatusMenu : IModuleUI
{
    private readonly IFlightStatusService _service;

    public FlightStatusMenu(IFlightStatusService service)
    {
        _service = service;
    }

    public string Key => "flight-statuses";
    public string Title => "Estados de vuelo";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Estados de vuelo [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Listar todos",
                        "Buscar por ID",
                        "Buscar por nombre",
                        "Crear estado",
                        "Actualizar estado",
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
                case "Crear estado":
                    await CreateAsync(cancellationToken);
                    break;
                case "Actualizar estado":
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
        RenderTable(await _service.GetAllAsync(cancellationToken), "Estados de vuelo");
        Pause();
    }

    private async Task SearchByIdAsync(CancellationToken cancellationToken)
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value, cancellationToken);
        RenderTable(item is null ? Array.Empty<DomainFlightStatus>() : new[] { item }, $"ID {id.Value}");
        Pause();
    }

    private async Task SearchByNameAsync(CancellationToken cancellationToken)
    {
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre:");
        if (name is null)
            return;

        var item = await _service.GetByNameAsync(name, cancellationToken);
        RenderTable(item is null ? Array.Empty<DomainFlightStatus>() : new[] { item }, name);
        Pause();
    }

    private async Task CreateAsync(CancellationToken cancellationToken)
    {
        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Registrar estado de vuelo"))
            return;

        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre del estado:");
        if (name is null)
            return;

        try
        {
            await _service.CreateAsync(name, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Estado creado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task UpdateAsync(CancellationToken cancellationToken)
    {
        var current = await PromptFlightStatusSelectionAsync("Seleccione el estado a actualizar:", cancellationToken);
        if (current is null)
        {
            return;
        }

        var name = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nuevo nombre:", current.Name.Value);
        if (name is null)
            return;

        try
        {
            await _service.UpdateAsync(current.Id!.Value, name, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Estado actualizado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteByIdAsync(CancellationToken cancellationToken)
    {
        var current = await PromptFlightStatusSelectionAsync("Seleccione el estado a eliminar:", cancellationToken);
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
            AnsiConsole.MarkupLine("\n[green]Estado eliminado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteByNameAsync(CancellationToken cancellationToken)
    {
        var current = await PromptFlightStatusSelectionAsync("Seleccione el estado a eliminar:", cancellationToken);
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
            AnsiConsole.MarkupLine("\n[green]Estado eliminado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private static void RenderTable(IEnumerable<DomainFlightStatus> items, string title)
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
            .AddColumn("[bold grey]name[/]");

        foreach (var item in list)
        {
            table.AddRow(
                item.Id?.Value.ToString() ?? "-",
                item.Name.Value);
        }

        AnsiConsole.Write(table);
    }

    private async Task<DomainFlightStatus?> PromptFlightStatusSelectionAsync(string title, CancellationToken cancellationToken)
    {
        var statuses = (await _service.GetAllAsync(cancellationToken))
            .Where(s => s.Id != null)
            .OrderBy(s => s.Name.Value)
            .ToList();

        if (statuses.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay estados de vuelo registrados.[/]");
            Pause();
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(statuses.Select(FormatFlightStatusChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return statuses.First(s => FormatFlightStatusChoice(s) == selected);
    }

    private static string FormatFlightStatusChoice(DomainFlightStatus status) =>
        $"{status.Id?.Value ?? 0} · {Markup.Escape(status.Name.Value)}";

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Presiona Enter para continuar...[/]").AllowEmpty());
    }
}
