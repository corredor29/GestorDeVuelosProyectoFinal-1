using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Application.Interfaces;
using DomainFlights = GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Aggregate.Flights;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.UI;

public sealed class FlightsMenu : IModuleUI
{
    private readonly IFlightsRepository _flights;
    private readonly IFlightsOperationalService _operational;
    private readonly IFlightStatusService _statuses;
    private readonly IAirlinesService _airlines;
    private readonly IRoutesService _routes;
    private readonly IAirportsService _airports;
    private readonly IAircraftService _aircraft;

    public FlightsMenu(
        IFlightsRepository flights,
        IFlightsOperationalService operational,
        IFlightStatusService statuses,
        IAirlinesService airlines,
        IRoutesService routes,
        IAirportsService airports,
        IAircraftService aircraft)
    {
        _flights = flights;
        _operational = operational;
        _statuses = statuses;
        _airlines = airlines;
        _routes = routes;
        _airports = airports;
        _aircraft = aircraft;
    }

    public string Key => "flights";
    public string Title => "Vuelos";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Vuelos[/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Opciones")
                    .AddChoices(
                        "Listar vuelos",
                        "Registrar vuelo",
                        "Cambiar estado de vuelo",
                        "Volver"));

            switch (option)
            {
                case "Listar vuelos":
                    await ListAsync(cancellationToken);
                    break;
                case "Registrar vuelo":
                    await CreateAsync(cancellationToken);
                    break;
                case "Cambiar estado de vuelo":
                    await ChangeStatusAsync(cancellationToken);
                    break;
                case "Volver":
                    return;
            }
        }
    }

    private async Task CreateAsync(CancellationToken cancellationToken)
    {
        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Registrar vuelo"))
            return;

        var airlines = (await _airlines.GetAllAsync())
            .Where(a => a.Id is not null)
            .OrderBy(a => a.Id!.Value)
            .ToList();
        var routes = (await _routes.GetAllAsync())
            .Where(r => r.Id is not null)
            .OrderBy(r => r.Id!.Value)
            .ToList();
        var aircraft = (await _aircraft.GetAllAsync(cancellationToken))
            .Where(a => a.Id is not null)
            .OrderBy(a => a.Id!.Value)
            .ToList();
        var statuses = (await _statuses.GetAllAsync(cancellationToken))
            .Where(s => s.Id is not null)
            .OrderBy(s => s.Id!.Value)
            .ToList();

        if (airlines.Count == 0 || routes.Count == 0 || aircraft.Count == 0 || statuses.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay catálogos suficientes para registrar un vuelo.[/]");
            AnsiConsole.MarkupLine("[grey]Se requieren: aerolíneas, rutas, aeronaves y estados de vuelo.[/]");
            Pause();
            return;
        }

        var code = ConsoleMenuHelpers.PromptRequiredStringOrBack("Código del vuelo (ej. AV101):");
        if (code is null)
            return;

        var airportNames = (await _airports.GetAllAsync())
            .Where(a => a.Id is not null)
            .ToDictionary(a => a.Id!.Value, a => a.Name.Value);

        var airlineChoices = airlines.Select(a => $"{a.Id!.Value} · {Markup.Escape(a.Name.Value)}").ToList();
        airlineChoices.Add(ConsoleMenuHelpers.VolverSinGuardar);
        var pickAirline = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Seleccione la aerolínea").PageSize(15).AddChoices(airlineChoices));
        if (pickAirline == ConsoleMenuHelpers.VolverSinGuardar)
            return;
        var airlineIdPart = pickAirline.Split('·')[0].Trim();
        if (!int.TryParse(airlineIdPart, out var airlineId))
        {
            AnsiConsole.MarkupLine("[red]Selección inválida.[/]");
            Pause();
            return;
        }

        var routeChoices = routes.Select(r =>
        {
            var o = airportNames.TryGetValue(r.OriginAirportId.Value, out var on) ? on : r.OriginAirportId.Value.ToString();
            var d = airportNames.TryGetValue(r.DestinationAirportId.Value, out var dn) ? dn : r.DestinationAirportId.Value.ToString();
            return $"{r.Id!.Value} · {Markup.Escape(o)} → {Markup.Escape(d)}";
        }).ToList();
        routeChoices.Add(ConsoleMenuHelpers.VolverSinGuardar);
        var pickRoute = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Seleccione la ruta").PageSize(15).AddChoices(routeChoices));
        if (pickRoute == ConsoleMenuHelpers.VolverSinGuardar)
            return;
        var routeIdPart = pickRoute.Split('·')[0].Trim();
        if (!int.TryParse(routeIdPart, out var routeId))
        {
            AnsiConsole.MarkupLine("[red]Selección inválida.[/]");
            Pause();
            return;
        }

        var aircraftChoices = aircraft
            .Select(a => $"{a.Id!.Value} · {Markup.Escape(a.Registration.Value)}")
            .ToList();
        aircraftChoices.Add(ConsoleMenuHelpers.VolverSinGuardar);
        var pickAircraft = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Seleccione la aeronave").PageSize(15).AddChoices(aircraftChoices));
        if (pickAircraft == ConsoleMenuHelpers.VolverSinGuardar)
            return;
        var aircraftIdPart = pickAircraft.Split('·')[0].Trim();
        if (!int.TryParse(aircraftIdPart, out var aircraftId))
        {
            AnsiConsole.MarkupLine("[red]Selección inválida.[/]");
            Pause();
            return;
        }

        var dep = ConsoleMenuHelpers.PromptDateTimeOrBack("Salida (UTC) [dim](yyyy-MM-dd HH:mm)[/]:", "yyyy-MM-dd HH:mm");
        if (dep is null)
            return;
        var arr = ConsoleMenuHelpers.PromptDateTimeOrBack("Llegada estimada (UTC) [dim](yyyy-MM-dd HH:mm)[/]:", "yyyy-MM-dd HH:mm");
        if (arr is null)
            return;

        var totalCapacity = ConsoleMenuHelpers.PromptPositiveIntOrBack("Capacidad total:");
        if (totalCapacity is null)
            return;

        var statusChoices = statuses.Select(s => $"{s.Id!.Value} · {Markup.Escape(s.Name.Value)}").ToList();
        statusChoices.Add(ConsoleMenuHelpers.VolverSinGuardar);
        var pickStatus = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Seleccione el estado inicial").PageSize(15).AddChoices(statusChoices));
        if (pickStatus == ConsoleMenuHelpers.VolverSinGuardar)
            return;
        var statusIdPart = pickStatus.Split('·')[0].Trim();
        if (!int.TryParse(statusIdPart, out var statusId))
        {
            AnsiConsole.MarkupLine("[red]Selección inválida.[/]");
            Pause();
            return;
        }

        try
        {
            var flight = DomainFlights.Create(
                code.Trim(),
                airlineId,
                routeId,
                aircraftId,
                dep.Value,
                arr.Value,
                totalCapacity.Value,
                totalCapacity.Value,
                statusId,
                rescheduledAt: null);

            await _flights.SaveAsync(flight, cancellationToken);
            AnsiConsole.MarkupLine("[green]Vuelo registrado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task ListAsync(CancellationToken cancellationToken)
    {
        var list = (await _flights.GetAllAsync(cancellationToken)).ToList();
        if (list.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]No hay vuelos registrados.[/]");
            Pause();
            return;
        }

        var airlineNames = (await _airlines.GetAllAsync())
            .Where(a => a.Id != null)
            .ToDictionary(a => a.Id!.Value, a => a.Name.Value);

        var airportNames = (await _airports.GetAllAsync())
            .Where(a => a.Id != null)
            .ToDictionary(a => a.Id!.Value, a => a.Name.Value);

        var routeLabels = (await _routes.GetAllAsync())
            .Where(r => r.Id != null)
            .ToDictionary(
                r => r.Id!.Value,
                r =>
                {
                    var o = airportNames.TryGetValue(r.OriginAirportId.Value, out var on) ? on : r.OriginAirportId.Value.ToString();
                    var d = airportNames.TryGetValue(r.DestinationAirportId.Value, out var dn) ? dn : r.DestinationAirportId.Value.ToString();
                    return $"{r.Id!.Value} · {o} → {d}";
                });

        var statusNames = (await _statuses.GetAllAsync(cancellationToken))
            .Where(s => s.Id != null)
            .ToDictionary(s => s.Id!.Value, s => s.Name.Value);

        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("ID");
        table.AddColumn("Código");
        table.AddColumn("Aerolínea");
        table.AddColumn("Ruta");
        table.AddColumn("Salida (UTC)");
        table.AddColumn("Estado");
        table.AddColumn("Plazas");

        foreach (var f in list.OrderBy(x => x.Id?.Value ?? 0))
        {
            table.AddRow(
                f.Id?.Value.ToString() ?? "-",
                Markup.Escape(f.Code.Value),
                airlineNames.TryGetValue(f.AirlineId.Value, out var an)
                    ? $"{f.AirlineId.Value} · {Markup.Escape(an)}"
                    : f.AirlineId.Value.ToString(),
                routeLabels.TryGetValue(f.RouteId.Value, out var rl)
                    ? Markup.Escape(rl)
                    : f.RouteId.Value.ToString(),
                f.DepartureAt.Value.ToString("u"),
                statusNames.TryGetValue(f.FlightStatusId.Value, out var sn)
                    ? $"{f.FlightStatusId.Value} · {Markup.Escape(sn)}"
                    : f.FlightStatusId.Value.ToString(),
                $"{f.AvailableSeats.Value}/{f.TotalCapacity.Value}");
        }

        AnsiConsole.Write(table);
        Pause();
    }

    private async Task ChangeStatusAsync(CancellationToken cancellationToken)
    {
        var flight = await PromptFlightSelectionAsync("Seleccione el vuelo a actualizar:");
        if (flight is null)
            return;

        var id = flight.Id?.Value ?? 0;

        var statuses = (await _statuses.GetAllAsync()).ToList();
        if (statuses.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No hay estados de vuelo en catálogo.[/]");
            Pause();
            return;
        }

        var pick = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Nuevo estado")
                .AddChoices(statuses.Select(s => $"{s.Id!.Value} — {s.Name.Value}")));

        var idPart = pick.Split('—')[0].Trim();
        if (!int.TryParse(idPart, out var newStatusId))
        {
            AnsiConsole.MarkupLine("[red]Estado inválido.[/]");
            Pause();
            return;
        }

        try
        {
            await _operational.ChangeFlightStatusAsync(id, newStatusId, cancellationToken);
            AnsiConsole.MarkupLine("[green]Estado actualizado.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task ConfirmCheckInAsync(CancellationToken cancellationToken)
    {
        var flight = await PromptFlightSelectionAsync("Seleccione el vuelo para confirmar check-in:");
        if (flight is null)
            return;

        var id = flight.Id?.Value ?? 0;

        try
        {
            await _operational.ConfirmCheckInAsync(id, cancellationToken);
            AnsiConsole.MarkupLine("[green]Check-in confirmado.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task CancelCheckInAsync(CancellationToken cancellationToken)
    {
        var flight = await PromptFlightSelectionAsync("Seleccione el vuelo para cancelar check-in:");
        if (flight is null)
            return;

        var id = flight.Id?.Value ?? 0;

        try
        {
            await _operational.CancelCheckInAsync(id, cancellationToken);
            AnsiConsole.MarkupLine("[green]Check-in cancelado (asiento liberado).[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private static void Pause()
    {
        AnsiConsole.MarkupLine("\n[grey]Pulsa Enter...[/]");
        AnsiConsole.Console.Input.ReadKey(true);
    }

    private async Task<DomainFlights?> PromptFlightSelectionAsync(string title)
    {
        var flights = (await _flights.GetAllAsync())
            .Where(f => f.Id is not null)
            .OrderBy(f => f.DepartureAt.Value)
            .ThenBy(f => f.Code.Value)
            .ToList();

        if (flights.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay vuelos registrados.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(14)
                .AddChoices(flights.Select(FormatFlightChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return flights.First(f => FormatFlightChoice(f) == selected);
    }

    private static string FormatFlightChoice(DomainFlights flight)
    {
        return $"{flight.Id?.Value} · {Markup.Escape(flight.Code.Value)} · {flight.DepartureAt.Value:yyyy-MM-dd HH:mm}";
    }
}
