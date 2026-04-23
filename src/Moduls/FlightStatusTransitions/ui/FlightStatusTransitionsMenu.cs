using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Application.Interfaces;
using DomainTransition = GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.Aggregate.FlightStatusTransition;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.ui;

public sealed class FlightStatusTransitionsMenu : IModuleUI
{
    private readonly IFlightStatusTransitionsService _service;

    public FlightStatusTransitionsMenu(IFlightStatusTransitionsService service)
    {
        _service = service;
    }

    public string Key => "flight-status-transitions";
    public string Title => "Transiciones de estado de vuelo";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Transiciones de estado de vuelo [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Listar todas",
                        "Buscar por ID",
                        "Buscar por from_status_id",
                        "Validar transicion",
                        "Crear transicion",
                        "Actualizar transicion",
                        "Eliminar por ID",
                        "Volver"));

            switch (option)
            {
                case "Listar todas":
                    await ListAsync(cancellationToken);
                    break;
                case "Buscar por ID":
                    await SearchByIdAsync(cancellationToken);
                    break;
                case "Buscar por from_status_id":
                    await SearchByFromStatusIdAsync(cancellationToken);
                    break;
                case "Validar transicion":
                    await ValidateTransitionAsync(cancellationToken);
                    break;
                case "Crear transicion":
                    await CreateAsync(cancellationToken);
                    break;
                case "Actualizar transicion":
                    await UpdateAsync(cancellationToken);
                    break;
                case "Eliminar por ID":
                    await DeleteByIdAsync(cancellationToken);
                    break;
                case "Volver":
                    return;
            }
        }
    }

    private async Task ListAsync(CancellationToken cancellationToken)
    {
        RenderTable(await _service.GetAllAsync(cancellationToken), "Transiciones de estado");
        Pause();
    }

    private async Task SearchByIdAsync(CancellationToken cancellationToken)
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value, cancellationToken);
        RenderTable(item is null ? Array.Empty<DomainTransition>() : new[] { item }, $"ID {id.Value}");
        Pause();
    }

    private async Task SearchByFromStatusIdAsync(CancellationToken cancellationToken)
    {
        var fromStatusId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID del estado origen:");
        if (fromStatusId is null)
            return;

        RenderTable(await _service.GetByFromStatusIdAsync(fromStatusId.Value, cancellationToken), $"Transiciones desde estado {fromStatusId.Value}");
        Pause();
    }

    private async Task ValidateTransitionAsync(CancellationToken cancellationToken)
    {
        var fromStatusId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID del estado origen:");
        if (fromStatusId is null)
            return;

        var toStatusId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID del estado destino:");
        if (toStatusId is null)
            return;

        var isValid = await _service.ValidateTransitionAsync(fromStatusId.Value, toStatusId.Value, cancellationToken);
        if (isValid)
            AnsiConsole.MarkupLine("\n[green]La transicion es valida.[/]");
        else
            AnsiConsole.MarkupLine("\n[yellow]La transicion no esta permitida.[/]");

        Pause();
    }

    private async Task CreateAsync(CancellationToken cancellationToken)
    {
        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Registrar transición de estado de vuelo"))
            return;

        var fromStatusId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID del estado origen:");
        if (fromStatusId is null)
            return;

        var toStatusId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID del estado destino:");
        if (toStatusId is null)
            return;

        try
        {
            await _service.CreateAsync(fromStatusId.Value, toStatusId.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Transicion creada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task UpdateAsync(CancellationToken cancellationToken)
    {
        var current = await PromptTransitionSelectionAsync("Seleccione la transición a actualizar:", cancellationToken);
        if (current is null)
            return;

        var fromStatusId = ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("Nuevo ID del estado origen:", current.FromStatusId.Value);
        if (fromStatusId is null)
            return;

        var toStatusId = ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("Nuevo ID del estado destino:", current.ToStatusId.Value);
        if (toStatusId is null)
            return;

        try
        {
            await _service.UpdateAsync(current.Id!.Value, fromStatusId.Value, toStatusId.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Transicion actualizada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteByIdAsync(CancellationToken cancellationToken)
    {
        var current = await PromptTransitionSelectionAsync("Seleccione la transición a eliminar:", cancellationToken);
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
            AnsiConsole.MarkupLine("\n[green]Transicion eliminada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private static void RenderTable(IEnumerable<DomainTransition> items, string title)
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
            .AddColumn("[bold grey]from_status_id[/]")
            .AddColumn("[bold grey]to_status_id[/]");

        foreach (var item in list)
        {
            table.AddRow(
                item.Id?.Value.ToString() ?? "-",
                item.FromStatusId.Value.ToString(),
                item.ToStatusId.Value.ToString());
        }

        AnsiConsole.Write(table);
    }

    private async Task<DomainTransition?> PromptTransitionSelectionAsync(string title, CancellationToken cancellationToken)
    {
        var transitions = (await _service.GetAllAsync(cancellationToken))
            .Where(t => t.Id != null)
            .OrderBy(t => t.FromStatusId.Value)
            .ThenBy(t => t.ToStatusId.Value)
            .ToList();
        if (transitions.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay transiciones registradas.[/]");
            Pause();
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(transitions.Select(FormatTransitionChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return transitions.First(t => FormatTransitionChoice(t) == selected);
    }

    private static string FormatTransitionChoice(DomainTransition transition) =>
        $"{transition.Id?.Value ?? 0} · {transition.FromStatusId.Value} -> {transition.ToStatusId.Value}";

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Presiona Enter para continuar...[/]").AllowEmpty());
    }
}
