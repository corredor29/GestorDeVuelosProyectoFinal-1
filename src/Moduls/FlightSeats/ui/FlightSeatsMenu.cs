using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using FlightSeatAggregate = GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.Aggregate.FlightSeat;
using System.Text.RegularExpressions;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.ui;

public sealed class FlightSeatsMenu : IModuleUI
{
    private readonly IFlightSeatsService _service;

    public FlightSeatsMenu(IFlightSeatsService service)
    {
        _service = service;
    }

    public string Key => "flight-seats";
    public string Title => "Asientos de Vuelo";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Gestión de Asientos [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Selecciona una acción[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Listar todos",
                        "Buscar por ID",
                        "Listar por Vuelo",
                        "Verificar disponibilidad",
                        "Crear asiento",
                        "Actualizar asiento",
                        "Eliminar asiento",
                        "Volver"));

            switch (option)
            {
                case "Listar todos": await ListAllAsync(cancellationToken); break;
                case "Buscar por ID": await SearchByIdAsync(cancellationToken); break;
                case "Listar por Vuelo": await ListByFlightAsync(cancellationToken); break;
                case "Verificar disponibilidad": await CheckStatusAsync(cancellationToken); break;
                case "Crear asiento": await CreateAsync(cancellationToken); break;
                case "Actualizar asiento": await UpdateAsync(cancellationToken); break;
                case "Eliminar asiento": await DeleteAsync(cancellationToken); break;
                case "Volver": return;
            }
        }
    }

    private async Task ListAllAsync(CancellationToken ct)
    {
        RenderTable(await _service.GetAllAsync(ct), "Asientos Registrados");
        Pause();
    }

    private async Task ListByFlightAsync(CancellationToken ct)
    {
        var flightId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID del Vuelo:");
        if (flightId is null)
            return;

        RenderTable(await _service.GetByFlightIdAsync(flightId.Value, ct), $"Asientos del Vuelo {flightId.Value}");
        Pause();
    }

    private async Task CheckStatusAsync(CancellationToken ct)
    {
        var seat = await PromptSeatSelectionAsync("Seleccione el asiento a consultar:", ct);
        if (seat is null)
            return;

        var isOccupied = await _service.isOccupiedAsync(seat.Id.Value, ct);
        var color = isOccupied ? "red" : "green";
        var status = isOccupied ? "OCUPADO" : "DISPONIBLE";
        
        AnsiConsole.MarkupLine($"\nEl asiento {seat.Id.Value} está: [{color}]{status}[/]");
        Pause();
    }

    private async Task CreateAsync(CancellationToken ct)
    {
        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Registrar asiento"))
            return;

        var form = PromptForm();
        if (form is null)
            return;

        try {
            await _service.CreateAsync(form.Value.flightId, form.Value.cabinId, form.Value.locId, form.Value.occupied, form.Value.code, ct);
            AnsiConsole.MarkupLine("[green]Asiento creado con éxito.[/]");
        } catch (Exception ex) {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
        }
        Pause();
    }

    private async Task UpdateAsync(CancellationToken ct)
    {
        var seat = await PromptSeatSelectionAsync("Seleccione el asiento a actualizar:", ct);
        if (seat == null) return;

        var form = PromptForm(seat.FlightId.Value, seat.CabinTypeId.Value, seat.SeatLocationTypeId.Value, seat.IsOccupied.Value, seat.Code.Value);
        if (form is null)
            return;

        try {
            await _service.UpdateAsync(seat.Id.Value, form.Value.flightId, form.Value.cabinId, form.Value.locId, form.Value.occupied, form.Value.code, ct);
            AnsiConsole.MarkupLine("[green]Actualizado.[/]");
        } catch (Exception ex) {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }
        Pause();
    }

    private async Task DeleteAsync(CancellationToken ct)
    {
        var seat = await PromptSeatSelectionAsync("Seleccione el asiento a eliminar:", ct);
        if (seat is null)
            return;

        if (AnsiConsole.Confirm("¿Seguro?", false))
        {
            await _service.DeleteByIdAsync(seat.Id.Value, ct);
            AnsiConsole.MarkupLine("[green]Eliminado.[/]");
        }
        Pause();
    }

    private async Task SearchByIdAsync(CancellationToken ct)
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID:");
        if (id is null)
            return;

        var seat = await _service.GetByIdAsync(id.Value, ct);
        RenderTable(seat == null ? Array.Empty<FlightSeatAggregate>() : new[] { seat }, $"Resultado ID {id.Value}");
        Pause();
    }

    private static void RenderTable(IEnumerable<FlightSeatAggregate> items, string title)
    {
        var table = new Table().Title(title).Border(TableBorder.Rounded).BorderColor(Color.Grey);
        table.AddColumn("[grey]ID[/]").AddColumn("[grey]Vuelo[/]").AddColumn("[grey]Cabina[/]").AddColumn("[grey]Ubicación[/]").AddColumn("[grey]Código[/]").AddColumn("[grey]Estado[/]");

        foreach (var item in items)
        {
            var status = item.IsOccupied.Value ? "[red]Ocupado[/]" : "[green]Libre[/]";
            table.AddRow(item.Id.Value.ToString(), item.FlightId.Value.ToString(), item.CabinTypeId.Value.ToString(), item.SeatLocationTypeId.Value.ToString(), item.Code.Value, status);
        }
        AnsiConsole.Write(table);
    }

    private static (int flightId, int cabinId, int locId, bool occupied, string code)? PromptForm(int? fId=null, int? cId=null, int? lId=null, bool? occ=null, string? cod=null)
    {
        var flightId = fId is int currentFlightId
            ? ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("ID Vuelo:", currentFlightId)
            : ConsoleMenuHelpers.PromptPositiveIntOrBack("ID Vuelo:");
        if (flightId is null)
            return null;

        var cabinId = cId is int currentCabinId
            ? ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("ID Cabina:", currentCabinId)
            : ConsoleMenuHelpers.PromptPositiveIntOrBack("ID Cabina:");
        if (cabinId is null)
            return null;

        var locId = lId is int currentLocId
            ? ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("ID Ubicación:", currentLocId)
            : ConsoleMenuHelpers.PromptPositiveIntOrBack("ID Ubicación:");
        if (locId is null)
            return null;

        var code = PromptSeatCodeOrBack("Código del asiento (ej. 12A):", cod);
        if (code is null)
            return null;

        var occupied = PromptOccupiedSelection(occ);
        if (occupied is null)
            return null;

        return (flightId.Value, cabinId.Value, locId.Value, occupied.Value, code);
    }

    private static string? PromptSeatCodeOrBack(string label, string? current = null)
    {
        return ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            label,
            current ?? string.Empty,
            allowEmpty: false,
            validate: value =>
            {
                var text = value.Trim().ToUpperInvariant();
                if (string.IsNullOrWhiteSpace(text))
                    return "El código del asiento es obligatorio.";
                if (!Regex.IsMatch(text, @"^\d{1,3}[A-Z]$"))
                    return "Usa el formato fila y letra, por ejemplo 12A.";
                return null;
            })?.ToUpperInvariant();
    }

    private static bool? PromptOccupiedSelection(bool? current = null)
    {
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Estado del asiento:")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices(
                    current == true ? "Sí, ocupado" : "No, libre",
                    current == true ? "No, libre" : "Sí, ocupado",
                    ConsoleMenuHelpers.VolverAlMenu));

        return selected switch
        {
            "Sí, ocupado" => true,
            "No, libre" => false,
            _ => null
        };
    }

    private async Task<FlightSeatAggregate?> PromptSeatSelectionAsync(string title, CancellationToken ct)
    {
        var seats = (await _service.GetAllAsync(ct))
            .OrderBy(s => s.FlightId.Value)
            .ThenBy(s => s.Code.Value)
            .ToList();
        if (seats.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay asientos registrados.[/]");
            Pause();
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(seats.Select(FormatSeatChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return seats.First(s => FormatSeatChoice(s) == selected);
    }

    private static string FormatSeatChoice(FlightSeatAggregate seat) =>
        $"{seat.Id.Value} · Vuelo {seat.FlightId.Value} · {Markup.Escape(seat.Code.Value)}";

    private static void Pause() => AnsiConsole.Prompt(new TextPrompt<string>("\n[grey]Presiona Enter para continuar...[/]").AllowEmpty());
}
