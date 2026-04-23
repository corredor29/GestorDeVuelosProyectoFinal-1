using System.Linq;
using Spectre.Console;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Rates.UI;

public sealed class RatesMenu : IModuleUI
{
    private readonly IRatesService _service;
    private readonly IRoutesService _routes;
    private readonly ICabinTypeService _cabinTypes;
    private readonly IPassengerTypesService _passengerTypes;
    private readonly ISeasonsService _seasons;

    public RatesMenu(
        IRatesService service,
        IRoutesService routes,
        ICabinTypeService cabinTypes,
        IPassengerTypesService passengerTypes,
        ISeasonsService seasons)
    {
        _service = service;
        _routes = routes;
        _cabinTypes = cabinTypes;
        _passengerTypes = passengerTypes;
        _seasons = seasons;
    }

    public string Key => "fares";
    public string Title => "Tarifas";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Tarifas [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar[/]")
                    .PageSize(12)
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Listar todas",
                        "Buscar por ID",
                        "Filtrar por route_id",
                        "Filtrar por cabin_type_id",
                        "Crear tarifa",
                        "Actualizar tarifa",
                        "Eliminar por ID",
                        "Calcular precio final",
                        "Volver"));

            switch (option)
            {
                case "Listar todas":
                    await ListAllAsync();
                    break;
                case "Buscar por ID":
                    await SearchByIdAsync();
                    break;
                case "Filtrar por route_id":
                    await SearchByRouteIdAsync();
                    break;
                case "Filtrar por cabin_type_id":
                    await SearchByCabinTypeIdAsync();
                    break;
                case "Crear tarifa":
                    await CreateAsync();
                    break;
                case "Actualizar tarifa":
                    await UpdateAsync();
                    break;
                case "Eliminar por ID":
                    await DeleteByIdAsync();
                    break;
                case "Calcular precio final":
                    await CalculatePriceAsync();
                    break;
                case "Volver":
                    return;
            }
        }
    }

    private async Task ListAllAsync()
    {
        await RenderTableAsync(await _service.GetAllAsync(), "Todas las tarifas");
        Pause();
    }

    private async Task SearchByIdAsync()
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value);
        await RenderTableAsync(item is null ? Array.Empty<Rate>() : new[] { item }, $"ID {id.Value}");
        Pause();
    }

    private async Task SearchByRouteIdAsync()
    {
        var routeId = await PromptRouteSelectionAsync("Seleccione la ruta a consultar:");
        if (routeId is null)
            return;

        await RenderTableAsync(await _service.GetByRouteIdAsync(routeId.Value), $"Tarifas de la ruta {routeId.Value}");
        Pause();
    }

    private async Task SearchByCabinTypeIdAsync()
    {
        var cabinTypeId = await PromptCabinTypeSelectionAsync("Seleccione el tipo de cabina a consultar:");
        if (cabinTypeId is null)
            return;

        await RenderTableAsync(await _service.GetByCabinTypeIdAsync(cabinTypeId.Value), $"Tarifas del tipo de cabina {cabinTypeId.Value}");
        Pause();
    }

    private async Task CreateAsync()
    {
        try
        {
            if (!ConsoleMenuHelpers.TryBeginFormOrBack("Registrar tarifa"))
                return;

            var values = await PromptRateValuesAsync();
            if (values is null)
                return;

            await _service.CreateAsync(values.Value.routeId, values.Value.cabinTypeId, values.Value.passengerTypeId, values.Value.seasonId, values.Value.basePrice, values.Value.validFrom, values.Value.validUntil);
            AnsiConsole.MarkupLine("\n[green]Tarifa creada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task UpdateAsync()
    {
        var current = await PromptRateSelectionAsync("Seleccione la tarifa a actualizar:");
        if (current is null)
        {
            return;
        }

        try
        {
            var values = await PromptRateValuesAsync(
                current.RouteId.Value,
                current.CabinTypeId.Value,
                current.PassengerTypeId.Value,
                current.SeasonId.Value,
                current.BasePrice.Value,
                current.ValidFrom.Value,
                current.ValidUntil.Value);
            if (values is null)
                return;

            await _service.UpdateAsync(current.Id!.Value, values.Value.routeId, values.Value.cabinTypeId, values.Value.passengerTypeId, values.Value.seasonId, values.Value.basePrice, values.Value.validFrom, values.Value.validUntil);
            AnsiConsole.MarkupLine("\n[green]Tarifa actualizada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteByIdAsync()
    {
        var rate = await PromptRateSelectionAsync("Seleccione la tarifa a eliminar:");
        if (rate is null)
            return;

        if (!AnsiConsole.Confirm("¿Confirmar eliminación?", false))
        {
            Pause();
            return;
        }

        try
        {
            await _service.DeleteByIdAsync(rate.Id!.Value);
            AnsiConsole.MarkupLine("\n[green]Tarifa eliminada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task CalculatePriceAsync()
    {
        var rate = await PromptRateSelectionAsync("Seleccione la tarifa para calcular el precio:");
        if (rate is null)
            return;

        try
        {
            var total = await _service.CalculatePriceAsync(rate.Id!.Value);
            AnsiConsole.MarkupLine($"\n[green]Precio calculado:[/] {total:0.00}");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task RenderTableAsync(IEnumerable<Rate> items, string title)
    {
        var list = items.ToList();
        if (list.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay registros para mostrar.[/]");
            return;
        }

        var routeLabels = (await _routes.GetAllAsync())
            .Where(r => r.Id != null)
            .ToDictionary(
                r => r.Id!.Value,
                r => $"{r.Id!.Value} · {r.OriginAirportId.Value}->{r.DestinationAirportId.Value}");

        var cabinLabels = (await _cabinTypes.GetAllAsync())
            .ToDictionary(c => c.Id.Value, c => $"{c.Id.Value} · {c.Name.Value}");

        var passengerLabels = (await _passengerTypes.GetAllAsync())
            .Where(p => p.Id != null)
            .ToDictionary(p => p.Id!.Value, p => $"{p.Id!.Value} · {p.Name.Value}");

        var seasonLabels = (await _seasons.GetAllAsync())
            .Where(s => s.Id != null)
            .ToDictionary(s => s.Id!.Value, s => $"{s.Id!.Value} · {s.Name.Value}");

        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold grey]ID[/]")
            .AddColumn("[bold grey]Ruta[/]")
            .AddColumn("[bold grey]Cabina[/]")
            .AddColumn("[bold grey]Tipo pasajero[/]")
            .AddColumn("[bold grey]Temporada[/]")
            .AddColumn("[bold grey]Precio base[/]")
            .AddColumn("[bold grey]Válida desde[/]")
            .AddColumn("[bold grey]Válida hasta[/]");

        foreach (var item in list)
        {
            var route = routeLabels.TryGetValue(item.RouteId.Value, out var rl)
                ? Markup.Escape(rl)
                : item.RouteId.Value.ToString();
            var cabin = cabinLabels.TryGetValue(item.CabinTypeId.Value, out var cl)
                ? Markup.Escape(cl)
                : item.CabinTypeId.Value.ToString();
            var passenger = passengerLabels.TryGetValue(item.PassengerTypeId.Value, out var pl)
                ? Markup.Escape(pl)
                : item.PassengerTypeId.Value.ToString();
            var season = seasonLabels.TryGetValue(item.SeasonId.Value, out var sl)
                ? Markup.Escape(sl)
                : item.SeasonId.Value.ToString();

            table.AddRow(
                item.Id?.Value.ToString() ?? "-",
                route,
                cabin,
                passenger,
                season,
                item.BasePrice.Value.ToString("0.00"),
                item.ValidFrom.Value?.ToString("yyyy-MM-dd") ?? "-",
                item.ValidUntil.Value?.ToString("yyyy-MM-dd") ?? "-");
        }

        AnsiConsole.Write(table);
    }

    private async Task<(int routeId, int cabinTypeId, int passengerTypeId, int seasonId, decimal basePrice, DateOnly? validFrom, DateOnly? validUntil)?> PromptRateValuesAsync(
        int? currentRouteId = null,
        int? currentCabinTypeId = null,
        int? currentPassengerTypeId = null,
        int? currentSeasonId = null,
        decimal? currentBasePrice = null,
        DateOnly? currentValidFrom = null,
        DateOnly? currentValidUntil = null)
    {
        var routeId = await PromptRouteSelectionAsync("Seleccione la ruta:", currentRouteId);
        if (routeId is null)
            return null;

        var cabinTypeId = await PromptCabinTypeSelectionAsync("Seleccione el tipo de cabina:", currentCabinTypeId);
        if (cabinTypeId is null)
            return null;

        var passengerTypeId = await PromptPassengerTypeSelectionAsync("Seleccione el tipo de pasajero:", currentPassengerTypeId);
        if (passengerTypeId is null)
            return null;

        var seasonId = await PromptSeasonSelectionAsync("Seleccione la temporada:", currentSeasonId);
        if (seasonId is null)
            return null;

        var basePrice = PromptNonNegativeDecimalOrBack("Precio base:", currentBasePrice);
        if (basePrice is null)
            return null;

        var validFrom = PromptOptionalDateOrBack("Válida desde (yyyy-MM-dd, vacío = omitir):", currentValidFrom);
        if (validFrom.WentBack)
            return null;

        var validUntil = PromptOptionalDateOrBack("Válida hasta (yyyy-MM-dd, vacío = omitir):", currentValidUntil);
        if (validUntil.WentBack)
            return null;

        return (routeId.Value, cabinTypeId.Value, passengerTypeId.Value, seasonId.Value, basePrice.Value, validFrom.Value, validUntil.Value);
    }

    private async Task<int?> PromptRouteSelectionAsync(string title, int? currentRouteId = null)
    {
        var routes = (await _routes.GetAllAsync())
            .Where(r => r.Id != null)
            .OrderByDescending(r => r.Id!.Value == currentRouteId)
            .ThenBy(r => r.Id!.Value)
            .ToList();
        if (routes.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay rutas registradas.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(routes.Select(FormatRouteChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return routes.First(r => FormatRouteChoice(r) == selected).Id!.Value;
    }

    private async Task<int?> PromptCabinTypeSelectionAsync(string title, int? currentCabinTypeId = null)
    {
        var cabinTypes = (await _cabinTypes.GetAllAsync())
            .OrderByDescending(c => c.Id.Value == currentCabinTypeId)
            .ThenBy(c => c.Name.Value)
            .ToList();
        if (cabinTypes.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay tipos de cabina registrados.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(cabinTypes.Select(FormatCabinTypeChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return cabinTypes.First(c => FormatCabinTypeChoice(c) == selected).Id.Value;
    }

    private async Task<int?> PromptPassengerTypeSelectionAsync(string title, int? currentPassengerTypeId = null)
    {
        var passengerTypes = (await _passengerTypes.GetAllAsync())
            .Where(p => p.Id != null)
            .OrderByDescending(p => p.Id!.Value == currentPassengerTypeId)
            .ThenBy(p => p.Name.Value)
            .ToList();
        if (passengerTypes.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay tipos de pasajero registrados.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(passengerTypes.Select(FormatPassengerTypeChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return passengerTypes.First(p => FormatPassengerTypeChoice(p) == selected).Id!.Value;
    }

    private async Task<int?> PromptSeasonSelectionAsync(string title, int? currentSeasonId = null)
    {
        var seasons = (await _seasons.GetAllAsync())
            .Where(s => s.Id != null)
            .OrderByDescending(s => s.Id!.Value == currentSeasonId)
            .ThenBy(s => s.Name.Value)
            .ToList();
        if (seasons.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay temporadas registradas.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(seasons.Select(FormatSeasonChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return seasons.First(s => FormatSeasonChoice(s) == selected).Id!.Value;
    }

    private async Task<Rate?> PromptRateSelectionAsync(string title)
    {
        var rates = (await _service.GetAllAsync())
            .ToList();
        if (rates.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay tarifas registradas.[/]");
            Pause();
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(rates.Select(FormatRateChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return rates.First(r => FormatRateChoice(r) == selected);
    }

    private static decimal? PromptNonNegativeDecimalOrBack(string label, decimal? current = null)
    {
        while (true)
        {
            var value = ConsoleMenuHelpers.PromptStringWithInitialOrBack(label, current?.ToString("0.##") ?? string.Empty, allowEmpty: false);
            if (value is null)
                return null;

            if (!decimal.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var parsed)
                && !decimal.TryParse(value, out parsed))
            {
                AnsiConsole.MarkupLine("[red]Número decimal no válido.[/]");
                continue;
            }

            if (parsed < 0)
            {
                AnsiConsole.MarkupLine("[red]No puede ser negativo.[/]");
                continue;
            }

            return parsed;
        }
    }

    private static (bool WentBack, DateOnly? Value) PromptOptionalDateOrBack(string label, DateOnly? current = null)
    {
        while (true)
        {
            var text = ConsoleMenuHelpers.PromptStringWithInitialOrBack(label, current?.ToString("yyyy-MM-dd") ?? string.Empty, allowEmpty: true);
            if (text is null)
                return (true, null);

            if (string.IsNullOrWhiteSpace(text))
                return (false, null);

            if (DateOnly.TryParse(text.Trim(), out var value))
                return (false, value);

            AnsiConsole.MarkupLine("[red]Fecha inválida. Usa el formato yyyy-MM-dd.[/]");
        }
    }

    private static string FormatRouteChoice(dynamic route) =>
        $"{route.Id!.Value} · {route.OriginAirportId.Value}->{route.DestinationAirportId.Value}";

    private static string FormatCabinTypeChoice(dynamic cabinType) =>
        $"{cabinType.Id.Value} · {Markup.Escape(cabinType.Name.Value)}";

    private static string FormatPassengerTypeChoice(dynamic passengerType) =>
        $"{passengerType.Id!.Value} · {Markup.Escape(passengerType.Name.Value)}";

    private static string FormatSeasonChoice(dynamic season) =>
        $"{season.Id!.Value} · {Markup.Escape(season.Name.Value)}";

    private static string FormatRateChoice(Rate rate) =>
        $"{rate.Id?.Value ?? 0} · Ruta {rate.RouteId.Value} · Cabina {rate.CabinTypeId.Value} · {rate.BasePrice.Value:0.00}";

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Presiona Enter para continuar...[/]").AllowEmpty());
    }
}
