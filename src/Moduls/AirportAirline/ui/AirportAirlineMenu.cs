using GestorDeVuelosProyectoFinal.Moduls.Airlines.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.ui;

public sealed class AirportAirlineMenu : IModuleUI
{
    private readonly IAirportAirlineService _service;
    private readonly IAirportsService _airports;
    private readonly IAirlinesService _airlines;

    public string Key => "airport_airline";
    public string Title => "🛫  Gestión de Operación Aeropuerto-Aerolínea";

    public AirportAirlineMenu(IAirportAirlineService service, IAirportsService airports, IAirlinesService airlines)
    {
        _service = service;
        _airports = airports;
        _airlines = airlines;
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
        // Aquí la UI navega entre varios catálogos relacionados, por eso usa ayudas de selección.
        while (!ct.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule($"[yellow]{Title}[/]").RuleStyle("grey").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Seleccione una opción:")
                    .PageSize(14)
                    .AddChoices(new[]
                    {
                        "1. Listar todas las operaciones",
                        "2. Listar solo operaciones activas",
                        "3. Buscar por ID",
                        "4. Buscar por Terminal",
                        "5. Buscar por nombre de aeropuerto",
                        "6. Buscar por nombre de aerolínea",
                        "7. Buscar por fecha de inicio",
                        "8. Buscar por fecha de fin",
                        "9. Registrar nueva Operación",
                        "10. Actualizar Operación",
                        "11. Desactivar Operación",
                        "12. Reactivar Operación",
                        "0. Volver al menú principal"
                    }));

            if (option.StartsWith("0"))
                break;

            switch (option.Split('.')[0])
            {
                case "1": await ListAllAsync(); break;
                case "2": await ListActiveAsync(); break;
                case "3": await SearchByIdAsync(); break;
                case "4": await SearchByTerminalAsync(); break;
                case "5": await SearchByAirportNameAsync(); break;
                case "6": await SearchByAirlineNameAsync(); break;
                case "7": await SearchByStartDateAsync(); break;
                case "8": await SearchByEndDateAsync(); break;
                case "9": await CreateAsync(); break;
                case "10": await UpdateAsync(); break;
                case "11": await DeactivateMenuAsync(); break;
                case "12": await ReactivateAsync(); break;
            }
        }
    }

    private async Task ListAllAsync()
    {
        var items = await _service.GetAllAsync();
        await ShowTableAsync(items, "Todas las Operaciones");
        Pause();
    }

    private async Task ListActiveAsync()
    {
        var items = await _service.GetActiveAsync();
        await ShowTableAsync(items, "Operaciones Activas");
        Pause();
    }

    private async Task SearchByIdAsync()
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("Ingrese el [green]ID[/] de la operación:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value);

        if (item is null)
            AnsiConsole.MarkupLine("[red]❌ No se encontró ninguna operación con ese ID.[/]");
        else
            await ShowTableAsync(new[] { item }, $"Resultado para ID: {id.Value}");

        Pause();
    }

    private async Task SearchByTerminalAsync()
    {
        var terminal = ConsoleMenuHelpers.PromptRequiredStringOrBack("Ingrese la [green]terminal[/]:");
        if (terminal is null)
            return;

        var items = await _service.GetByTerminalAsync(terminal);
        await ShowSearchResultsAsync(items, $"Operaciones con terminal: {terminal}");
    }

    private async Task SearchByAirportNameAsync()
    {
        var airportId = await PromptAirportSelectionAsync();
        if (airportId is null)
        {
            Pause();
            return;
        }

        var items = await _service.GetByAirportIdAsync(airportId.Value);
        await ShowSearchResultsAsync(items, $"Operaciones del aeropuerto (#{airportId})");
    }

    private async Task SearchByAirlineNameAsync()
    {
        var airlineId = await PromptAirlineSelectionAsync();
        if (airlineId is null)
        {
            Pause();
            return;
        }

        var items = await _service.GetByAirlineIdAsync(airlineId.Value);
        await ShowSearchResultsAsync(items, $"Operaciones de la aerolínea (#{airlineId})");
    }

    private async Task<int?> PromptAirportSelectionAsync(int? currentAirportId = null)
    {
        // La lista evita obligar al usuario a recordar ids de aeropuerto.
        var airports = (await _airports.GetAllAsync())
            .OrderBy(a => a.Name.Value)
            .ToList();

        if (airports.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay aeropuertos registrados.[/]");
            return null;
        }

        var title = currentAirportId is null
            ? "Seleccione el [green]aeropuerto[/]:"
            : $"Seleccione el [green]aeropuerto[/] [dim](actual id: {currentAirportId})[/]:";

        var sel = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(15)
                .AddChoices(airports.Select(FormatAirportChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (sel == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return airports.First(a => FormatAirportChoice(a) == sel).Id!.Value;
    }

    private async Task<int?> PromptAirlineSelectionAsync(int? currentAirlineId = null)
    {
        // Igual con aerolíneas: se arma una selección amigable y consistente con el resto del admin.
        var airlines = (await _airlines.GetAllAsync())
            .OrderBy(a => a.Name.Value)
            .ToList();

        if (airlines.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay aerolíneas registradas.[/]");
            return null;
        }

        var title = currentAirlineId is null
            ? "Seleccione la [green]aerolínea[/]:"
            : $"Seleccione la [green]aerolínea[/] [dim](actual id: {currentAirlineId})[/]:";

        var sel = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(15)
                .AddChoices(airlines.Select(FormatAirlineChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (sel == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return airlines.First(a => FormatAirlineChoice(a) == sel).Id!.Value;
    }

    private async Task SearchByStartDateAsync()
    {
        var startDate = ConsoleMenuHelpers.PromptDateTimeOrBack("Ingrese la [green]fecha de inicio[/] (yyyy-MM-dd):", "yyyy-MM-dd");
        if (startDate is null)
            return;

        var items = await _service.GetByStartDateAsync(startDate.Value);
        await ShowSearchResultsAsync(items, $"Operaciones con inicio: {startDate:yyyy-MM-dd}");
    }

    private async Task SearchByEndDateAsync()
    {
        var endDate = ConsoleMenuHelpers.PromptDateTimeOrBack("Ingrese la [green]fecha de fin[/] (yyyy-MM-dd):", "yyyy-MM-dd");
        if (endDate is null)
            return;

        var items = await _service.GetByEndDateAsync(endDate.Value);
        await ShowSearchResultsAsync(items, $"Operaciones con fin: {endDate:yyyy-MM-dd}");
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Registrar Nueva Operación[/]");
        var airportId = await PromptAirportSelectionAsync();
        if (airportId is null)
        {
            Pause();
            return;
        }

        var airlineId = await PromptAirlineSelectionAsync();
        if (airlineId is null)
        {
            Pause();
            return;
        }

        var terminal = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Terminal (opcional):", string.Empty, allowEmpty: true);
        if (terminal is null)
        {
            Pause();
            return;
        }

        var startDate = ConsoleMenuHelpers.PromptDateTimeOrBack("Fecha de inicio (yyyy-MM-dd):", "yyyy-MM-dd");
        if (startDate is null)
        {
            Pause();
            return;
        }

        var endDateInput = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Fecha de fin (opcional, yyyy-MM-dd):", string.Empty, allowEmpty: true);
        if (endDateInput is null)
        {
            Pause();
            return;
        }

        var isActive = AnsiConsole.Confirm("¿Registrar como activa?", true);

        if (AnsiConsole.Confirm("¿Desea guardar los cambios?"))
        {
            try
            {
                await _service.CreateAsync(
                    airportId.Value,
                    airlineId.Value,
                    string.IsNullOrWhiteSpace(terminal) ? null : terminal,
                    startDate.Value,
                    ParseNullableDate(endDateInput),
                    isActive);
                AnsiConsole.MarkupLine("[green]✅ Operación registrada correctamente.[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
            }
        }

        Pause();
    }

    private async Task UpdateAsync()
    {
        var operations = (await _service.GetAllAsync())
            .OrderBy(x => x.AirportId.Value)
            .ThenBy(x => x.AirlineId.Value)
            .ThenBy(x => x.StartDate.Value)
            .ToList();

        if (operations.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay operaciones registradas.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione la operación a modificar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(operations.Select(FormatOperationChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var item = operations.First(x => FormatOperationChoice(x) == selected);

        var airportId = await PromptAirportSelectionAsync(item.AirportId.Value);
        if (airportId is null)
        {
            Pause();
            return;
        }

        var airlineId = await PromptAirlineSelectionAsync(item.AirlineId.Value);
        if (airlineId is null)
        {
            Pause();
            return;
        }

        var terminal = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nueva terminal:", item.Terminal.Value ?? string.Empty, allowEmpty: true);
        if (terminal is null)
        {
            Pause();
            return;
        }

        var startDate = ConsoleMenuHelpers.PromptDateTimeOrBack("Nueva fecha de inicio (yyyy-MM-dd):", "yyyy-MM-dd");
        if (startDate is null)
        {
            Pause();
            return;
        }

        var endDateInput = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nueva fecha de fin:", item.EndDate.Value?.ToString("yyyy-MM-dd") ?? string.Empty, allowEmpty: true);
        if (endDateInput is null)
        {
            Pause();
            return;
        }

        var isActive = AnsiConsole.Confirm("¿La operación está activa?", item.IsActive.Value);

        try
        {
            await _service.UpdateAsync(item.Id!.Value, airportId.Value, airlineId.Value, string.IsNullOrWhiteSpace(terminal) ? null : terminal, startDate.Value, ParseNullableDate(endDateInput), isActive);
            AnsiConsole.MarkupLine("[green]✅ Operación actualizada.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private async Task DeactivateMenuAsync()
    {
        var operations = (await _service.GetActiveAsync())
            .OrderBy(x => x.AirportId.Value)
            .ThenBy(x => x.AirlineId.Value)
            .ThenBy(x => x.StartDate.Value)
            .ToList();

        if (operations.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay operaciones activas para desactivar.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red]Seleccione la operación a desactivar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(operations.Select(FormatOperationChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var item = operations.First(x => FormatOperationChoice(x) == selected);

        try
        {
            if (AnsiConsole.Confirm("¿Desea desactivar la operación?"))
            {
                await _service.DeactivateByIdAsync(item.Id!.Value);
                AnsiConsole.MarkupLine("[green]✅ Operación procesada.[/]");
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private async Task ReactivateAsync()
    {
        var operations = (await _service.GetAllAsync())
            .Where(x => !x.IsActive.Value)
            .OrderBy(x => x.AirportId.Value)
            .ThenBy(x => x.AirlineId.Value)
            .ThenBy(x => x.StartDate.Value)
            .ToList();

        if (operations.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay operaciones inactivas para reactivar.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione la operación a reactivar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(operations.Select(FormatOperationChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var item = operations.First(x => FormatOperationChoice(x) == selected);

        try
        {
            await _service.ReactivateAsync(item.Id!.Value);
            AnsiConsole.MarkupLine("[green]✅ Operación reactivada.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private async Task ShowSearchResultsAsync(IEnumerable<AirportAirlineOperation> items, string title)
    {
        var list = items.ToList();
        if (list.Count == 0)
            AnsiConsole.MarkupLine("[red]❌ No se encontraron registros.[/]");
        else
            await ShowTableAsync(list, title);

        Pause();
    }

    private async Task ShowTableAsync(IEnumerable<AirportAirlineOperation> items, string title)
    {
        var airports = (await _airports.GetAllAsync())
            .Where(x => x.Id is not null)
            .ToDictionary(x => x.Id!.Value, x => x.Name.Value);

        var airlines = (await _airlines.GetAllAsync())
            .Where(x => x.Id is not null)
            .ToDictionary(x => x.Id!.Value, x => x.Name.Value);

        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .AddColumn("[yellow]ID[/]")
            .AddColumn("[green]Aeropuerto[/]")
            .AddColumn("[blue]Aerolínea[/]")
            .AddColumn("[green]Terminal[/]")
            .AddColumn("[blue]Fecha inicio[/]")
            .AddColumn("[blue]Fecha fin[/]")
            .AddColumn("[green]Activo[/]");

        foreach (var item in items)
        {
            var airportLabel = airports.TryGetValue(item.AirportId.Value, out var ap)
                ? $"{item.AirportId.Value} · {Markup.Escape(ap)}"
                : item.AirportId.Value.ToString();

            var airlineLabel = airlines.TryGetValue(item.AirlineId.Value, out var al)
                ? $"{item.AirlineId.Value} · {Markup.Escape(al)}"
                : item.AirlineId.Value.ToString();

            table.AddRow(
                (item.Id?.Value ?? 0).ToString(),
                airportLabel,
                airlineLabel,
                item.Terminal.Value ?? "-",
                item.StartDate.Value.ToString("yyyy-MM-dd"),
                item.EndDate.Value?.ToString("yyyy-MM-dd") ?? "-",
                item.IsActive.Value ? "Sí" : "No");
        }

        AnsiConsole.Write(table);
    }

    private static DateTime? ParseNullableDate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        if (DateTime.TryParseExact(
                input.Trim(),
                new[]
                {
                    "yyyy-MM-dd",
                    "yyyy-MM-dd HH:mm",
                    "dd-MM-yyyy",
                    "dd-MM-yyyy HH:mm",
                    "dd/MM/yyyy",
                    "dd/MM/yyyy HH:mm"
                },
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AllowWhiteSpaces,
                out var dt))
            return dt;

        return DateTime.TryParse(input.Trim(), out dt) ? dt : null;
    }

    private static void Pause()
    {
        AnsiConsole.MarkupLine("\nPresione [grey]ENTER[/] para continuar...");
        Console.ReadLine();
    }

    private static string FormatAirportChoice(GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Aggregate.Airport airport) =>
        $"{airport.Id!.Value} · {Markup.Escape(airport.Name.Value)} · {Markup.Escape(airport.IataCode.Value)}";

    private static string FormatAirlineChoice(GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Aggregate.Airline airline) =>
        $"{airline.Id!.Value} · {Markup.Escape(airline.Name.Value)} · {Markup.Escape(airline.IataCode.Value)}";

    private static string FormatOperationChoice(AirportAirlineOperation item) =>
        $"{item.Id?.Value ?? 0} · Aeropuerto {item.AirportId.Value} · Aerolínea {item.AirlineId.Value} · {item.StartDate.Value:yyyy-MM-dd}";
}
