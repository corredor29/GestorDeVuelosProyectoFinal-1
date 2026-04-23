using GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Reports.UI;

public sealed class ReportsMenu : IModuleUI
{
    private readonly IReportsService _service;

    public ReportsMenu(IReportsService service)
    {
        _service = service;
    }

    public string Key => "reports";
    public string Title => "Reportes (administración)";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Reportes · administración [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Vuelos con mayor ocupación",
                        "Vuelos con asientos disponibles",
                        "Usuarios registrados (cuenta, documento, reservas)",
                        "Top 5 clientes con más reservas",
                        "Top 5 destinos más solicitados",
                        "Reservas por estado",
                        "Ingresos estimados por aerolínea",
                        "Tiquetes emitidos por rango de fechas",
                        "Volver"));

            switch (option)
            {
                case "Vuelos con mayor ocupación":
                    await ShowFlightOccupancyAsync(cancellationToken);
                    break;
                case "Vuelos con asientos disponibles":
                    await ShowAvailableFlightsAsync(cancellationToken);
                    break;
                case "Usuarios registrados (cuenta, documento, reservas)":
                    await ShowRegisteredUsersReportAsync(cancellationToken);
                    break;
                case "Top 5 clientes con más reservas":
                    await ShowTopClientsAsync(cancellationToken);
                    break;
                case "Top 5 destinos más solicitados":
                    await ShowTopDestinationsAsync(cancellationToken);
                    break;
                case "Reservas por estado":
                    await ShowBookingsByStatusAsync(cancellationToken);
                    break;
                case "Ingresos estimados por aerolínea":
                    await ShowRevenueByAirlineAsync(cancellationToken);
                    break;
                case "Tiquetes emitidos por rango de fechas":
                    await ShowTicketsByDateRangeAsync(cancellationToken);
                    break;
                case "Volver":
                    return;
            }
        }
    }

    // ── Vuelos con mayor ocupación ───────────────────────────────────────────

    private async Task ShowFlightOccupancyAsync(CancellationToken ct)
    {
        AnsiConsole.Clear();
        var data = (await _service.GetFlightOccupancyAsync(ct)).ToList();

        if (data.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay vuelos registrados.[/]");
            Pause();
            return;
        }

        var table = new Table()
            .Title("Vuelos con mayor ocupación")
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold grey]Vuelo[/]")
            .AddColumn("[bold grey]Origen[/]")
            .AddColumn("[bold grey]Destino[/]")
            .AddColumn("[bold grey]Capacidad[/]")
            .AddColumn("[bold grey]Ocupados[/]")
            .AddColumn("[bold grey]Disponibles[/]")
            .AddColumn("[bold grey]Ocupación %[/]");

        foreach (var r in data)
        {
            // Text/Style evita el parser de markup ([], %) que rompe con datos reales.
            var occColor = OccupancyForeground(r.OccupancyPercent);
            table.AddRow(
                Cell(r.FlightCode),
                Cell(r.OriginIata),
                Cell(r.DestinationIata),
                Cell(r.TotalCapacity.ToString()),
                Cell(r.OccupiedSeats.ToString()),
                Cell(r.AvailableSeats.ToString()),
                new Text($"{r.OccupancyPercent:0.##}%", new Style(foreground: occColor)));
        }

        AnsiConsole.Write(table);
        Pause();
    }

    // ── Vuelos con asientos disponibles ─────────────────────────────────────

    private async Task ShowAvailableFlightsAsync(CancellationToken ct)
    {
        AnsiConsole.Clear();
        var data = (await _service.GetAvailableFlightsAsync(ct)).ToList();

        if (data.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay vuelos con asientos disponibles.[/]");
            Pause();
            return;
        }

        var table = new Table()
            .Title("Vuelos con asientos disponibles")
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold grey]Vuelo[/]")
            .AddColumn("[bold grey]Origen[/]")
            .AddColumn("[bold grey]Destino[/]")
            .AddColumn("[bold grey]Capacidad[/]")
            .AddColumn("[bold grey]Disponibles[/]")
            .AddColumn("[bold grey]Ocupación %[/]");

        foreach (var r in data)
        {
            table.AddRow(
                Cell(r.FlightCode),
                Cell(r.OriginIata),
                Cell(r.DestinationIata),
                Cell(r.TotalCapacity.ToString()),
                Cell(r.AvailableSeats.ToString()),
                Cell($"{r.OccupancyPercent:0.##}%"));
        }

        AnsiConsole.Write(table);
        Pause();
    }

    // ── Todos los usuarios registrados (tabla users + persona) ─────────────

    private async Task ShowRegisteredUsersReportAsync(CancellationToken ct)
    {
        AnsiConsole.Clear();
        var data = (await _service.GetRegisteredUsersReportAsync(ct)).ToList();

        if (data.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay usuarios con persona asociada en el sistema.[/]");
            Pause();
            return;
        }

        var table = new Table()
            .Title("Usuarios registrados · reservas y totales")
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold grey]#[/]")
            .AddColumn("[bold grey]Usuario (login)[/]")
            .AddColumn("[bold grey]Nombre[/]")
            .AddColumn("[bold grey]Documento[/]")
            .AddColumn("[bold grey]Reservas[/]")
            .AddColumn("[bold grey]Total gastado[/]");

        var rank = 1;
        foreach (var r in data)
        {
            table.AddRow(
                Cell((rank++).ToString()),
                Cell(string.IsNullOrWhiteSpace(r.AccountUsername) ? "—" : r.AccountUsername),
                Cell(r.ClientDisplayName),
                Cell(r.IdentityDocument),
                Cell(r.TotalBookings.ToString()),
                Cell(r.TotalSpent.ToString("C")));
        }

        AnsiConsole.Write(table);
        Pause();
    }

    // ── Top clientes (solo quienes tienen reservas, ranking top 5) ────────────

    private async Task ShowTopClientsAsync(CancellationToken ct)
    {
        AnsiConsole.Clear();
        var data = (await _service.GetTopClientsAsync(5, ct)).ToList();

        if (data.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay clientes con reservas.[/]");
            Pause();
            return;
        }

        var table = new Table()
            .Title("Top 5 · solo clientes con al menos una reserva")
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold grey]#[/]")
            .AddColumn("[bold grey]Usuario (login)[/]")
            .AddColumn("[bold grey]Nombre[/]")
            .AddColumn("[bold grey]Documento[/]")
            .AddColumn("[bold grey]Reservas[/]")
            .AddColumn("[bold grey]Total gastado[/]");

        var rank = 1;
        foreach (var r in data)
        {
            table.AddRow(
                Cell((rank++).ToString()),
                Cell(string.IsNullOrWhiteSpace(r.AccountUsername) ? "—" : r.AccountUsername),
                Cell(r.ClientDisplayName),
                Cell(r.IdentityDocument),
                Cell(r.TotalBookings.ToString()),
                Cell(r.TotalSpent.ToString("C")));
        }

        AnsiConsole.Write(table);
        Pause();
    }

    // ── Top destinos ─────────────────────────────────────────────────────────

    private async Task ShowTopDestinationsAsync(CancellationToken ct)
    {
        AnsiConsole.Clear();
        var data = (await _service.GetTopDestinationsAsync(5, ct)).ToList();

        if (data.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay destinos registrados.[/]");
            Pause();
            return;
        }

        var table = new Table()
            .Title("Top 5 destinos más solicitados")
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold grey]#[/]")
            .AddColumn("[bold grey]Aeropuerto[/]")
            .AddColumn("[bold grey]IATA[/]")
            .AddColumn("[bold grey]Total reservas[/]");

        var rank = 1;
        foreach (var r in data)
        {
            table.AddRow(
                Cell((rank++).ToString()),
                Cell(r.AirportName),
                Cell(r.IataCode),
                Cell(r.TotalBookings.ToString()));
        }

        AnsiConsole.Write(table);
        Pause();
    }

    // ── Reservas por estado ───────────────────────────────────────────────────

    private async Task ShowBookingsByStatusAsync(CancellationToken ct)
    {
        AnsiConsole.Clear();
        var data = (await _service.GetBookingsByStatusAsync(ct)).ToList();

        if (data.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay reservas registradas.[/]");
            Pause();
            return;
        }

        var table = new Table()
            .Title("Reservas por estado")
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold grey]Estado[/]")
            .AddColumn("[bold grey]Cantidad[/]")
            .AddColumn("[bold grey]Monto total[/]");

        foreach (var r in data)
        {
            table.AddRow(
                Cell(r.StatusName),
                Cell(r.Count.ToString()),
                Cell(r.TotalAmount.ToString("C")));
        }

        AnsiConsole.Write(table);
        Pause();
    }

    // ── Ingresos por aerolínea ────────────────────────────────────────────────

    private async Task ShowRevenueByAirlineAsync(CancellationToken ct)
    {
        AnsiConsole.Clear();
        var data = (await _service.GetRevenueByAirlineAsync(ct)).ToList();

        if (data.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay datos de ingresos.[/]");
            Pause();
            return;
        }

        var table = new Table()
            .Title("Ingresos estimados por aerolínea")
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold grey]Aerolínea[/]")
            .AddColumn("[bold grey]IATA[/]")
            .AddColumn("[bold grey]Ingresos[/]")
            .AddColumn("[bold grey]Vuelos[/]");

        foreach (var r in data)
        {
            table.AddRow(
                Cell(r.AirlineName),
                Cell(r.IataCode),
                Cell(r.TotalRevenue.ToString("C")),
                Cell(r.TotalFlights.ToString()));
        }

        AnsiConsole.Write(table);
        Pause();
    }

    // ── Tiquetes por rango de fechas ──────────────────────────────────────────

    private async Task ShowTicketsByDateRangeAsync(CancellationToken ct)
    {
        AnsiConsole.Clear();

        var fromDate = PromptDateTime("Fecha inicial (yyyy-MM-dd):", DateTime.UtcNow.AddDays(-30));
        var toDate   = PromptDateTime("Fecha final   (yyyy-MM-dd):", DateTime.UtcNow);

        try
        {
            var data = (await _service.GetTicketsByDateRangeAsync(fromDate, toDate, ct)).ToList();

            if (data.Count == 0)
            {
                AnsiConsole.MarkupLine("\n[yellow]No hay tiquetes en ese rango de fechas.[/]");
                Pause();
                return;
            }

            var table = new Table()
                .Title($"Tiquetes del {fromDate:yyyy-MM-dd} al {toDate:yyyy-MM-dd}")
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Grey)
                .AddColumn("[bold grey]Código[/]")
                .AddColumn("[bold grey]Vuelo[/]")
                .AddColumn("[bold grey]Origen[/]")
                .AddColumn("[bold grey]Destino[/]")
                .AddColumn("[bold grey]Fecha emisión[/]")
                .AddColumn("[bold grey]Estado[/]");

            foreach (var r in data)
            {
                table.AddRow(
                    Cell(r.TicketCode),
                    Cell(r.FlightCode),
                    Cell(r.OriginIata),
                    Cell(r.DestinationIata),
                    Cell(r.IssuedAt.ToString("yyyy-MM-dd HH:mm")),
                    Cell(r.TicketStatus));
            }

            AnsiConsole.Write(table);
        }
        catch (ArgumentException ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>Celda de tabla sin interpretar corchetes / % como markup de Spectre.</summary>
    private static Text Cell(string? text) => new(text ?? "");

    private static Color OccupancyForeground(double occupancyPercent) =>
        occupancyPercent >= 90 ? Color.Red :
        occupancyPercent >= 60 ? Color.Yellow : Color.Green;

    private static DateTime PromptDateTime(string label, DateTime defaultValue)
    {
        var value = AnsiConsole.Prompt(
            new TextPrompt<string>($"[deepskyblue1]{label}[/]")
                .DefaultValue(defaultValue.ToString("yyyy-MM-dd"))
                .Validate(text => DateTime.TryParse(text, out _)
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red]Fecha inválida. Usa formato yyyy-MM-dd[/]")));

        return DateTime.Parse(value);
    }

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Presiona Enter para continuar...[/]").AllowEmpty());
    }
}
