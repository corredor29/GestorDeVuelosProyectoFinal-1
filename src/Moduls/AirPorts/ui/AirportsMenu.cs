using GestorDeVuelosProyectoFinal.Moduls.Airports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.Moduls.Airports.ui;

public sealed class AirportsMenu : IModuleUI
{
    private readonly IAirportsService _service;

    public string Key => "airports";
    public string Title => "🛬  Gestión de Aeropuertos";

    public AirportsMenu(IAirportsService service)
    {
        _service = service;
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
        // Aquí se mezclan búsquedas rápidas con el CRUD del catálogo de aeropuertos.
        while (!ct.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule($"[yellow]{Title}[/]").RuleStyle("grey").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Seleccione una opción:")
                    .PageSize(12)
                    .AddChoices(new[]
                    {
                        "1. Listar todos los aeropuertos",
                        "2. Buscar por ID",
                        "3. Buscar por Nombre",
                        "4. Buscar por IATA",
                        "5. Buscar por ICAO",
                        "6. Buscar por Ciudad ID",
                        "7. Buscar por Nombre de Ciudad",
                        "8. Registrar nuevo Aeropuerto",
                        "9. Actualizar Aeropuerto",
                        "10. Eliminar Aeropuerto",
                        "0. Volver al menú principal"
                    }));

            if (option.StartsWith("0"))
                break;

            switch (option.Split('.')[0])
            {
                case "1": await ListAllAsync(); break;
                case "2": await SearchByIdAsync(); break;
                case "3": await SearchByNameAsync(); break;
                case "4": await SearchByIataAsync(); break;
                case "5": await SearchByIcaoAsync(); break;
                case "6": await SearchByCityIdAsync(); break;
                case "7": await SearchByCityNameAsync(); break;
                case "8": await CreateAsync(); break;
                case "9": await UpdateAsync(); break;
                case "10": await DeleteMenuAsync(); break;
            }
        }
    }

    private async Task ListAllAsync()
    {
        var items = await _service.GetAllAsync();
        ShowTable(items, "Todos los Aeropuertos");
        Pause();
    }

    private async Task SearchByIdAsync()
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("Ingrese el [green]ID[/] del aeropuerto:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value);

        if (item is null)
            AnsiConsole.MarkupLine("[red]❌ No se encontró ningún aeropuerto con ese ID.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para ID: {id.Value}");

        Pause();
    }

    private async Task SearchByNameAsync()
    {
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Ingrese el [green]nombre[/] del aeropuerto:");
        if (name is null)
            return;

        var item = await _service.GetByNameAsync(name);

        if (item is null)
            AnsiConsole.MarkupLine("[red]❌ No se encontró ningún aeropuerto con ese nombre.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para: {name}");

        Pause();
    }

    private async Task SearchByIataAsync()
    {
        var iata = PromptIataCodeOrBack("Ingrese el [green]código IATA[/] del aeropuerto:");
        if (iata is null)
            return;

        var item = await _service.GetByIataCodeAsync(iata);

        if (item is null)
            AnsiConsole.MarkupLine("[red]❌ No se encontró ningún aeropuerto con ese código IATA.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para IATA: {iata.ToUpperInvariant()}");

        Pause();
    }

    private async Task SearchByIcaoAsync()
    {
        var icao = PromptIcaoCodeOrBack("Ingrese el [green]código ICAO[/] del aeropuerto:", allowEmpty: false);
        if (icao is null)
            return;

        var item = await _service.GetByIcaoCodeAsync(icao);

        if (item is null)
            AnsiConsole.MarkupLine("[red]❌ No se encontró ningún aeropuerto con ese código ICAO.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para ICAO: {icao.ToUpperInvariant()}");

        Pause();
    }

    private async Task SearchByCityIdAsync()
    {
        var cityId = ConsoleMenuHelpers.PromptPositiveIntOrBack("Ingrese el [green]ID de la ciudad[/]:");
        if (cityId is null)
            return;

        var items = await _service.GetByCityIdAsync(cityId.Value);
        ShowSearchResults(items, $"Aeropuertos de la ciudad #{cityId.Value}");
    }

    private async Task SearchByCityNameAsync()
    {
        // Este filtro es útil cuando el usuario recuerda la ciudad pero no el código del aeropuerto.
        var cityName = ConsoleMenuHelpers.PromptRequiredStringOrBack("Ingrese el [green]nombre[/] de la ciudad:");
        if (cityName is null)
            return;

        var items = await _service.GetByCityNameAsync(cityName);
        ShowSearchResults(items, $"Aeropuertos de la ciudad: {cityName}");
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Registrar Nuevo Aeropuerto[/]");
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre:");
        if (name is null)
        {
            Pause();
            return;
        }

        var iataCode = PromptIataCodeOrBack("Código IATA (máximo 3 letras):");
        if (iataCode is null)
        {
            Pause();
            return;
        }

        var icaoCode = PromptIcaoCodeOrBack("Código ICAO (4 letras, opcional):", allowEmpty: true);
        if (icaoCode is null)
        {
            Pause();
            return;
        }

        var cityId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID de la ciudad:");
        if (cityId is null)
        {
            Pause();
            return;
        }

        if (AnsiConsole.Confirm("¿Desea guardar los cambios?"))
        {
            try
            {
                await _service.CreateAsync(name, iataCode, string.IsNullOrWhiteSpace(icaoCode) ? null : icaoCode, cityId.Value);
                AnsiConsole.MarkupLine("[green]✅ Aeropuerto registrado correctamente.[/]");
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
        // Igual que en otros menús, primero se selecciona el registro y después se editan los campos.
        var item = await PromptSelectAirportForEditAsync();
        if (item is null)
        {
            Pause();
            return;
        }

        var id = item.Id!.Value;

        AnsiConsole.MarkupLine($"Modificando: [bold]{item.Name.Value}[/] ([blue]{item.IataCode.Value}[/])");
        var name = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nuevo nombre:", item.Name.Value);
        if (name is null)
        {
            Pause();
            return;
        }

        var iataCode = PromptIataCodeOrBack("Nuevo código IATA:", item.IataCode.Value);
        if (iataCode is null)
        {
            Pause();
            return;
        }

        var icaoCode = PromptIcaoCodeOrBack("Nuevo código ICAO:", allowEmpty: true, defaultValue: item.IcaoCode.Value ?? string.Empty);
        if (icaoCode is null)
        {
            Pause();
            return;
        }

        var cityId = ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("Nuevo ID de la ciudad:", item.CityId.Value);
        if (cityId is null)
        {
            Pause();
            return;
        }

        try
        {
            await _service.UpdateAsync(id, name, iataCode, string.IsNullOrWhiteSpace(icaoCode) ? null : icaoCode, cityId.Value);
            AnsiConsole.MarkupLine("[green]✅ Aeropuerto actualizado.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private async Task<Airport?> PromptSelectAirportForEditAsync()
    {
        var airports = (await _service.GetAllAsync())
            .OrderBy(a => a.Name.Value)
            .ThenBy(a => a.IataCode.Value)
            .ToList();

        if (airports.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay aeropuertos registrados.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione el aeropuerto a modificar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(airports.Select(FormatAirportChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return airports.First(a => FormatAirportChoice(a) == selected);
    }

    private async Task DeleteMenuAsync()
    {
        var airports = (await _service.GetAllAsync())
            .OrderBy(a => a.Name.Value)
            .ThenBy(a => a.IataCode.Value)
            .ToList();

        if (airports.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay aeropuertos registrados.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red]Seleccione el aeropuerto a eliminar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(airports.Select(FormatAirportChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var airport = airports.First(a => FormatAirportChoice(a) == selected);

        try
        {
            if (!AnsiConsole.Confirm($"[red]¿Está seguro de eliminar el aeropuerto {Markup.Escape(airport.Name.Value)} ({Markup.Escape(airport.IataCode.Value)})?[/]"))
            {
                Pause();
                return;
            }

            await _service.DeleteByIdAsync(airport.Id!.Value);

            AnsiConsole.MarkupLine("[green]✅ Operación procesada.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private static void ShowSearchResults(IEnumerable<Airport> items, string title)
    {
        var list = items.ToList();
        if (list.Count == 0)
            AnsiConsole.MarkupLine("[red]❌ No se encontraron registros.[/]");
        else
            ShowTable(list, title);

        Pause();
    }

    private static void ShowTable(IEnumerable<Airport> items, string title)
    {
        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .AddColumn("[yellow]ID[/]")
            .AddColumn("[green]Nombre[/]")
            .AddColumn("[blue]IATA[/]")
            .AddColumn("[blue]ICAO[/]")
            .AddColumn("[green]Ciudad ID[/]");

        foreach (var item in items)
        {
            table.AddRow(
                (item.Id?.Value ?? 0).ToString(),
                item.Name.Value,
                item.IataCode.Value,
                item.IcaoCode.Value ?? "-",
                item.CityId.Value.ToString());
        }

        AnsiConsole.Write(table);
    }

    private static string? PromptIataCodeOrBack(string label, string? defaultValue = null)
    {
        return ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            label,
            defaultValue ?? string.Empty,
            allowEmpty: false,
            validate: value =>
            {
                var text = value?.Trim().ToUpperInvariant() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(text))
                    return "El código IATA es obligatorio.";
                if (text.Length != 3)
                    return "El código IATA debe tener exactamente 3 letras.";
                if (!text.All(char.IsLetter))
                    return "El código IATA solo puede contener letras.";
                return null;
            })?.ToUpperInvariant();
    }

    private static string? PromptIcaoCodeOrBack(string label, bool allowEmpty, string? defaultValue = null)
    {
        return ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            label,
            defaultValue ?? string.Empty,
            allowEmpty: allowEmpty,
            validate: value =>
            {
                var text = value?.Trim().ToUpperInvariant() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(text))
                    return allowEmpty ? null : "El código ICAO es obligatorio.";
                if (text.Length != 4)
                    return "El código ICAO debe tener exactamente 4 letras.";
                if (!text.All(char.IsLetter))
                    return "El código ICAO solo puede contener letras.";
                return null;
            })?.ToUpperInvariant();
    }

    private static string FormatAirportChoice(Airport airport) =>
        $"{airport.Id?.Value ?? 0} · {Markup.Escape(airport.Name.Value)} · {Markup.Escape(airport.IataCode.Value)}";

    private static void Pause()
    {
        AnsiConsole.MarkupLine("\nPresione [grey]ENTER[/] para continuar...");
        Console.ReadLine();
    }
}
