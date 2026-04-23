using Spectre.Console;
using GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using CabinConfigurationAggregate = GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.Aggregate.CabinConfiguration;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CabinConfiguration.Infrastructure.ui;

public sealed class CabinConfigurationMenu : IModuleUI
{
    private readonly ICabinConfigurationService _service;

    public CabinConfigurationMenu(ICabinConfigurationService service)
    {
        _service = service;
    }

    public string Key => "cabin-configuration";
    public string Title => "Configuración de Cabina";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Configuración de Cabina [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Listar todas",
                        "Buscar por aeronave",
                        "Crear configuración",
                        "Actualizar configuración",
                        "Eliminar configuración",
                        "Volver"));

            switch (option)
            {
                case "Listar todas": await ListAllAsync(); break;
                case "Buscar por aeronave": await ListByAircraftAsync(); break;
                case "Crear configuración": await CreateAsync(); break;
                case "Actualizar configuración": await UpdateAsync(); break;
                case "Eliminar configuración": await DeleteAsync(); break;
                case "Volver": return;
            }
        }
    }

    private async Task ListAllAsync()
    {
        RenderTable(await _service.GetAllAsync(), "Configuraciones de cabina");
        Pause();
    }

    private async Task ListByAircraftAsync()
    {
        var aircraftId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID de la aeronave:");
        if (aircraftId is null)
            return;

        RenderTable(await _service.GetByAircraftIdAsync(aircraftId.Value), $"Configuraciones para aeronave {aircraftId.Value}");
        Pause();
    }

    private async Task CreateAsync()
    {
        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Registrar configuración de cabina"))
            return;

        var form = PromptForm();
        if (form is null)
            return;

        try
        {
            await _service.CreateAsync(form.Value.aircraftId, form.Value.cabinTypeId, form.Value.rowStart, form.Value.rowEnd, form.Value.seatsPerRow, form.Value.seatLetters);
            AnsiConsole.MarkupLine("\n[green]Configuración creada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{ex.Message}[/]");
        }
        Pause();
    }

    private async Task UpdateAsync()
    {
        var current = await PromptConfigurationSelectionAsync("Seleccione la configuración a actualizar:");
        if (current is null)
            return;

        var form = PromptForm(
            current.AircraftId,
            current.CabinTypeId.Value,
            current.RowRange.StartRow,
            current.RowRange.EndRow,
            current.SeatsPerRow.Value,
            current.SeatLetters.Value);
        if (form is null)
            return;

        try
        {
            await _service.UpdateAsync(current.Id.Value, form.Value.aircraftId, form.Value.cabinTypeId, form.Value.rowStart, form.Value.rowEnd, form.Value.seatsPerRow, form.Value.seatLetters);
            AnsiConsole.MarkupLine("\n[green]Configuración actualizada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{ex.Message}[/]");
        }
        Pause();
    }

    private async Task DeleteAsync()
    {
        var current = await PromptConfigurationSelectionAsync("Seleccione la configuración a eliminar:");
        if (current is null)
            return;

        if (!AnsiConsole.Confirm("¿Confirmas la eliminación?", false))
        {
            Pause();
            return;
        }

        try
        {
            await _service.DeleteAsync(current.Id.Value);
            AnsiConsole.MarkupLine("\n[green]Configuración eliminada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{ex.Message}[/]");
        }
        Pause();
    }

    private static void RenderTable(IEnumerable<CabinConfigurationAggregate> items, string title)
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
            .AddColumn("[bold grey]Aeronave[/]")
            .AddColumn("[bold grey]Tipo[/]")
            .AddColumn("[bold grey]Filas[/]")
            .AddColumn("[bold grey]Asientos/Fila[/]")
            .AddColumn("[bold grey]Letras[/]")
            .AddColumn("[bold grey]Asientos generados[/]");

        foreach (var item in list.OrderBy(x => x.AircraftId).ThenBy(x => x.CabinTypeId.Value))
        {
            table.AddRow(
                item.Id.Value.ToString(),
                item.AircraftId.ToString(),
                item.CabinTypeId.Value.ToString(),
                $"{item.RowRange.StartRow}-{item.RowRange.EndRow}",
                item.SeatsPerRow.Value.ToString(),
                item.SeatLetters.Value,
                item.GenerateSeatDesignators().Count().ToString());
        }

        AnsiConsole.Write(table);
    }

    private static (int aircraftId, int cabinTypeId, int rowStart, int rowEnd, int seatsPerRow, string seatLetters)? PromptForm(
        int? currentAircraftId = null,
        int? currentCabinTypeId = null,
        int? currentRowStart = null,
        int? currentRowEnd = null,
        int? currentSeatsPerRow = null,
        string? currentSeatLetters = null)
    {
        var aircraftId = currentAircraftId is int currentAircraft
            ? ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("ID de la aeronave:", currentAircraft)
            : ConsoleMenuHelpers.PromptPositiveIntOrBack("ID de la aeronave:");
        if (aircraftId is null)
            return null;

        var cabinTypeId = currentCabinTypeId is int currentCabin
            ? ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("ID del tipo de cabina:", currentCabin)
            : ConsoleMenuHelpers.PromptPositiveIntOrBack("ID del tipo de cabina:");
        if (cabinTypeId is null)
            return null;

        var rowStart = currentRowStart is int currentStart
            ? ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("Fila inicial:", currentStart)
            : ConsoleMenuHelpers.PromptPositiveIntOrBack("Fila inicial:");
        if (rowStart is null)
            return null;

        var rowEnd = currentRowEnd is int currentEnd
            ? ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("Fila final:", currentEnd)
            : ConsoleMenuHelpers.PromptPositiveIntOrBack("Fila final:");
        if (rowEnd is null)
            return null;

        var seatsPerRow = currentSeatsPerRow is int currentSeats
            ? ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("Asientos por fila:", currentSeats)
            : ConsoleMenuHelpers.PromptPositiveIntOrBack("Asientos por fila:");
        if (seatsPerRow is null)
            return null;

        var seatLetters = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Letras de asientos:", currentSeatLetters ?? string.Empty);
        if (seatLetters is null)
            return null;

        return (aircraftId.Value, cabinTypeId.Value, rowStart.Value, rowEnd.Value, seatsPerRow.Value, seatLetters.ToUpperInvariant());
    }

    private async Task<CabinConfigurationAggregate?> PromptConfigurationSelectionAsync(string title)
    {
        var configurations = (await _service.GetAllAsync())
            .OrderBy(c => c.AircraftId)
            .ThenBy(c => c.CabinTypeId.Value)
            .ToList();
        if (configurations.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay configuraciones registradas.[/]");
            Pause();
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(configurations.Select(FormatConfigurationChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return configurations.First(c => FormatConfigurationChoice(c) == selected);
    }

    private static string FormatConfigurationChoice(CabinConfigurationAggregate item) =>
        $"{item.Id.Value} · Aeronave {item.AircraftId} · Cabina {item.CabinTypeId.Value}";

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Presiona Enter para continuar...[/]").AllowEmpty());
    }
}
