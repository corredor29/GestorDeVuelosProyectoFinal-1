using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.UI;

public sealed class RescheduleMenu
{
    private readonly IBookingsService _bookingsService;
    private readonly IBookingFlightsService _bookingFlightsService;
    private readonly IFlightsRepository _flightsRepository;
    private readonly IBookingStatusService _statusService; 

    public RescheduleMenu(
        IBookingsService bookingsService,
        IBookingFlightsService bookingFlightsService,
        IFlightsRepository flightsRepository,
        IBookingStatusService statusService) 
    {
        _bookingsService       = bookingsService;
        _bookingFlightsService = bookingFlightsService;
        _flightsRepository     = flightsRepository;
        _statusService         = statusService; 
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold yellow]Reprogramar reserva[/]").LeftJustified());
        AnsiConsole.WriteLine();

        var statuses = (await _statusService.GetAllStatuses())
            .ToDictionary(s => s.Id!.Value, s => s.Name.Value);

        // Mostrar tabla de todas las reservas
        var allBookings = (await _bookingsService.GetAllAsync(cancellationToken))
            .OrderBy(b => b.Id!.Value)
            .ToList();

        if (allBookings.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay reservas registradas.[/]");
            Pause();
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold]ID[/]")
            .AddColumn("[bold]Cliente[/]")
            .AddColumn("[bold]Estado[/]")
            .AddColumn("[bold]Reservado[/]")
            .AddColumn("[bold]Total[/]");

        foreach (var b in allBookings)
        {
            var statusName = statuses.TryGetValue(b.BookingStatusId.Value, out var name)
                ? name
                : b.BookingStatusId.Value.ToString();

            var statusLabel = statusName switch
            {
                "Confirmed"   => $"[green]{statusName}[/]",
                "Rescheduled" => $"[yellow]{statusName}[/]",
                "Cancelled"   => $"[red]{statusName}[/]",
                "Pending"     => $"[grey]{statusName}[/]",
                "OnHold"      => $"[blue]{statusName}[/]",
                _             => statusName
            };

            table.AddRow(
                b.Id!.Value.ToString(),
                b.ClientId.Value.ToString(),
                statusLabel,
                b.BookedAt.Value.ToString("yyyy-MM-dd HH:mm"),
                b.TotalAmount.Value.ToString("C"));
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();

        // 1. ID de la reserva
        var bookingIdRaw = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID de la reserva (0 = volver):");
        if (bookingIdRaw is null) return;
        var bookingId = bookingIdRaw.Value;

        // 2. Cargar y mostrar la reserva
        var booking = await _bookingsService.GetByIdAsync(bookingId, cancellationToken);
        if (booking is null)
        {
            AnsiConsole.MarkupLine($"\n[red]No existe la reserva con id {bookingId}.[/]");
            Pause();
            return;
        }

        var bookingStatusName = statuses.TryGetValue(booking.BookingStatusId.Value, out var bsName)
            ? bsName
            : booking.BookingStatusId.Value.ToString();

        AnsiConsole.Write(new Panel(
            $"ID: [bold]{booking.Id!.Value}[/]  |  " +
            $"Cliente: [bold]{booking.ClientId.Value}[/]  |  " +
            $"Estado: [bold]{bookingStatusName}[/]  |  " +
            $"Reservado: [bold]{booking.BookedAt.Value:yyyy-MM-dd HH:mm}[/]")
            .Header("[grey]Datos de la reserva[/]")
            .BorderColor(Color.Grey));

        AnsiConsole.WriteLine();

        // 3. Cargar vuelos asociados a la reserva
        var bookingFlights = (await _bookingFlightsService.GetByBookingIdAsync(bookingId, cancellationToken)).ToList();
        if (bookingFlights.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[red]Esta reserva no tiene vuelos asignados.[/]");
            Pause();
            return;
        }

        // 4. Cargar todos los vuelos
        var allFlights = (await _flightsRepository.GetAllAsync(cancellationToken)).ToList();
        var flightsMap = allFlights.ToDictionary(f => f.Id!.Value, f => f);

        var flightChoices = bookingFlights
            .Where(bf => flightsMap.ContainsKey(bf.FlightId.Value))
            .Select(bf =>
            {
                var f = flightsMap[bf.FlightId.Value];
                return $"ID {f.Id!.Value} · {f.Code.Value} · Salida: {f.DepartureAt.Value:yyyy-MM-dd HH:mm} · Asientos: {f.AvailableSeats.Value}";
            })
            .ToList();

        if (flightChoices.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[red]No se encontraron vuelos válidos para esta reserva.[/]");
            Pause();
            return;
        }

        var selectedCurrentLabel = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione el vuelo actual a reprogramar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices(flightChoices.Append(ConsoleMenuHelpers.VolverAlMenu)));

        if (selectedCurrentLabel == ConsoleMenuHelpers.VolverAlMenu) return;

        var currentFlightId = int.Parse(selectedCurrentLabel.Split('·')[0].Replace("ID", "").Trim());
        var currentFlight   = flightsMap[currentFlightId];

        // 5. Mostrar vuelos compatibles
        var compatibleFlights = allFlights
            .Where(f =>
                f.Id!.Value != currentFlightId &&
                f.RouteId.Value == currentFlight.RouteId.Value &&
                f.DepartureAt.Value > DateTime.UtcNow &&
                f.AvailableSeats.Value > 0)
            .OrderBy(f => f.DepartureAt.Value)
            .ToList();

        if (compatibleFlights.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay vuelos compatibles disponibles para reprogramar.[/]");
            AnsiConsole.MarkupLine("[grey]Se requiere misma ruta, fecha futura y asientos disponibles.[/]");
            Pause();
            return;
        }

        var newFlightChoices = compatibleFlights
            .Select(f => $"ID {f.Id!.Value} · {f.Code.Value} · Salida: {f.DepartureAt.Value:yyyy-MM-dd HH:mm} · Asientos: {f.AvailableSeats.Value}")
            .ToList();

        var selectedNewLabel = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione el nuevo vuelo:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(10)
                .AddChoices(newFlightChoices.Append(ConsoleMenuHelpers.VolverAlMenu)));

        if (selectedNewLabel == ConsoleMenuHelpers.VolverAlMenu) return;

        var newFlightId = int.Parse(selectedNewLabel.Split('·')[0].Replace("ID", "").Trim());

        // 6. Motivo
        var reason = AnsiConsole.Ask<string>("[deepskyblue1]Motivo del cambio:[/]").Trim();
        if (string.IsNullOrWhiteSpace(reason))
        {
            AnsiConsole.MarkupLine("\n[red]El motivo es obligatorio.[/]");
            Pause();
            return;
        }

        try
        {
            var hasSeat = await _bookingsService.RescheduleAsync(
                bookingId,
                currentFlightId,
                newFlightId,
                reason,
                cancellationToken);

            if (hasSeat)
            {
                AnsiConsole.MarkupLine("\n[green] Reserva reprogramada correctamente.[/]");
                Pause();

                // ✅ Volver a mostrar la tabla actualizada para confirmar el cambio
                await RunAsync(cancellationToken);
                return;
            }

            AnsiConsole.MarkupLine("\n[yellow]⚠ El vuelo no tiene asientos disponibles.[/]");

            var addToWaiting = AnsiConsole.Confirm("¿Desea agregar la reserva a la lista de espera?");
            if (!addToWaiting)
            {
                Pause();
                return;
            }

            var priority = AnsiConsole.Prompt(
                new TextPrompt<int>("[deepskyblue1]Prioridad (0 = normal, mayor = más urgente):[/]")
                    .DefaultValue(0)
                    .Validate(p => p >= 0
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]La prioridad no puede ser negativa.[/]")));

            await _bookingsService.AddToWaitingListAsync(
                bookingId,
                newFlightId,
                priority,
                cancellationToken);

            AnsiConsole.MarkupLine("\n[green] Reserva agregada a la lista de espera.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]Error: {Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Pulse Enter para continuar...[/]").AllowEmpty());
    }
}