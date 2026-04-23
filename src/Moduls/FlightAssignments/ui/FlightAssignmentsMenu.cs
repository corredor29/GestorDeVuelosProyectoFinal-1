using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using System.Linq;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.ui;

public sealed class FlightAssignmentsMenu : IModuleUI
{
    private readonly IFlightAssignmentsService _service;
    private readonly IFlightsRepository _flights;
    private readonly IPersonalService _staff;
    private readonly IFlightRolesService _roles;

    public FlightAssignmentsMenu(
        IFlightAssignmentsService service,
        IFlightsRepository flights,
        IPersonalService staff,
        IFlightRolesService roles)
    {
        _service = service;
        _flights = flights;
        _staff = staff;
        _roles = roles;
    }

    public string Key => "flight-crew-assignments";
    public string Title => "Asignaciones de tripulación";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Asignaciones de tripulación [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Listar todas",
                        "Buscar por ID",
                        "Buscar por ID de vuelo",
                        "Buscar por ID de personal",
                        "Buscar por ID de rol",
                        "Crear asignación",
                        "Actualizar asignación",
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
                case "Buscar por ID de vuelo":
                    await SearchByFlightIdAsync(cancellationToken);
                    break;
                case "Buscar por ID de personal":
                    await SearchByStaffIdAsync(cancellationToken);
                    break;
                case "Buscar por ID de rol":
                    await SearchByRoleIdAsync(cancellationToken);
                    break;
                case "Crear asignación":
                    await CreateAsync(cancellationToken);
                    break;
                case "Actualizar asignación":
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
        await RenderTableAsync(await _service.GetAllAsync(cancellationToken), "Asignaciones de tripulación", cancellationToken);
        Pause();
    }

    private async Task SearchByIdAsync(CancellationToken cancellationToken)
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value, cancellationToken);
        await RenderTableAsync(item is null ? Array.Empty<FlightAssignment>() : new[] { item }, $"ID {id.Value}", cancellationToken);
        Pause();
    }

    private async Task SearchByFlightIdAsync(CancellationToken cancellationToken)
    {
        var flightId = await PromptFlightSelectionAsync(cancellationToken, "Seleccione el vuelo a consultar:");
        if (flightId is null)
            return;

        await RenderTableAsync(await _service.GetByFlightIdAsync(flightId.Value, cancellationToken), $"Asignaciones del vuelo {flightId.Value}", cancellationToken);
        Pause();
    }

    private async Task SearchByStaffIdAsync(CancellationToken cancellationToken)
    {
        var staffId = await PromptStaffSelectionAsync(cancellationToken, "Seleccione el personal a consultar:");
        if (staffId is null)
            return;

        await RenderTableAsync(await _service.GetByStaffIdAsync(staffId.Value, cancellationToken), $"Asignaciones del personal {staffId.Value}", cancellationToken);
        Pause();
    }

    private async Task SearchByRoleIdAsync(CancellationToken cancellationToken)
    {
        var roleId = await PromptRoleSelectionAsync(cancellationToken, "Seleccione el rol a consultar:");
        if (roleId is null)
            return;

        await RenderTableAsync(await _service.GetByRoleIdAsync(roleId.Value, cancellationToken), $"Asignaciones del rol {roleId.Value}", cancellationToken);
        Pause();
    }

    private async Task CreateAsync(CancellationToken cancellationToken)
    {
        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Registrar asignación de tripulación"))
            return;

        var flightId = await PromptFlightSelectionAsync(cancellationToken, "Seleccione el vuelo:");
        if (flightId is null)
            return;

        var staffId = await PromptStaffSelectionAsync(cancellationToken, "Seleccione el personal:");
        if (staffId is null)
            return;

        var flightRoleId = await PromptRoleSelectionAsync(cancellationToken, "Seleccione el rol de vuelo:");
        if (flightRoleId is null)
            return;

        try
        {
            await _service.CreateAsync(flightId.Value, staffId.Value, flightRoleId.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Asignación creada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task UpdateAsync(CancellationToken cancellationToken)
    {
        var current = await PromptAssignmentSelectionAsync(cancellationToken, "Seleccione la asignación a actualizar:");
        if (current is null)
        {
            return;
        }

        var flightId = await PromptFlightSelectionAsync(cancellationToken, "Seleccione el nuevo vuelo:", current.FlightId.Value);
        if (flightId is null)
            return;

        var staffId = await PromptStaffSelectionAsync(cancellationToken, "Seleccione el nuevo personal:", current.StaffId.Value);
        if (staffId is null)
            return;

        var flightRoleId = await PromptRoleSelectionAsync(cancellationToken, "Seleccione el nuevo rol de vuelo:", current.FlightRoleId.Value);
        if (flightRoleId is null)
            return;

        try
        {
            await _service.UpdateAsync(current.Id!.Value, flightId.Value, staffId.Value, flightRoleId.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Asignación actualizada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteByIdAsync(CancellationToken cancellationToken)
    {
        var assignment = await PromptAssignmentSelectionAsync(cancellationToken, "Seleccione la asignación a eliminar:");
        if (assignment is null)
            return;

        if (!AnsiConsole.Confirm("¿Confirmas la eliminación?", false))
        {
            Pause();
            return;
        }

        try
        {
            await _service.DeleteByIdAsync(assignment.Id!.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Asignación eliminada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task RenderTableAsync(IEnumerable<FlightAssignment> items, string title, CancellationToken cancellationToken)
    {
        var list = items.ToList();
        if (list.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay registros para mostrar.[/]");
            return;
        }

        var flights = (await _flights.GetAllAsync(cancellationToken))
            .Where(f => f.Id != null)
            .ToDictionary(f => f.Id!.Value, f => $"{f.Id!.Value} · {f.Code.Value}");

        var staff = (await _staff.GetAllAsync())
            .Where(s => s.Id != null)
            .ToDictionary(s => s.Id!.Value, s => $"{s.Id!.Value} · Persona {s.PersonId.Value}");

        var roles = (await _roles.GetAllAsync(cancellationToken))
            .Where(r => r.Id != null)
            .ToDictionary(r => r.Id!.Value, r => $"{r.Id!.Value} · {r.Name.Value}");

        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold grey]ID[/]")
            .AddColumn("[bold grey]Vuelo[/]")
            .AddColumn("[bold grey]Personal[/]")
            .AddColumn("[bold grey]Rol[/]");

        foreach (var item in list)
        {
            table.AddRow(
                item.Id?.Value.ToString() ?? "-",
                flights.TryGetValue(item.FlightId.Value, out var fl) ? Markup.Escape(fl) : item.FlightId.Value.ToString(),
                staff.TryGetValue(item.StaffId.Value, out var st) ? Markup.Escape(st) : item.StaffId.Value.ToString(),
                roles.TryGetValue(item.FlightRoleId.Value, out var rl) ? Markup.Escape(rl) : item.FlightRoleId.Value.ToString());
        }

        AnsiConsole.Write(table);
    }

    private async Task<int?> PromptFlightSelectionAsync(CancellationToken cancellationToken, string title, int? currentFlightId = null)
    {
        var flights = (await _flights.GetAllAsync(cancellationToken))
            .Where(f => f.Id != null)
            .OrderByDescending(f => f.Id!.Value == currentFlightId)
            .ThenBy(f => f.Code.Value)
            .ToList();
        if (flights.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay vuelos registrados.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(flights.Select(FormatFlightChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return flights.First(f => FormatFlightChoice(f) == selected).Id!.Value;
    }

    private async Task<int?> PromptStaffSelectionAsync(CancellationToken cancellationToken, string title, int? currentStaffId = null)
    {
        var staffItems = (await _staff.GetAllAsync())
            .Where(s => s.Id != null)
            .OrderByDescending(s => s.Id!.Value == currentStaffId)
            .ThenBy(s => s.PersonId.Value)
            .ToList();
        if (staffItems.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay personal registrado.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(staffItems.Select(FormatStaffChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return staffItems.First(s => FormatStaffChoice(s) == selected).Id!.Value;
    }

    private async Task<int?> PromptRoleSelectionAsync(CancellationToken cancellationToken, string title, int? currentRoleId = null)
    {
        var roles = (await _roles.GetAllAsync(cancellationToken))
            .Where(r => r.Id != null)
            .OrderByDescending(r => r.Id!.Value == currentRoleId)
            .ThenBy(r => r.Name.Value)
            .ToList();
        if (roles.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay roles de vuelo registrados.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(roles.Select(FormatRoleChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return roles.First(r => FormatRoleChoice(r) == selected).Id!.Value;
    }

    private async Task<FlightAssignment?> PromptAssignmentSelectionAsync(CancellationToken cancellationToken, string title)
    {
        var assignments = (await _service.GetAllAsync(cancellationToken)).ToList();
        if (assignments.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay asignaciones registradas.[/]");
            Pause();
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(assignments.Select(FormatAssignmentChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return assignments.First(a => FormatAssignmentChoice(a) == selected);
    }

    private static string FormatFlightChoice(dynamic flight) =>
        $"{flight.Id!.Value} · {Markup.Escape(flight.Code.Value)}";

    private static string FormatStaffChoice(dynamic staff) =>
        $"{staff.Id!.Value} · Persona {staff.PersonId.Value}";

    private static string FormatRoleChoice(FlightRole role) =>
        $"{role.Id!.Value} · {Markup.Escape(role.Name.Value)}";

    private static string FormatAssignmentChoice(FlightAssignment assignment) =>
        $"{assignment.Id?.Value ?? 0} · Vuelo {assignment.FlightId.Value} · Personal {assignment.StaffId.Value} · Rol {assignment.FlightRoleId.Value}";

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Presiona Enter para continuar...[/]").AllowEmpty());
    }
}
