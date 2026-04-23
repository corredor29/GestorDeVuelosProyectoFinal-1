using System.Linq;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.Moduls.Routes.ui;

public sealed class RoutesMenu : IModuleUI
{
    private readonly IRoutesService _service;
    private readonly IAirportsService _airports;

    public string Key => "routes";
    public string Title => "Gestión de Rutas";

    public RoutesMenu(IRoutesService service, IAirportsService airports)
    {
        _service = service;
        _airports = airports;
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
        // Este menú se apoya bastante en listas porque una ruta siempre depende de dos aeropuertos reales.
        while (!ct.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule($"[yellow]{Title}[/]").RuleStyle("grey").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Seleccione una opción:")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "1. Listar todas las rutas",
                        "2. Buscar por ID",
                        "3. Buscar por aeropuerto origen",
                        "4. Buscar por aeropuerto destino",
                        "5. Buscar por origen y destino",
                        "6. Registrar ruta",
                        "7. Actualizar ruta",
                        "8. Eliminar ruta por ID",
                        "0. Volver al menú principal"
                    }));

            if (option.StartsWith("0"))
                break;

            switch (option.Split('.')[0])
            {
                case "1": await ListAllAsync(); break;
                case "2": await SearchByIdAsync(); break;
                case "3": await SearchByOriginAirportIdAsync(); break;
                case "4": await SearchByDestinationAirportIdAsync(); break;
                case "5": await SearchByOriginAndDestinationAsync(); break;
                case "6": await CreateAsync(); break;
                case "7": await UpdateAsync(); break;
                case "8": await DeleteByIdAsync(); break;
            }
        }
    }

    private async Task ListAllAsync()
    {
        var items = await _service.GetAllAsync();
        ShowTable(items, "Todas las rutas");
        Pause();
    }

    private async Task SearchByIdAsync()
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("Ingrese el [green]ID[/] de la ruta:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value);

        if (item is null)
            AnsiConsole.MarkupLine("[red]No se encontró ninguna ruta con ese ID.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para ID: {id.Value}");

        Pause();
    }

    private async Task SearchByOriginAirportIdAsync()
    {
        var airportId = await PromptAirportSelectionAsync("origen");
        if (airportId is null)
            return;

        var items = await _service.GetByOriginAirportIdAsync(airportId.Value);
        ShowSearchResults(items, $"Rutas con origen #{airportId.Value}");
    }

    private async Task SearchByDestinationAirportIdAsync()
    {
        var airportId = await PromptAirportSelectionAsync("destino");
        if (airportId is null)
        {
            Pause();
            return;
        }

        var items = await _service.GetByDestinationAirportIdAsync(airportId.Value);
        ShowSearchResults(items, $"Rutas con destino #{airportId.Value}");
    }

    private async Task SearchByOriginAndDestinationAsync()
    {
        var originAirportId = await PromptAirportSelectionAsync("origen");
        if (originAirportId is null)
        {
            Pause();
            return;
        }

        var destinationAirportId = await PromptAirportSelectionAsync("destino");
        if (destinationAirportId is null)
        {
            Pause();
            return;
        }

        var item = await _service.GetByOriginAndDestinationAsync(originAirportId.Value, destinationAirportId.Value);

        if (item is null)
            AnsiConsole.MarkupLine("[red]No se encontró ninguna ruta para esa combinación.[/]");
        else
            ShowTable(new[] { item }, $"Ruta {originAirportId.Value} -> {destinationAirportId.Value}");

        Pause();
    }

    private async Task<int?> PromptAirportSelectionAsync(string tipo, int? currentAirportId = null)
    {
        // Se reutiliza el mismo selector para origen y destino, cambiando solo el contexto visual.
        var airports = (await _airports.GetAllAsync())
            .OrderBy(a => a.Name.Value)
            .ToList();

        if (airports.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay aeropuertos registrados.[/]");
            return null;
        }

        var title = currentAirportId is null
            ? $"Seleccione el aeropuerto de [green]{tipo}[/]:"
            : $"Seleccione el aeropuerto de [green]{tipo}[/] [dim](actual id: {currentAirportId})[/]:";

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(15)
                .AddChoices(airports.Select(FormatAirportChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return airports.First(a => FormatAirportChoice(a) == selected).Id!.Value;
    }

    private async Task CreateAsync()
    {
        try
        {
            // El flujo obliga a elegir ambos aeropuertos antes de pedir datos opcionales de la ruta.
            AnsiConsole.MarkupLine("[bold blue]Registrar ruta[/]");
            var originAirportId = await PromptAirportSelectionAsync("origen");
            if (originAirportId is null)
            {
                Pause();
                return;
            }

            var destinationAirportId = await PromptAirportSelectionAsync("destino");
            if (destinationAirportId is null)
            {
                Pause();
                return;
            }

            var distanceKm = PromptOptionalIntOrBack("Distancia en KM (opcional):");
            if (distanceKm.WentBack)
            {
                Pause();
                return;
            }

            var estimatedDurationMin = PromptOptionalIntOrBack("Duración estimada en minutos (opcional):");
            if (estimatedDurationMin.WentBack)
            {
                Pause();
                return;
            }

            if (AnsiConsole.Confirm("Desea guardar los cambios?"))
            {
                await _service.CreateAsync(originAirportId.Value, destinationAirportId.Value, distanceKm.Value, estimatedDurationMin.Value);
                AnsiConsole.MarkupLine("[green]Ruta registrada correctamente.[/]");
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }

        Pause();
    }

    private async Task UpdateAsync()
    {
        var routes = (await _service.GetAllAsync()).ToList();
        if (routes.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay rutas registradas.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione la ruta a modificar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(routes.Select(FormatRouteChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var item = routes.First(r => FormatRouteChoice(r) == selected);
        var id = item.Id!.Value;

        try
        {
            var originAirportId = await PromptAirportSelectionAsync("origen", item.OriginAirportId.Value);
            if (originAirportId is null)
            {
                Pause();
                return;
            }

            var destinationAirportId = await PromptAirportSelectionAsync("destino", item.DestinationAirportId.Value);
            if (destinationAirportId is null)
            {
                Pause();
                return;
            }

            var distanceKm = PromptOptionalIntOrBack("Nueva distancia en KM (opcional):", item.Distance.Value);
            if (distanceKm.WentBack)
            {
                Pause();
                return;
            }

            var estimatedDurationMin = PromptOptionalIntOrBack("Nueva duración estimada en minutos (opcional):", item.Duration.Value);
            if (estimatedDurationMin.WentBack)
            {
                Pause();
                return;
            }

            await _service.UpdateAsync(id, originAirportId.Value, destinationAirportId.Value, distanceKm.Value, estimatedDurationMin.Value);
            AnsiConsole.MarkupLine("[green]Ruta actualizada.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }

        Pause();
    }

    private async Task DeleteByIdAsync()
    {
        try
        {
            var routes = (await _service.GetAllAsync()).ToList();
            if (routes.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No hay rutas registradas.[/]");
                Pause();
                return;
            }

            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]Seleccione la ruta a eliminar:[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .PageSize(12)
                    .AddChoices(routes.Select(FormatRouteChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

            if (selected == ConsoleMenuHelpers.VolverAlMenu)
                return;

            var route = routes.First(r => FormatRouteChoice(r) == selected);
            if (AnsiConsole.Confirm("¿Estás seguro?"))
            {
                await _service.DeleteByIdAsync(route.Id!.Value);
                AnsiConsole.MarkupLine("[green]Ruta eliminada.[/]");
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }

        Pause();
    }

    private static void ShowSearchResults(IEnumerable<Route> items, string title)
    {
        var list = items.ToList();
        if (list.Count == 0)
            AnsiConsole.MarkupLine("[red]No se encontraron registros.[/]");
        else
            ShowTable(list, title);

        Pause();
    }

    private static void ShowTable(IEnumerable<Route> items, string title)
    {
        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .AddColumn("[yellow]ID[/]")
            .AddColumn("[green]Origen[/]")
            .AddColumn("[blue]Destino[/]")
            .AddColumn("[green]Distancia KM[/]")
            .AddColumn("[blue]Duracion Min[/]");

        foreach (var item in items)
        {
            table.AddRow(
                (item.Id?.Value ?? 0).ToString(),
                item.OriginAirportId.Value.ToString(),
                item.DestinationAirportId.Value.ToString(),
                item.Distance.Value?.ToString() ?? "-",
                item.Duration.Value?.ToString() ?? "-");
        }

        AnsiConsole.Write(table);
    }

    private static (bool WentBack, int? Value) PromptOptionalIntOrBack(string prompt, int? currentValue = null)
    {
        var raw = ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            prompt,
            currentValue?.ToString() ?? string.Empty,
            allowEmpty: true,
            validate: value => string.IsNullOrWhiteSpace(value) || int.TryParse(value.Trim(), out _)
                ? null
                : "Debe ingresar un número entero válido o dejar el valor vacío.");

        if (raw is null)
            return (true, null);

        if (string.IsNullOrWhiteSpace(raw))
            return (false, null);

        return (false, int.Parse(raw.Trim()));
    }

    private static string FormatAirportChoice(Airport airport) =>
        $"{airport.Id!.Value} · {Markup.Escape(airport.Name.Value)} · {Markup.Escape(airport.IataCode.Value)}";

    private static string FormatRouteChoice(Route route) =>
        $"{route.Id?.Value ?? 0} · {route.OriginAirportId.Value} -> {route.DestinationAirportId.Value}";

    private static void Pause()
    {
        AnsiConsole.MarkupLine("\nPresione [grey]ENTER[/] para continuar...");
        Console.ReadLine();
    }
}
