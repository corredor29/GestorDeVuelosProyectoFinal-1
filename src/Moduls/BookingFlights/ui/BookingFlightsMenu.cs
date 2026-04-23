using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;
using DomainBookingFlight = GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.Aggregate.BookingFlight;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.ui;

public sealed class BookingFlightsMenu : IModuleUI
{
    private readonly IBookingFlightsService _service;

    public BookingFlightsMenu(IBookingFlightsService service)
    {
        _service = service;
    }

    public string Key => "booking-flights";
    public string Title => "Vuelos por reserva";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Vuelos por reserva [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Listar todos",
                        "Buscar por ID",
                        "Buscar por ID de reserva",
                        "Crear registro",
                        "Actualizar registro",
                        "Eliminar por ID",
                        "Volver"));

            switch (option)
            {
                case "Listar todos":
                    await ListAsync(cancellationToken);
                    break;
                case "Buscar por ID":
                    await SearchByIdAsync(cancellationToken);
                    break;
                case "Buscar por ID de reserva":
                    await SearchByBookingIdAsync(cancellationToken);
                    break;
                case "Crear registro":
                    await CreateAsync(cancellationToken);
                    break;
                case "Actualizar registro":
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
        RenderTable(await _service.GetAllAsync(cancellationToken), "Vuelos por reserva");
        Pause();
    }

    private async Task SearchByIdAsync(CancellationToken cancellationToken)
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value, cancellationToken);
        RenderTable(item is null ? Array.Empty<DomainBookingFlight>() : new[] { item }, $"ID {id.Value}");
        Pause();
    }

    private async Task SearchByBookingIdAsync(CancellationToken cancellationToken)
    {
        var bookingId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID de reserva:");
        if (bookingId is null)
            return;

        RenderTable(await _service.GetByBookingIdAsync(bookingId.Value, cancellationToken), $"Vuelos de la reserva {bookingId.Value}");
        Pause();
    }

    private async Task CreateAsync(CancellationToken cancellationToken)
    {
        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Registrar vuelo por reserva"))
            return;

        var bookingId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID de reserva:");
        if (bookingId is null)
            return;

        var flightId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID de vuelo:");
        if (flightId is null)
            return;

        var partialAmount = PromptAmountOrBack("Monto parcial:");
        if (partialAmount is null)
            return;

        try
        {
            await _service.CreateAsync(bookingId.Value, flightId.Value, partialAmount.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Vuelo por reserva creado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task UpdateAsync(CancellationToken cancellationToken)
    {
        var current = await PromptBookingFlightSelectionAsync("Seleccione el registro a actualizar:", cancellationToken);
        if (current is null)
            return;

        var bookingId = ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("Nuevo ID de reserva:", current.BookingId.Value);
        if (bookingId is null)
            return;

        var flightId = ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("Nuevo ID de vuelo:", current.FlightId.Value);
        if (flightId is null)
            return;

        var partialAmount = PromptAmountOrBack("Nuevo monto parcial:", current.PartialAmount.Value);
        if (partialAmount is null)
            return;

        try
        {
            await _service.UpdateAsync(current.Id!.Value, bookingId.Value, flightId.Value, partialAmount.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Vuelo por reserva actualizado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteByIdAsync(CancellationToken cancellationToken)
    {
        var current = await PromptBookingFlightSelectionAsync("Seleccione el registro a eliminar:", cancellationToken);
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
            AnsiConsole.MarkupLine("\n[green]Vuelo por reserva eliminado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private static void RenderTable(IEnumerable<DomainBookingFlight> items, string title)
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
            .AddColumn("[bold grey]Reserva ID[/]")
            .AddColumn("[bold grey]Vuelo ID[/]")
            .AddColumn("[bold grey]Monto parcial[/]");

        foreach (var item in list)
        {
            table.AddRow(
                item.Id?.Value.ToString() ?? "-",
                item.BookingId.Value.ToString(),
                item.FlightId.Value.ToString(),
                item.PartialAmount.Value.ToString("0.00"));
        }

        AnsiConsole.Write(table);
    }

    private static decimal? PromptAmountOrBack(string label, decimal? current = null)
    {
        while (true)
        {
            var value = ConsoleMenuHelpers.PromptStringWithInitialOrBack(label, current?.ToString("0.00") ?? "0.00", allowEmpty: false);
            if (value is null)
                return null;

            if (!decimal.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var parsed)
                && !decimal.TryParse(value, out parsed))
            {
                AnsiConsole.MarkupLine("[red]Monto inválido.[/]");
                continue;
            }

            if (parsed < 0)
            {
                AnsiConsole.MarkupLine("[red]No puede ser negativo.[/]");
                continue;
            }

            return parsed;
        }
    }

    private async Task<DomainBookingFlight?> PromptBookingFlightSelectionAsync(string title, CancellationToken cancellationToken)
    {
        var items = (await _service.GetAllAsync(cancellationToken))
            .OrderBy(x => x.BookingId.Value)
            .ThenBy(x => x.FlightId.Value)
            .ToList();
        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay vuelos por reserva registrados.[/]");
            Pause();
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(items.Select(FormatBookingFlightChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return items.First(x => FormatBookingFlightChoice(x) == selected);
    }

    private static string FormatBookingFlightChoice(DomainBookingFlight item) =>
        $"{item.Id?.Value ?? 0} · Reserva {item.BookingId.Value} · Vuelo {item.FlightId.Value}";

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Presiona Enter para continuar...[/]").AllowEmpty());
    }
}
