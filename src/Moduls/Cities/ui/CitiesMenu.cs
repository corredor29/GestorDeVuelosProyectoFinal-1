using GestorDeVuelosProyectoFinal.Moduls.Cities.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Cities.ui;

public sealed class CitiesMenu : IModuleUI
{
    private readonly ICityService _service;
    private readonly IRegionService _regions;

    public string Key => "cities";
    public string Title => "Ciudades";

    public CitiesMenu(ICityService service, IRegionService regions)
    {
        _service = service;
        _regions = regions;
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
        // Este menú mezcla consultas y CRUD, así que cada opción delega en un flujo corto separado.
        while (!ct.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Gestión de ciudades [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar y Enter para seleccionar[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .PageSize(10)
                    .AddChoices(
                        "Listar todas las ciudades",
                        "Buscar ciudad por nombre",
                        "Listar ciudades por región",
                        "Crear ciudad",
                        "Actualizar ciudad",
                        "Eliminar ciudad",
                        ConsoleMenuHelpers.VolverAlMenu));

            switch (option)
            {
                case "Listar todas las ciudades": await ListAllAsync(); break;
                case "Buscar ciudad por nombre": await SearchByNameAsync(); break;
                case "Listar ciudades por región": await ListByRegionAsync(); break;
                case "Crear ciudad": await CreateAsync(); break;
                case "Actualizar ciudad": await UpdateAsync(); break;
                case "Eliminar ciudad": await DeleteMenuAsync(); break;
                default: return;
            }
        }
    }

    private async Task ListAllAsync()
    {
        var cities = (await _service.GetAllAsync()).ToList();
        ShowTable(cities, "Todas las ciudades");
        Pause();
    }

    private async Task SearchByNameAsync()
    {
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack(
            "[deepskyblue1]Nombre de la ciudad:[/]",
            value =>
            {
                if (value.Length < 2 || value.Length > 100)
                    return "El nombre debe tener entre 2 y 100 caracteres.";
                if (!value.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                    return "El nombre solo puede contener letras y espacios.";
                return null;
            });

        if (name is null)
            return;

        var city = await _service.GetByNameAsync(name);

        if (city is null)
            AnsiConsole.MarkupLine("[yellow]No se encontró ninguna ciudad con ese nombre.[/]");
        else
            ShowTable(new[] { city }, $"Resultado para {Markup.Escape(name)}");

        Pause();
    }

    private async Task ListByRegionAsync()
    {
        // El filtro usa una selección de región para evitar errores al escribir nombres a mano.
        var selectedRegion = await SelectRegionAsync("[deepskyblue1]Seleccione la región:[/]");
        if (selectedRegion is null)
            return;

        var cities = (await _service.GetByRegionIdAsync(selectedRegion.Id.Value)).ToList();
        ShowTable(cities, $"Ciudades de {Markup.Escape(selectedRegion.Name.Value)}");
        Pause();
    }

    private async Task CreateAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Crear ciudad[/]").LeftJustified());
        AnsiConsole.WriteLine();

        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack(
            "[deepskyblue1]Nombre de la ciudad:[/]",
            value =>
            {
                if (value.Length < 2 || value.Length > 100)
                    return "El nombre debe tener entre 2 y 100 caracteres.";
                if (!value.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                    return "El nombre solo puede contener letras y espacios.";
                return null;
            });

        if (name is null)
            return;

        var selectedRegion = await SelectRegionAsync("[deepskyblue1]Seleccione la región disponible:[/]");
        if (selectedRegion is null)
            return;

        var preview = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[grey]Campo[/]")
            .AddColumn("[grey]Valor[/]")
            .AddRow("Nombre", $"[white]{Markup.Escape(name)}[/]")
            .AddRow("Región", $"[white]{Markup.Escape(selectedRegion.Name.Value)}[/]");

        AnsiConsole.WriteLine();
        AnsiConsole.Write(preview);

        var confirm = ConsoleMenuHelpers.PromptSaveCancelOrBack("\n¿Confirmar creación?");
        if (confirm != ConsoleMenuHelpers.SaveChoice.Guardar)
            return;

        await _service.CreateAsync(name, selectedRegion.Id.Value);
        AnsiConsole.MarkupLine("[green]Ciudad registrada correctamente.[/]");
        Pause();
    }

    private async Task UpdateAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Actualizar ciudad[/]").LeftJustified());
        AnsiConsole.WriteLine();

        // Se elige primero la ciudad real y después se dejan editar nombre y región.
        var cities = (await _service.GetAllAsync()).OrderBy(c => c.Name.Value).ToList();
        if (cities.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay ciudades registradas.[/]");
            Pause();
            return;
        }

        var selectedCityText = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[deepskyblue1]Seleccione la ciudad a modificar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(cities.Select(c => $"{c.Name.Value}  (ID {c.Id.Value})").Append(ConsoleMenuHelpers.VolverAlMenu)));

        if (selectedCityText == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var selectedCity = cities.First(c => $"{c.Name.Value}  (ID {c.Id.Value})" == selectedCityText);

        var newName = ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            $"[deepskyblue1]Nombre de la ciudad[/] [grey](actual: {Markup.Escape(selectedCity.Name.Value)})[/]:",
            selectedCity.Name.Value,
            validate: value =>
            {
                if (value.Length < 2 || value.Length > 100)
                    return "El nombre debe tener entre 2 y 100 caracteres.";
                if (!value.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                    return "El nombre solo puede contener letras y espacios.";
                return null;
            });

        if (newName is null)
            return;

        var selectedRegion = await SelectRegionAsync("[deepskyblue1]Seleccione la nueva región:[/]", selectedCity.RegionId.Value);
        if (selectedRegion is null)
            return;

        var preview = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[grey]Campo[/]")
            .AddColumn("[grey]Nuevo valor[/]")
            .AddRow("Ciudad", $"[white]{Markup.Escape(newName)}[/]")
            .AddRow("Región", $"[white]{Markup.Escape(selectedRegion.Name.Value)}[/]");

        AnsiConsole.WriteLine();
        AnsiConsole.Write(preview);

        var confirm = ConsoleMenuHelpers.PromptSaveCancelOrBack("\n¿Confirmar actualización?");
        if (confirm != ConsoleMenuHelpers.SaveChoice.Guardar)
            return;

        await _service.UpdateAsync(selectedCity.Id.Value, newName, selectedRegion.Id.Value);
        AnsiConsole.MarkupLine("[green]Ciudad actualizada correctamente.[/]");
        Pause();
    }

    private async Task DeleteMenuAsync()
    {
        var subOption = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red]Menú de eliminación[/]")
                .HighlightStyle(new Style(foreground: Color.Red))
                .AddChoices("Eliminar por ciudad", "Eliminar todas por región", "Cancelar"));

        switch (subOption)
        {
            case "Eliminar por ciudad":
            {
                var cities = (await _service.GetAllAsync()).OrderBy(c => c.Name.Value).ToList();
                if (cities.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]No hay ciudades para eliminar.[/]");
                    Pause();
                    return;
                }

                var selectedCityText = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[red]Seleccione la ciudad a eliminar[/]")
                        .HighlightStyle(new Style(foreground: Color.Red))
                        .PageSize(12)
                        .AddChoices(cities.Select(c => $"{c.Name.Value}  (ID {c.Id.Value})").Append(ConsoleMenuHelpers.VolverAlMenu)));

                if (selectedCityText == ConsoleMenuHelpers.VolverAlMenu)
                    return;

                var selectedCity = cities.First(c => $"{c.Name.Value}  (ID {c.Id.Value})" == selectedCityText);
                if (AnsiConsole.Confirm($"[red]¿Eliminar la ciudad {Markup.Escape(selectedCity.Name.Value)}?[/]"))
                    await _service.DeleteAsync(selectedCity.Id.Value);
                break;
            }
            case "Eliminar todas por región":
            {
                var selectedRegion = await SelectRegionAsync("[red]Seleccione la región a vaciar:[/]");
                if (selectedRegion is null)
                    return;

                if (AnsiConsole.Confirm($"[red]¿Eliminar todas las ciudades de {Markup.Escape(selectedRegion.Name.Value)}?[/]"))
                    await _service.DeleteByRegionIdAsync(selectedRegion.Id.Value);
                break;
            }
            default:
                return;
        }

        AnsiConsole.MarkupLine("[green]Operación procesada correctamente.[/]");
        Pause();
    }

    private void ShowTable(IEnumerable<GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.Aggregate.City> cities, string title)
    {
        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .AddColumn("[grey]ID[/]")
            .AddColumn("[grey]Nombre[/]")
            .AddColumn("[grey]Región ID[/]");

        foreach (var city in cities)
            table.AddRow(
                city.Id.Value.ToString(),
                Markup.Escape(city.Name.Value),
                city.RegionId.Value.ToString());

        AnsiConsole.Write(table);
    }

    private async Task<GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.Aggregate.Region?> SelectRegionAsync(
        string title,
        int? currentRegionId = null)
    {
        var regions = (await _regions.GetAllAsync()).OrderBy(r => r.Name.Value).ToList();
        if (regions.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay regiones disponibles.[/]");
            Pause();
            return null;
        }

        const string keepCurrent = "Mantener región actual";
        var choices = regions.Select(r => $"{r.Name.Value}  ({r.Type.Value})").ToList();
        if (currentRegionId.HasValue)
            choices.Insert(0, keepCurrent);
        choices.Add(ConsoleMenuHelpers.VolverAlMenu);

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(choices));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        if (selected == keepCurrent)
            return regions.First(r => r.Id.Value == currentRegionId!.Value);

        return regions.First(r => $"{r.Name.Value}  ({r.Type.Value})" == selected);
    }

    private static void Pause()
    {
        AnsiConsole.MarkupLine("\n[grey]Presiona Enter para continuar...[/]");
        Console.ReadLine();
    }
}
