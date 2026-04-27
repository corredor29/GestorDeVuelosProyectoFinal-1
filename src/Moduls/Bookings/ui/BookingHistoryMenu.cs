using GestorDeVuelosProyectoFinal.src.Moduls.ReschedulingHistory.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.WaitingList.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.UI;

public sealed class BookingHistoryMenu
{
    private readonly IReschedulingHistoryRepository _historyRepository;
    private readonly IWaitingListRepository _waitingListRepository;
    private readonly IFlightsRepository _flightsRepository;

    public BookingHistoryMenu(
        IReschedulingHistoryRepository historyRepository,
        IWaitingListRepository waitingListRepository,
        IFlightsRepository flightsRepository)
    {
        _historyRepository     = historyRepository;
        _waitingListRepository = waitingListRepository;
        _flightsRepository     = flightsRepository;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold yellow]Historial y lista de espera[/]").LeftJustified());
            AnsiConsole.WriteLine();

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold white]¿Qué deseas consultar?[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Historial de reprogramaciones",
                        "Lista de espera",
                        ConsoleMenuHelpers.VolverAlMenu));

            switch (option)
            {
                case "Historial de reprogramaciones":
                    await ShowReschedulingHistoryAsync(cancellationToken);
                    break;
                case "Lista de espera":
                    await ShowWaitingListAsync(cancellationToken);
                    break;
                default:
                    return;
            }
        }
    }

    private async Task ShowReschedulingHistoryAsync(CancellationToken cancellationToken)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Historial de reprogramaciones[/]").LeftJustified());
        AnsiConsole.WriteLine();

        var history = (await _historyRepository.GetAllAsync(cancellationToken)).ToList();

        if (history.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay registros de reprogramaciones.[/]");
            Pause();
            return;
        }

        // Cargar vuelos para mostrar códigos en lugar de IDs
        var allFlights  = (await _flightsRepository.GetAllAsync(cancellationToken)).ToList();
        var flightsMap  = allFlights.ToDictionary(f => f.Id!.Value, f => f.Code.Value);

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold]ID[/]")
            .AddColumn("[bold]Reserva[/]")
            .AddColumn("[bold]Vuelo anterior[/]")
            .AddColumn("[bold]Nuevo vuelo[/]")
            .AddColumn("[bold]Fecha cambio[/]")
            .AddColumn("[bold]Motivo[/]");

        foreach (var h in history)
        {
            var prevFlight = flightsMap.TryGetValue(h.PreviousFlightId, out var pc) ? pc : h.PreviousFlightId.ToString();
            var newFlight  = flightsMap.TryGetValue(h.NewFlightId,      out var nc) ? nc : h.NewFlightId.ToString();

            table.AddRow(
                h.Id?.ToString() ?? "-",
                h.BookingId.ToString(),
                prevFlight,
                newFlight,
                h.ChangedAt.ToString("yyyy-MM-dd HH:mm"),
                Markup.Escape(h.Reason));
        }

        AnsiConsole.Write(table);
        Pause();
    }

    private async Task ShowWaitingListAsync(CancellationToken cancellationToken)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Lista de espera[/]").LeftJustified());
        AnsiConsole.WriteLine();

        // Preguntar si filtrar por vuelo o ver todos
        var filter = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[grey]¿Cómo deseas ver la lista?[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices("Ver todos", "Filtrar por vuelo"));

        List<GestorDeVuelosProyectoFinal.src.Moduls.WaitingList.Domain.Aggregate.WaitingLis> entries;

        if (filter == "Filtrar por vuelo")
        {
            var flightIdRaw = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID del vuelo (0 = volver):");
            if (flightIdRaw is null) return;

            entries = (await _waitingListRepository.GetByFlightIdAsync(flightIdRaw.Value, cancellationToken)).ToList();
        }
        else
        {
            // Cargar todos agrupando por vuelo
            var allFlights = (await _flightsRepository.GetAllAsync(cancellationToken)).ToList();
            var allEntries = new List<GestorDeVuelosProyectoFinal.src.Moduls.WaitingList.Domain.Aggregate.WaitingLis>();

            foreach (var f in allFlights)
            {
                var flightEntries = await _waitingListRepository.GetByFlightIdAsync(f.Id!.Value, cancellationToken);
                allEntries.AddRange(flightEntries);
            }

            entries = allEntries;
        }

        if (entries.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay entradas en la lista de espera.[/]");
            Pause();
            return;
        }

        var flightsForTable = (await _flightsRepository.GetAllAsync(cancellationToken)).ToList();
        var flightsMap      = flightsForTable.ToDictionary(f => f.Id!.Value, f => f.Code.Value);

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold]ID[/]")
            .AddColumn("[bold]Reserva[/]")
            .AddColumn("[bold]Vuelo[/]")
            .AddColumn("[bold]Solicitado[/]")
            .AddColumn("[bold]Prioridad[/]")
            .AddColumn("[bold]Estado[/]");

        foreach (var e in entries.OrderByDescending(x => x.Priority).ThenBy(x => x.RequestedAt))
        {
            var flightCode = flightsMap.TryGetValue(e.FlightId, out var fc) ? fc : e.FlightId.ToString();

            var statusLabel = e.Status switch
            {
                "Waiting"  => "[yellow]Waiting[/]",
                "Promoted" => "[green]Promoted[/]",
                "Cancelled" => "[red]Cancelled[/]",
                _          => e.Status
            };

            table.AddRow(
                e.Id?.ToString() ?? "-",
                e.BookingId.ToString(),
                flightCode,
                e.RequestedAt.ToString("yyyy-MM-dd HH:mm"),
                e.Priority.ToString(),
                statusLabel);
        }

        AnsiConsole.Write(table);
        Pause();
    }

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Pulse Enter para continuar...[/]").AllowEmpty());
    }
}