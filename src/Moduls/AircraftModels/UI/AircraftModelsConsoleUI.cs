using System.Linq;
using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.UI;

public sealed class AircraftModelsConsoleUI : IModuleUI
{
    private readonly IAircraftModelsService _service;
    private readonly IAircraftManufacturersService _manufacturers;

    public AircraftModelsConsoleUI(IAircraftModelsService service, IAircraftManufacturersService manufacturers)
    {
        _service = service;
        _manufacturers = manufacturers;
    }

    public string Key => "aircraft-models";
    public string Title => "Modelos de Aeronave";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Modelos de Aeronave [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Listar todos",
                        "Buscar por ID",
                        "Buscar por nombre",
                        "Buscar por fabricante",
                        "Crear modelo",
                        "Actualizar modelo",
                        "Eliminar modelo",
                        "Volver"));

            switch (option)
            {
                case "Listar todos": await ListAllAsync(cancellationToken); break;
                case "Buscar por ID": await SearchByIdAsync(cancellationToken); break;
                case "Buscar por nombre": await SearchByNameAsync(cancellationToken); break;
                case "Buscar por fabricante": await SearchByManufacturerAsync(cancellationToken); break;
                case "Crear modelo": await CreateAsync(cancellationToken); break;
                case "Actualizar modelo": await UpdateAsync(cancellationToken); break;
                case "Eliminar modelo": await DeleteAsync(cancellationToken); break;
                case "Volver": return;
            }
        }
    }

    private async Task ListAllAsync(CancellationToken cancellationToken)
    {
        await RenderTableAsync(await _service.GetAllAsync(cancellationToken), "Todos los modelos", cancellationToken);
        Pause();
    }

    private async Task SearchByIdAsync(CancellationToken cancellationToken)
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID del modelo:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value, cancellationToken);
        await RenderTableAsync(item is null ? Array.Empty<AircraftModel>() : new[] { item }, $"Resultado para ID {id.Value}", cancellationToken);
        Pause();
    }

    private async Task SearchByNameAsync(CancellationToken cancellationToken)
    {
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre del modelo:");
        if (name is null)
            return;

        var item = await _service.GetByNameAsync(name, cancellationToken);
        await RenderTableAsync(item is null ? Array.Empty<AircraftModel>() : new[] { item }, $"Resultado para {name}", cancellationToken);
        Pause();
    }

    private async Task SearchByManufacturerAsync(CancellationToken cancellationToken)
    {
        var manufacturerId = await PromptManufacturerSelectionAsync(cancellationToken);
        if (manufacturerId is null)
            return;

        await RenderTableAsync(
            await _service.GetByManufacturerIdAsync(manufacturerId.Value, cancellationToken),
            $"Modelos del fabricante {manufacturerId.Value}",
            cancellationToken);
        Pause();
    }

    private async Task CreateAsync(CancellationToken cancellationToken)
    {
        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Registrar modelo de aeronave"))
            return;

        var form = await PromptFormAsync(cancellationToken);
        if (form is null)
            return;

        try
        {
            await _service.CreateAsync(form.Value.manufacturerId, form.Value.name, form.Value.maxCapacity, form.Value.weight, form.Value.fuel, form.Value.speed, form.Value.altitude, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Modelo creado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{ex.Message}[/]");
        }
        Pause();
    }

    private async Task UpdateAsync(CancellationToken cancellationToken)
    {
        var model = await PromptAircraftModelSelectionAsync(
            "Seleccione el modelo a actualizar:",
            cancellationToken);

        if (model is null)
        {
            return;
        }

        var form = await PromptFormAsync(
            cancellationToken,
            model.ManufacturerId.Value,
            model.ModelName.Value,
            model.MaxCapacity.Value,
            model.MaxTakeoffWeight.Value,
            model.FuelConsumption.Value,
            model.CruiseSpeed.Value,
            model.CruiseAltitude.Value);
        if (form is null)
            return;

        try
        {
            await _service.UpdateAsync(model.Id!.Value, form.Value.manufacturerId, form.Value.name, form.Value.maxCapacity, form.Value.weight, form.Value.fuel, form.Value.speed, form.Value.altitude, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Modelo actualizado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{ex.Message}[/]");
        }
        Pause();
    }

    private async Task DeleteAsync(CancellationToken cancellationToken)
    {
        var model = await PromptAircraftModelSelectionAsync(
            "Seleccione el modelo a eliminar:",
            cancellationToken);
        if (model is null)
            return;

        if (!AnsiConsole.Confirm("¿Confirmas la eliminación?", false))
        {
            Pause();
            return;
        }

        try
        {
            await _service.DeleteAsync(model.Id!.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Modelo eliminado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{ex.Message}[/]");
        }
        Pause();
    }

    private async Task RenderTableAsync(IEnumerable<AircraftModel> items, string title, CancellationToken cancellationToken)
    {
        var list = items.ToList();
        if (list.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay registros para mostrar.[/]");
            return;
        }

        var mfgNames = (await _manufacturers.GetAllAsync(cancellationToken))
            .ToDictionary(m => m.Id.Value, m => m.Name.Value);

        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold grey]ID[/]")
            .AddColumn("[bold grey]Fabricante[/]")
            .AddColumn("[bold grey]Modelo[/]")
            .AddColumn("[bold grey]Capacidad[/]")
            .AddColumn("[bold grey]MTOW[/]")
            .AddColumn("[bold grey]Consumo[/]")
            .AddColumn("[bold grey]Velocidad[/]")
            .AddColumn("[bold grey]Altitud[/]");

        foreach (var item in list)
        {
            var mfgLabel = mfgNames.TryGetValue(item.ManufacturerId.Value, out var mn)
                ? $"{item.ManufacturerId.Value} · {Markup.Escape(mn)}"
                : item.ManufacturerId.Value.ToString();

            table.AddRow(
                item.Id.Value.ToString(),
                mfgLabel,
                item.ModelName.Value,
                item.MaxCapacity.Value.ToString(),
                item.MaxTakeoffWeight.Value?.ToString("0.##") ?? "-",
                item.FuelConsumption.Value?.ToString("0.##") ?? "-",
                item.CruiseSpeed.Value?.ToString() ?? "-",
                item.CruiseAltitude.Value?.ToString() ?? "-");
        }

        AnsiConsole.Write(table);
    }

    private async Task<(int manufacturerId, string name, int maxCapacity, decimal? weight, decimal? fuel, int? speed, int? altitude)?>
        PromptFormAsync(
            CancellationToken cancellationToken,
            int? currentManufacturerId = null,
            string? currentName = null,
            int? currentCapacity = null,
            decimal? currentWeight = null,
            decimal? currentFuel = null,
            int? currentSpeed = null,
            int? currentAltitude = null)
    {
        var manufacturerId = await PromptManufacturerSelectionAsync(cancellationToken, currentManufacturerId);
        if (manufacturerId is null)
            return null;

        var name = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nombre del modelo:", currentName ?? string.Empty);
        if (name is null)
            return null;

        var maxCapacity = currentCapacity is int currentCapacityValue
            ? ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("Capacidad máxima:", currentCapacityValue)
            : ConsoleMenuHelpers.PromptPositiveIntOrBack("Capacidad máxima:");
        if (maxCapacity is null)
            return null;

        var weight = PromptOptionalDecimalOrBack("MTOW kg (opcional):", currentWeight);
        if (weight.WentBack)
            return null;

        var fuel = PromptOptionalDecimalOrBack("Consumo kg/h (opcional):", currentFuel);
        if (fuel.WentBack)
            return null;

        var speed = PromptOptionalIntOrBack("Velocidad km/h (opcional):", currentSpeed);
        if (speed.WentBack)
            return null;

        var altitude = PromptOptionalIntOrBack("Altitud ft (opcional):", currentAltitude);
        if (altitude.WentBack)
            return null;

        return (manufacturerId.Value, name, maxCapacity.Value, weight.Value, fuel.Value, speed.Value, altitude.Value);
    }

    private async Task<int?> PromptManufacturerSelectionAsync(CancellationToken cancellationToken, int? currentManufacturerId = null)
    {
        var manufacturers = (await _manufacturers.GetAllAsync(cancellationToken))
            .OrderBy(m => m.Name.Value)
            .ToList();
        if (manufacturers.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay fabricantes registrados.[/]");
            return null;
        }

        var orderedChoices = manufacturers
            .OrderByDescending(m => m.Id.Value == currentManufacturerId)
            .ThenBy(m => m.Name.Value)
            .Select(FormatManufacturerChoice)
            .Append(ConsoleMenuHelpers.VolverAlMenu)
            .ToList();

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Seleccione el fabricante:")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(orderedChoices));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return manufacturers.First(m => FormatManufacturerChoice(m) == selected).Id.Value;
    }

    private async Task<AircraftModel?> PromptAircraftModelSelectionAsync(string title, CancellationToken cancellationToken)
    {
        var models = (await _service.GetAllAsync(cancellationToken))
            .OrderBy(m => m.ModelName.Value)
            .ToList();
        if (models.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay modelos registrados.[/]");
            Pause();
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(models.Select(FormatAircraftModelChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return models.First(m => FormatAircraftModelChoice(m) == selected);
    }

    private static (bool WentBack, decimal? Value) PromptOptionalDecimalOrBack(string label, decimal? current = null)
    {
        while (true)
        {
            var text = ConsoleMenuHelpers.PromptStringWithInitialOrBack(label, current?.ToString() ?? string.Empty, allowEmpty: true);
            if (text is null)
                return (true, null);

            if (string.IsNullOrWhiteSpace(text))
                return (false, null);

            if (decimal.TryParse(text.Trim(), System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out var decimalValue))
                return (false, decimalValue);

            if (decimal.TryParse(text.Trim(), out decimalValue))
                return (false, decimalValue);

            AnsiConsole.MarkupLine("[red]Número decimal no válido.[/]");
        }
    }

    private static (bool WentBack, int? Value) PromptOptionalIntOrBack(string label, int? current = null)
    {
        while (true)
        {
            var text = ConsoleMenuHelpers.PromptStringWithInitialOrBack(label, current?.ToString() ?? string.Empty, allowEmpty: true);
            if (text is null)
                return (true, null);

            if (string.IsNullOrWhiteSpace(text))
                return (false, null);

            if (int.TryParse(text.Trim(), out var v))
                return (false, v);

            AnsiConsole.MarkupLine("[red]Debe ser un número entero válido.[/]");
        }
    }

    private static string FormatManufacturerChoice(dynamic manufacturer) =>
        $"{manufacturer.Id.Value} · {Markup.Escape(manufacturer.Name.Value)}";

    private static string FormatAircraftModelChoice(AircraftModel model) =>
        $"{model.Id?.Value ?? 0} · {Markup.Escape(model.ModelName.Value)}";

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Presiona Enter para continuar...[/]").AllowEmpty());
    }
}
