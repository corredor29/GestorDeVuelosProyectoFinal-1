using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Session;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Checkins.UI;

public class CheckinsConsoleUI
{
    private readonly ICheckinsService _service;

    public CheckinsConsoleUI(ICheckinsService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Check-ins").Color(Color.Blue));

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Selecciona una opción:[/]")
                    .AddChoices(GetMenuOptions()));

            switch (option)
            {
                case "Listar check-ins":
                    await ListAllAsync();
                    break;
                case "Buscar por ID":
                    await GetByIdAsync();
                    break;
                case "Crear check-in":
                    await CreateAsync();
                    break;
                case "Actualizar estado":
                    await UpdateAsync();
                    break;
                case "Eliminar check-in":
                    await DeleteAsync();
                    break;
                case "Volver":
                    return;
            }

            AnsiConsole.WriteLine();
            AnsiConsole.Markup("[grey]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }
    }

    private IEnumerable<string> GetMenuOptions()
    {
        var options = new List<string>
        {
            "Listar check-ins",
            "Buscar por ID"
        };

        if (UserSession.Current?.IsAdmin == true)
        {
            options.Add("Crear check-in");
            options.Add("Actualizar estado");
            options.Add("Eliminar check-in");
        }

        options.Add("Volver");
        return options;
    }

    private async Task ListAllAsync()
    {
        var items = await _service.GetAllAsync();
        var list = items.ToList();

        if (!list.Any())
        {
            AnsiConsole.MarkupLine("[grey]No hay check-ins registrados.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[blue]ID[/]")
            .AddColumn("[blue]Tiquete ID[/]")
            .AddColumn("[blue]Personal ID[/]")
            .AddColumn("[blue]Asiento ID[/]")
            .AddColumn("[blue]Fecha[/]")
            .AddColumn("[blue]Estado ID[/]")
            .AddColumn("[blue]Pase de abordar[/]");

        foreach (var item in list)
        {
            table.AddRow(
                item.Id.Value.ToString(),
                item.TicketId.Value.ToString(),
                item.StaffId.Value.ToString(),
                item.FlightSeatId.Value.ToString(),
                item.CheckedInAt.Value.ToString("yyyy-MM-dd HH:mm"),
                item.CheckinStatusId.Value.ToString(),
                item.BoardingPassNumber.Value
            );
        }

        AnsiConsole.Write(table);
    }

    private async Task GetByIdAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Buscar check-in por ID[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null) return;

        try
        {
            var item = await _service.GetByIdAsync(id.Value);

            if (item is null)
            {
                AnsiConsole.MarkupLine("[red]Check-in no encontrado.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[blue]Campo[/]")
                .AddColumn("[blue]Valor[/]");

            table.AddRow("ID",           item.Id.Value.ToString());
            table.AddRow("Tiquete ID", item.TicketId.Value.ToString());
            table.AddRow("Personal ID", item.StaffId.Value.ToString());
            table.AddRow("Asiento ID",   item.FlightSeatId.Value.ToString());
            table.AddRow("Fecha",        item.CheckedInAt.Value.ToString("yyyy-MM-dd HH:mm"));
            table.AddRow("Estado ID",    item.CheckinStatusId.Value.ToString());
            table.AddRow("Pase de abordar", item.BoardingPassNumber.Value);

            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
        }
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Crear nuevo check-in[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null) return;
        var ticketId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]Tiquete ID:[/]");
        if (ticketId is null) return;
        var staffId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]Personal ID:[/]");
        if (staffId is null) return;
        var flightSeatId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]Asiento ID:[/]");
        if (flightSeatId is null) return;
        var checkedInAt = ConsoleMenuHelpers.PromptDateTimeOrBack("[yellow]Fecha de check-in (yyyy-MM-dd HH:mm):[/]", "yyyy-MM-dd HH:mm");
        if (checkedInAt is null) return;
        var checkinStatusId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]Estado ID:[/]");
        if (checkinStatusId is null) return;
        var boardingPass = ConsoleMenuHelpers.PromptRequiredStringOrBack("[yellow]Pase de abordar:[/]");
        if (boardingPass is null) return;

        await AnsiConsole.Status()
            .StartAsync("Creando check-in...", async ctx =>
            {
                try
                {
                    var item = await _service.CreateAsync(
                        id.Value, ticketId.Value, staffId.Value, flightSeatId.Value,
                        checkedInAt.Value, checkinStatusId.Value, boardingPass);

                    AnsiConsole.MarkupLine($"[green]Check-in '[bold]{item.BoardingPassNumber.Value}[/]' creado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task UpdateAsync()
    {
        AnsiConsole.MarkupLine("[bold yellow]Actualizar estado de check-in[/]");

        var existing = await PromptCheckinSelectionAsync("[yellow]Seleccione el check-in a actualizar:[/]");
        if (existing is null)
            return;

        AnsiConsole.MarkupLine($"[grey]Estado actual: {existing.CheckinStatusId.Value}[/]");

        var newStatusId = ConsoleMenuHelpers.PromptIntOrBack("[yellow]Nuevo estado ID (vacío = mantener):[/]");

        await AnsiConsole.Status()
            .StartAsync("Actualizando check-in...", async ctx =>
            {
                try
                {
                    var updated = await _service.UpdateAsync(existing.Id.Value, newStatusId);
                    AnsiConsole.MarkupLine($"[green]Check-in '[bold]{updated.Id.Value}[/]' actualizado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task DeleteAsync()
    {
        AnsiConsole.MarkupLine("[bold red]Eliminar check-in[/]");

        var existing = await PromptCheckinSelectionAsync("[yellow]Seleccione el check-in a eliminar:[/]");
        if (existing is null) return;

        var confirm = AnsiConsole.Confirm($"[red]¿Estás seguro de eliminar el check-in {existing.Id.Value} del tiquete {existing.TicketId.Value}?[/]");

        if (!confirm)
        {
            AnsiConsole.MarkupLine("[grey]Operación cancelada.[/]");
            return;
        }

        await AnsiConsole.Status()
            .StartAsync("Eliminando check-in...", async ctx =>
            {
                try
                {
                    var deleted = await _service.DeleteAsync(existing.Id.Value);

                    if (deleted)
                        AnsiConsole.MarkupLine("[green]Check-in eliminado correctamente.[/]");
                    else
                        AnsiConsole.MarkupLine("[red]Check-in no encontrado.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task<dynamic?> PromptCheckinSelectionAsync(string title)
    {
        var items = (await _service.GetAllAsync()).OrderByDescending(x => x.CheckedInAt.Value).ToList();
        if (!items.Any())
        {
            AnsiConsole.MarkupLine("[grey]No hay check-ins registrados.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(items.Select(FormatCheckinChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return items.First(x => FormatCheckinChoice(x) == selected);
    }

    private static string FormatCheckinChoice(dynamic item) =>
        $"{item.Id.Value} · Tiquete {item.TicketId.Value} · {item.CheckedInAt.Value:yyyy-MM-dd HH:mm}";
}
