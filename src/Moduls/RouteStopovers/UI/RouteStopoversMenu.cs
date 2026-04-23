// Consola interactiva: Spectre.Console (https://spectreconsole.net/)
using System.Linq;
using Spectre.Console;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.UI;

public sealed class RouteStopoversMenu : IModuleUI
{
    private readonly IRouteStopoversService _service;
    private readonly IRoutesService _routes;
    private readonly IAirportsService _airports;

    public RouteStopoversMenu(IRouteStopoversService service, IRoutesService routes, IAirportsService airports)
    {
        _service = service;
        _routes = routes;
        _airports = airports;
    }

    public string Key => "route-stopovers";
    public string Title => "Escalas de ruta";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Escalas de ruta [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar[/]")
                    .PageSize(10)
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Listar todas",
                        "Buscar por ID",
                        "Buscar por ruta",
                        "Crear escala",
                        "Actualizar escala",
                        "Eliminar por ID",
                        "Eliminar por ruta",
                        "Volver"));

            switch (option)
            {
                case "Listar todas":
                    await ListAllAsync(cancellationToken);
                    break;
                case "Buscar por ID":
                    await SearchByIdAsync(cancellationToken);
                    break;
                case "Buscar por ruta":
                    await SearchByRouteAsync(cancellationToken);
                    break;
                case "Crear escala":
                    await CreateAsync(cancellationToken);
                    break;
                case "Actualizar escala":
                    await UpdateAsync(cancellationToken);
                    break;
                case "Eliminar por ID":
                    await DeleteByIdAsync(cancellationToken);
                    break;
                case "Eliminar por ruta":
                    await DeleteByRouteAsync(cancellationToken);
                    break;
                case "Volver":
                    return;
            }
        }
    }

    private async Task ListAllAsync(CancellationToken cancellationToken)
    {
        await RenderTableAsync(await _service.GetAllAsync(cancellationToken), "Todas las escalas", cancellationToken);
        Pause();
    }

    private async Task SearchByIdAsync(CancellationToken cancellationToken)
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID de la escala:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value, cancellationToken);
        await RenderTableAsync(item is null ? Array.Empty<RouteStopover>() : new[] { item }, $"Resultado para ID {id.Value}", cancellationToken);
        Pause();
    }

    private async Task SearchByRouteAsync(CancellationToken cancellationToken)
    {
        var routeId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID de la ruta:");
        if (routeId is null)
            return;

        await RenderTableAsync(await _service.GetByRouteIdAsync(routeId.Value, cancellationToken), $"Escalas de la ruta {routeId.Value}", cancellationToken);
        Pause();
    }

    private async Task CreateAsync(CancellationToken cancellationToken)
    {
        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Registrar escala de ruta"))
            return;

        var routeId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID de la ruta:");
        if (routeId is null)
            return;

        var stopoverAirportId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID aeropuerto de escala:");
        if (stopoverAirportId is null)
            return;

        var stopOrder = ConsoleMenuHelpers.PromptPositiveIntOrBack("Orden de escala:");
        if (stopOrder is null)
            return;

        var layoverMin = PromptNonNegativeIntOrBack("Layover en minutos:");
        if (layoverMin is null)
            return;

        try
        {
            await _service.CreateAsync(routeId.Value, stopoverAirportId.Value, stopOrder.Value, layoverMin.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Escala creada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task UpdateAsync(CancellationToken cancellationToken)
    {
        var current = await PromptStopoverSelectionAsync("Seleccione la escala a actualizar:", cancellationToken);
        if (current is null)
            return;

        var routeId = ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("ID de la ruta:", current.RouteId.Value);
        if (routeId is null)
            return;

        var stopoverAirportId = ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("ID aeropuerto de escala:", current.StopoverAirportId.Value);
        if (stopoverAirportId is null)
            return;

        var stopOrder = ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("Orden de escala:", current.StopOrder.Value);
        if (stopOrder is null)
            return;

        var layoverMin = PromptNonNegativeIntOrBack("Layover en minutos:", current.LayoverMin.Value);
        if (layoverMin is null)
            return;

        try
        {
            await _service.UpdateAsync(current.Id!.Value, routeId.Value, stopoverAirportId.Value, stopOrder.Value, layoverMin.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Escala actualizada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteByIdAsync(CancellationToken cancellationToken)
    {
        var current = await PromptStopoverSelectionAsync("Seleccione la escala a eliminar:", cancellationToken);
        if (current is null)
            return;

        if (!AnsiConsole.Confirm("¿Confirmas la eliminación?", false))
        {
            Pause();
            return;
        }

        try
        {
            await _service.DeleteByIdAsync(current.Id!.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Escala eliminada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteByRouteAsync(CancellationToken cancellationToken)
    {
        var routeId = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID de la ruta (se eliminan todas sus escalas):");
        if (routeId is null)
            return;

        if (!AnsiConsole.Confirm("¿Confirmas la eliminación?", false))
        {
            Pause();
            return;
        }

        try
        {
            var n = await _service.DeleteByRouteIdAsync(routeId.Value, cancellationToken);
            AnsiConsole.MarkupLine($"\n[green]Eliminadas {n} escala(s).[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task RenderTableAsync(IEnumerable<RouteStopover> items, string title, CancellationToken cancellationToken)
    {
        var list = items.ToList();
        if (list.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay registros para mostrar.[/]");
            return;
        }

        var routes = (await _routes.GetAllAsync())
            .Where(r => r.Id != null)
            .ToDictionary(
                r => r.Id!.Value,
                r => $"#{r.Id!.Value} {r.OriginAirportId.Value}->{r.DestinationAirportId.Value}");

        var airportNames = (await _airports.GetAllAsync())
            .Where(a => a.Id != null)
            .ToDictionary(a => a.Id!.Value, a => a.Name.Value);

        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold grey]ID[/]")
            .AddColumn("[bold grey]Ruta[/]")
            .AddColumn("[bold grey]Aeropuerto escala[/]")
            .AddColumn("[bold grey]Orden[/]")
            .AddColumn("[bold grey]Espera (min)[/]");

        foreach (var item in list)
        {
            var routeLabel = routes.TryGetValue(item.RouteId.Value, out var rlabel)
                ? $"{item.RouteId.Value} · {Markup.Escape(rlabel)}"
                : item.RouteId.Value.ToString();

            var apLabel = airportNames.TryGetValue(item.StopoverAirportId.Value, out var an)
                ? $"{item.StopoverAirportId.Value} · {Markup.Escape(an)}"
                : item.StopoverAirportId.Value.ToString();

            table.AddRow(
                item.Id?.Value.ToString() ?? "-",
                routeLabel,
                apLabel,
                item.StopOrder.Value.ToString(),
                item.LayoverMin.Value.ToString());
        }

        AnsiConsole.Write(table);
    }

    private static int? PromptNonNegativeIntOrBack(string label, int? current = null)
    {
        while (true)
        {
            var value = ConsoleMenuHelpers.PromptStringWithInitialOrBack(label, current?.ToString() ?? "0", allowEmpty: false);
            if (value is null)
                return null;

            if (!int.TryParse(value.Trim(), out var parsed))
            {
                AnsiConsole.MarkupLine("[red]Valor entero inválido.[/]");
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

    private async Task<RouteStopover?> PromptStopoverSelectionAsync(string title, CancellationToken cancellationToken)
    {
        var stopovers = (await _service.GetAllAsync(cancellationToken))
            .OrderBy(x => x.RouteId.Value)
            .ThenBy(x => x.StopOrder.Value)
            .ToList();
        if (stopovers.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay escalas registradas.[/]");
            Pause();
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(stopovers.Select(FormatStopoverChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return stopovers.First(x => FormatStopoverChoice(x) == selected);
    }

    private static string FormatStopoverChoice(RouteStopover item) =>
        $"{item.Id?.Value ?? 0} · Ruta {item.RouteId.Value} · Escala {item.StopoverAirportId.Value} · Orden {item.StopOrder.Value}";

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Presiona Enter para continuar...[/]").AllowEmpty());
    }
}
