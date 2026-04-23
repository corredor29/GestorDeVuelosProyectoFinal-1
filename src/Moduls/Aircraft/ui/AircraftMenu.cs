using System.Linq;
using Spectre.Console;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using AircraftAggregate = GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Aggregate.Aircraft;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.ui;

public sealed class AircraftMenu : IModuleUI
{
    private readonly IAircraftService _service;
    private readonly IAircraftModelsService _models;
    private readonly IAirlinesService _airlines;

    public AircraftMenu(IAircraftService service, IAircraftModelsService models, IAirlinesService airlines)
    {
        _service = service;
        _models = models;
        _airlines = airlines;
    }

    public string Key => "aircraft";
    public string Title => "Aeronaves";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Aeronaves [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Listar todas",
                        "Buscar por ID",
                        "Buscar por matrícula",
                        "Buscar por aerolínea",
                        "Crear aeronave",
                        "Actualizar aeronave",
                        "Desactivar aeronave",
                        "Volver"));

            switch (option)
            {
                case "Listar todas": await ListAllAsync(cancellationToken); break;
                case "Buscar por ID": await SearchByIdAsync(cancellationToken); break;
                case "Buscar por matrícula": await SearchByRegistrationAsync(cancellationToken); break;
                case "Buscar por aerolínea": await SearchByAirlineAsync(cancellationToken); break;
                case "Crear aeronave": await CreateAsync(cancellationToken); break;
                case "Actualizar aeronave": await UpdateAsync(cancellationToken); break;
                case "Desactivar aeronave": await DeactivateAsync(cancellationToken); break;
                case "Volver": return;
            }
        }
    }

    private async Task ListAllAsync(CancellationToken cancellationToken)
    {
        await RenderTableAsync(await _service.GetAllAsync(cancellationToken), "Todas las aeronaves", cancellationToken);
        Pause();
    }

    private async Task SearchByIdAsync(CancellationToken cancellationToken)
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID de la aeronave:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value, cancellationToken);
        await RenderTableAsync(item is null ? Array.Empty<AircraftAggregate>() : new[] { item }, $"Resultado para ID {id.Value}", cancellationToken);
        Pause();
    }

    private async Task SearchByRegistrationAsync(CancellationToken cancellationToken)
    {
        var raw = AnsiConsole.Prompt(
            new TextPrompt<string>("Matrícula [dim](vacío = cancelar)[/]:")
                .AllowEmpty());

        var registration = raw.Trim();
        if (string.IsNullOrEmpty(registration))
        {
            AnsiConsole.MarkupLine("[yellow]Búsqueda cancelada: indique una matrícula para buscar.[/]");
            Pause();
            return;
        }

        try
        {
            var item = await _service.GetByRegistrationAsync(registration, cancellationToken);
            await RenderTableAsync(item is null ? Array.Empty<AircraftAggregate>() : new[] { item }, $"Resultado para {registration}", cancellationToken);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task SearchByAirlineAsync(CancellationToken cancellationToken)
    {
        var mode = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Buscar aeronaves por aerolínea:")
                .AddChoices("Por ID", "Por nombre (o parte del nombre)"));

        int? airlineId = null;

        if (mode == "Por ID")
        {
            var idText = AnsiConsole.Prompt(
                new TextPrompt<string>("ID de la aerolínea [dim](vacío = cancelar)[/]:")
                    .AllowEmpty());
            if (string.IsNullOrWhiteSpace(idText) || !int.TryParse(idText.Trim(), out var id) || id <= 0)
            {
                AnsiConsole.MarkupLine("[yellow]ID no válido o vacío.[/]");
                Pause();
                return;
            }

            airlineId = id;
        }
        else
        {
            var hint = AnsiConsole.Prompt(
                new TextPrompt<string>("Nombre o parte del nombre de la aerolínea [dim](vacío = cancelar)[/]:")
                    .AllowEmpty());
            var needle = hint.Trim();
            if (string.IsNullOrEmpty(needle))
            {
                AnsiConsole.MarkupLine("[yellow]Búsqueda cancelada.[/]");
                Pause();
                return;
            }

            var exact = await _airlines.GetByNameAsync(needle);
            if (exact is not null)
                airlineId = exact.Id!.Value;
            else
            {
                var matches = (await _airlines.GetAllAsync())
                    .Where(a => a.Name.Value.Contains(needle, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(a => a.Name.Value)
                    .ToList();

                if (matches.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]No hay aerolíneas que coincidan.[/]");
                    Pause();
                    return;
                }

                if (matches.Count == 1)
                    airlineId = matches[0].Id!.Value;
                else
                {
                    var labels = matches
                        .Select(a => $"{a.Id!.Value} · {Markup.Escape(a.Name.Value)} ({Markup.Escape(a.IataCode.Value)})")
                        .ToList();
                    var sel = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Varias aerolíneas coinciden. Elija una:")
                            .PageSize(15)
                            .AddChoices(labels));
                    airlineId = matches[labels.IndexOf(sel)].Id!.Value;
                }
            }
        }

        if (airlineId is null)
        {
            Pause();
            return;
        }

        await RenderTableAsync(
            await _service.GetByAirlineIdAsync(airlineId.Value, cancellationToken),
            $"Aeronaves de la aerolínea #{airlineId}",
            cancellationToken);
        Pause();
    }

    private async Task CreateAsync(CancellationToken cancellationToken)
    {
        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Registrar aeronave"))
            return;

        var form = PromptForm();
        if (form is null)
            return;

        try
        {
            await _service.CreateAsync(form.Value.modelId, form.Value.airlineId, form.Value.registration, form.Value.manufacturedDate, form.Value.isActive, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Aeronave creada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }
        Pause();
    }

    private async Task UpdateAsync(CancellationToken cancellationToken)
    {
        var aircraft = await PromptAircraftSelectionAsync("Seleccione la aeronave a actualizar:", cancellationToken);

        if (aircraft is null)
            return;

        var form = PromptForm(
            aircraft.ModelId.Value,
            aircraft.AirlineId.Value,
            aircraft.Registration.Value,
            aircraft.ManufacturedDate.Value,
            aircraft.IsActive.Value);
        if (form is null)
            return;

        try
        {
            await _service.UpdateAsync(aircraft.Id.Value, form.Value.modelId, form.Value.airlineId, form.Value.registration, form.Value.manufacturedDate, form.Value.isActive, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Aeronave actualizada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }
        Pause();
    }

    private async Task DeactivateAsync(CancellationToken cancellationToken)
    {
        var aircraft = await PromptAircraftSelectionAsync("Seleccione la aeronave a desactivar:", cancellationToken);
        if (aircraft is null)
            return;

        if (!AnsiConsole.Confirm("¿Confirmas la desactivación?", false))
        {
            Pause();
            return;
        }

        try
        {
            await _service.DeactivateAsync(aircraft.Id.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Aeronave desactivada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }
        Pause();
    }

    private async Task RenderTableAsync(IEnumerable<AircraftAggregate> items, string title, CancellationToken cancellationToken)
    {
        var list = items.ToList();
        if (list.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay registros para mostrar.[/]");
            return;
        }

        var modelNames = (await _models.GetAllAsync(cancellationToken))
            .ToDictionary(m => m.Id.Value, m => m.ModelName.Value);
        var airlineNames = (await _airlines.GetAllAsync())
            .Where(a => a.Id != null)
            .ToDictionary(a => a.Id!.Value, a => a.Name.Value);

        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold grey]ID[/]")
            .AddColumn("[bold grey]Modelo[/]")
            .AddColumn("[bold grey]Aerolínea[/]")
            .AddColumn("[bold grey]Matrícula[/]")
            .AddColumn("[bold grey]Fabricación[/]")
            .AddColumn("[bold grey]Activa[/]");

        foreach (var item in list)
        {
            var modelLabel = modelNames.TryGetValue(item.ModelId.Value, out var mn)
                ? $"{item.ModelId.Value} · {Markup.Escape(mn)}"
                : item.ModelId.Value.ToString();
            var airlineLabel = airlineNames.TryGetValue(item.AirlineId.Value, out var an)
                ? $"{item.AirlineId.Value} · {Markup.Escape(an)}"
                : item.AirlineId.Value.ToString();

            table.AddRow(
                item.Id.Value.ToString(),
                modelLabel,
                airlineLabel,
                item.Registration.Value,
                item.ManufacturedDate.Value?.ToString("yyyy-MM-dd") ?? "-",
                item.IsActive.Value ? "Sí" : "No");
        }

        AnsiConsole.Write(table);
    }

    private static (int modelId, int airlineId, string registration, DateTime? manufacturedDate, bool isActive)? PromptForm(
        int? currentModelId = null,
        int? currentAirlineId = null,
        string? currentRegistration = null,
        DateTime? currentManufacturedDate = null,
        bool? currentIsActive = null)
    {
        var modelId = currentModelId is int currentModel
            ? ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("ID del modelo:", currentModel)
            : ConsoleMenuHelpers.PromptPositiveIntOrBack("ID del modelo:");
        if (modelId is null)
            return null;

        var airlineId = currentAirlineId is int currentAirline
            ? ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("ID de la aerolínea:", currentAirline)
            : ConsoleMenuHelpers.PromptPositiveIntOrBack("ID de la aerolínea:");
        if (airlineId is null)
            return null;

        var registration = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Matrícula:", currentRegistration ?? string.Empty);
        if (registration is null)
            return null;

        var manufacturedDate = PromptOptionalDateOrBack("Fecha de fabricación (yyyy-MM-dd, opcional):", currentManufacturedDate);
        if (manufacturedDate.WentBack)
            return null;

        var isActive = PromptActiveSelection(currentIsActive);
        if (isActive is null)
            return null;

        return (modelId.Value, airlineId.Value, registration.ToUpperInvariant(), manufacturedDate.Value, isActive.Value);
    }

    private static (bool WentBack, DateTime? Value) PromptOptionalDateOrBack(string label, DateTime? current = null)
    {
        while (true)
        {
            var text = ConsoleMenuHelpers.PromptStringWithInitialOrBack(label, current?.ToString("yyyy-MM-dd") ?? string.Empty, allowEmpty: true);
            if (text is null)
                return (true, null);

            if (string.IsNullOrWhiteSpace(text))
                return (false, null);

            if (DateTime.TryParse(text.Trim(), out var dt))
                return (false, dt);

            AnsiConsole.MarkupLine("[red]Fecha inválida.[/]");
        }
    }

    private static bool? PromptActiveSelection(bool? current = null)
    {
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Estado de la aeronave:")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices(
                    current == false ? "Inactiva" : "Activa",
                    current == false ? "Activa" : "Inactiva",
                    ConsoleMenuHelpers.VolverAlMenu));

        return selected switch
        {
            "Activa" => true,
            "Inactiva" => false,
            _ => null
        };
    }

    private async Task<AircraftAggregate?> PromptAircraftSelectionAsync(string title, CancellationToken cancellationToken)
    {
        var aircraft = (await _service.GetAllAsync(cancellationToken))
            .OrderBy(x => x.Registration.Value)
            .ToList();
        if (!aircraft.Any())
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay aeronaves registradas.[/]");
            Pause();
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(aircraft.Select(FormatAircraftChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return aircraft.First(x => FormatAircraftChoice(x) == selected);
    }

    private static string FormatAircraftChoice(AircraftAggregate item) =>
        $"{item.Id.Value} · {Markup.Escape(item.Registration.Value)} · Aerolínea {item.AirlineId.Value}";

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Presiona Enter para continuar...[/]").AllowEmpty());
    }
}
