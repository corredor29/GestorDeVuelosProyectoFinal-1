using GestorDeVuelosProyectoFinal.Moduls.Countries.Application.Interfaces;
using Spectre.Console;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Regions.ui;

public sealed class RegionsMenu : IModuleUI
{
    private readonly IRegionService _service;
    private readonly ICountryService _countries;

    public string Key => "regions";
    public string Title => "Regiones";

    private static readonly string[] _regionTypes = new[]
    {
        "Departamento", "Estado", "Provincia",
        "Región", "Comunidad Autónoma", "Otro"
    };

    public RegionsMenu(IRegionService service, ICountryService countries)
    {
        _service = service;
        _countries = countries;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new Rule("[bold deepskyblue1] Gestión de regiones [/]").LeftJustified());

                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("\n[grey]Flechas y Enter para elegir[/]")
                        .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                        .AddChoices(
                            "Listar todas las regiones",
                            "Filtrar por país",
                            "Filtrar por tipo",
                            "Crear región",
                            "Actualizar región",
                            "Eliminar región",
                            ConsoleMenuHelpers.VolverAlMenu
                        )
                );

                switch (option)
                {
                    case "Listar todas las regiones": await ListAllAsync(); break;
                    case "Filtrar por país": await FilterByCountryAsync(); break;
                    case "Filtrar por tipo": await FilterByTypeAsync(); break;
                    case "Crear región": await CreateAsync(); break;
                    case "Actualizar región": await UpdateAsync(); break;
                    case "Eliminar región": await DeleteAsync(); break;
                    default: return;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"\n[red]Error: {Markup.Escape(ex.Message)}[/]");
                Pause();
            }
        }
    }

    // LIST ALL

    private async Task ListAllAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Todas las regiones[/]").LeftJustified());

        try
        {
            var regions = (await _service.GetAllAsync()).ToList();

            if (regions.Count == 0)
                AnsiConsole.MarkupLine("\n[yellow]No hay regiones registradas.[/]");
            else
            {
                AnsiConsole.Write(BuildTable(regions));
                AnsiConsole.MarkupLine($"\n[grey]Total: {regions.Count}[/]");
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error al cargar regiones: {Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    // FILTER BY COUNTRY

    private async Task FilterByCountryAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Filtrar por país[/]").LeftJustified());
        AnsiConsole.WriteLine();

        // Igual que en otros catálogos, el filtro se apoya en una lista para no pedir ids.
        var allCountries = (await _countries.GetAllAsync()).OrderBy(c => c.Name.Value).ToList();
        if (allCountries.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay países.[/]");
            Pause();
            return;
        }

        var choices = allCountries.Select(c => c.Name.Value).Append(ConsoleMenuHelpers.VolverAlMenu).ToList();
        var pick = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[deepskyblue1]Seleccione el país[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(10)
                .AddChoices(choices));

        if (pick == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var country = allCountries.First(c => c.Name.Value == pick);
        var countryId = country.Id.Value;

        var regions = (await _service.GetByCountryIdAsync(countryId)).ToList();

        if (regions.Count == 0)
            AnsiConsole.MarkupLine("\n[yellow]No hay regiones para ese país.[/]");
        else
        {
            AnsiConsole.Write(BuildTable(regions));
            AnsiConsole.MarkupLine($"\n[grey]Total: {regions.Count}[/]");
        }

        Pause();
    }

 
    // FILTER BY TYPE
 
    private async Task FilterByTypeAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Filtrar por tipo[/]").LeftJustified());
        AnsiConsole.WriteLine();

        var typeChoices = _regionTypes.ToList();
        typeChoices.Add("Cancelar");

        var selectedType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[deepskyblue1]Selecciona un tipo de región:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices(typeChoices)
        );

        if (selectedType == "Cancelar") return;

        var regions = (await _service.GetByTypeAsync(selectedType)).ToList();

        if (regions.Count == 0)
            AnsiConsole.MarkupLine($"\n[yellow]No se encontraron regiones del tipo '{Markup.Escape(selectedType)}'.[/]");
        else
        {
            AnsiConsole.Write(BuildTable(regions));
            AnsiConsole.MarkupLine($"\n[grey]Total: {regions.Count} región(es)[/]");
        }

        Pause();
    }


    // CREATE
    private async Task CreateAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Crear región[/]").LeftJustified());
        AnsiConsole.WriteLine();

        // Primero se define la identidad de la región y luego se amarra al país correspondiente.
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack(
            "[deepskyblue1]Nombre de la región:[/]",
            n =>
            {
                if (n.Length < 2 || n.Length > 100)
                    return "El nombre debe tener entre 2 y 100 caracteres.";
                if (!n.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                    return "El nombre solo puede contener letras y espacios.";
                return null;
            });

        if (name is null)
            return;

        var typeChoices = _regionTypes.ToList();
        typeChoices.Add(ConsoleMenuHelpers.VolverAlMenu);
        var type = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[deepskyblue1]Selecciona el tipo de región:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices(typeChoices)
        );

        if (type == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var selectedCountry = await SelectCountryAsync("[deepskyblue1]Selecciona el país:[/]", includeKeepCurrent: false);
        if (selectedCountry is null)
            return;

        AnsiConsole.WriteLine();
        var preview = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[grey]Field[/]")
            .AddColumn("[grey]Value[/]")
            .AddRow("Nombre", $"[white]{name}[/]")
            .AddRow("Tipo", $"[deepskyblue1]{type}[/]")
            .AddRow("País", $"[white]{Markup.Escape(selectedCountry.Name.Value)}[/]");

        AnsiConsole.Write(preview);

        var confirm = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\n¿Confirmar?")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices("Sí, guardar", "No, cancelar")
        );

        if (confirm == "Sí, guardar")
        {
            try
            {
                await AnsiConsole.Status()
                    .StartAsync("Guardando...", async _ =>
                        await _service.CreateAsync(name, type, selectedCountry.Id.Value));

                AnsiConsole.MarkupLine("\n[green]Región creada correctamente.[/]");
            }
            catch (InvalidOperationException ex)
            {
                AnsiConsole.MarkupLine($"\n[red]Error: {ex.Message}[/]");
            }
        }
        else
        {
            AnsiConsole.MarkupLine("\n[yellow]Operación cancelada.[/]");
        }

        Pause();
    }


    // UPDATE

    private async Task UpdateAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Actualizar región[/]").LeftJustified());

        var regions = (await _service.GetAllAsync()).ToList();

        if (regions.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay regiones para actualizar.[/]");
            Pause();
            return;
        }

        // SelectionPrompt trata el texto como markup: evitamos [] y escapamos valores.
        var choices = regions.Select(r => $"{r.Id.Value}  —  {r.Name.Value}  ({r.Type.Value})").ToList();
        choices.Add(ConsoleMenuHelpers.VolverAlMenu);

        AnsiConsole.WriteLine();
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[deepskyblue1]Selecciona la región a actualizar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(10)
                .AddChoices(choices)
        );

        if (selected == ConsoleMenuHelpers.VolverAlMenu) return;

        var idPart = selected.Split("  —  ")[0].Trim();
        if (!int.TryParse(idPart, out var regionId))
        {
            AnsiConsole.MarkupLine("\n[red]Selección inválida.[/]");
            Pause();
            return;
        }
        var currentName = selected.Split("  —  ")[1].Split("  (")[0].Trim();
        var currentType = selected.Split("  (")[1].TrimEnd(')');

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[grey]Editando:[/] [bold]{Markup.Escape(currentName)}[/] [grey]({Markup.Escape(currentType)})[/]");
        AnsiConsole.WriteLine();

        var newName = ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            $"[deepskyblue1]Nuevo nombre[/] [grey](actual: {Markup.Escape(currentName)})[/]:",
            currentName,
            validate: n =>
            {
                if (n.Length < 2 || n.Length > 100)
                    return "El nombre debe tener entre 2 y 100 caracteres.";
                if (!n.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                    return "El nombre solo puede contener letras y espacios.";
                return null;
            });

        if (newName is null)
            return;

        var typeChoices = _regionTypes.ToList();
        typeChoices.Insert(0, $"Mantener actual ({currentType})");

        var newTypeSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[deepskyblue1]Selecciona el nuevo tipo:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices(typeChoices)
        );

        var allCountries = (await _countries.GetAllAsync()).OrderBy(c => c.Name.Value).ToList();
        var currentCountry = allCountries.FirstOrDefault(c => c.Id.Value == regions.First(r => r.Id.Value == regionId).CountryId.Value);
        var selectedCountry = await SelectCountryAsync(
            currentCountry is null
                ? "[deepskyblue1]Selecciona el país:[/]"
                : $"[deepskyblue1]Selecciona el país[/] [grey](actual: {Markup.Escape(currentCountry.Name.Value)})[/]:",
            includeKeepCurrent: true,
            currentCountryId: currentCountry?.Id.Value);

        if (selectedCountry is null)
            return;

        var finalName      = newName;
        var finalType      = newTypeSelection.StartsWith("Mantener") ? currentType : newTypeSelection;
        var finalCountry   = selectedCountry.Id.Value;

        AnsiConsole.WriteLine();
        var preview = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[grey]Field[/]")
            .AddColumn("[grey]New value[/]")
            .AddRow("Nombre", $"[white]{Markup.Escape(finalName)}[/]")
            .AddRow("Tipo", $"[deepskyblue1]{finalType}[/]")
            .AddRow("País", $"[white]{Markup.Escape(selectedCountry.Name.Value)}[/]");

        AnsiConsole.Write(preview);

        var confirm = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\n¿Confirmar?")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices("Sí, actualizar", "No, cancelar")
        );

        if (confirm == "Sí, actualizar")
        {
            try
            {
                await AnsiConsole.Status()
                    .StartAsync("Actualizando...", async _ =>
                        await _service.UpdateAsync(regionId, finalName, finalType, finalCountry));

                AnsiConsole.MarkupLine("\n[green]Región actualizada correctamente.[/]");
            }
            catch (InvalidOperationException ex)
            {
                AnsiConsole.MarkupLine($"\n[red]Error: {ex.Message}[/]");
            }
        }
        else
        {
            AnsiConsole.MarkupLine("\n[yellow]Operación cancelada.[/]");
        }

        Pause();
    }

 
    // DELETE
 
    private async Task DeleteAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Eliminar región[/]").LeftJustified());
        AnsiConsole.WriteLine();

        // Se elimina SOLO por nombre (no permitimos borrar todas por tipo).
        {
            var regions = (await _service.GetAllAsync()).ToList();

            if (regions.Count == 0)
            {
                AnsiConsole.MarkupLine("\n[yellow]No hay regiones para eliminar.[/]");
                Pause();
                return;
            }

            var choices = regions.Select(r => $"{r.Name.Value}  ({r.Type.Value})").ToList();
            choices.Add(ConsoleMenuHelpers.VolverAlMenu);

            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[deepskyblue1]Seleccione la región[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .PageSize(10)
                    .AddChoices(choices)
            );

            if (selected == ConsoleMenuHelpers.VolverAlMenu) return;

            var name = selected.Split("  (")[0].Trim();

            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Panel($"[bold red]{Markup.Escape(name)}[/]")
                .Header("[red]A punto de eliminar[/]")
                .BorderColor(Color.Red));
            AnsiConsole.MarkupLine("\n[red]Advertencia:[/] Esta acción no se puede deshacer.");

            var confirm = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n¿Confirma eliminación?")
                    .HighlightStyle(new Style(foreground: Color.Red))
                    .AddChoices("Sí, eliminar", "No, cancelar")
            );

            if (confirm == "Sí, eliminar")
            {
                try
                {
                    await AnsiConsole.Status()
                        .StartAsync("Eliminando...", async _ =>
                            await _service.DeleteByNameAsync(name));

                    AnsiConsole.MarkupLine("\n[green]Región eliminada correctamente.[/]");
                }
                catch (InvalidOperationException ex)
                {
                    AnsiConsole.MarkupLine($"\n[red]Error: {Markup.Escape(ex.Message)}[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("\n[yellow]Operación cancelada.[/]");
            }
        }

        Pause();
    }

 
    // HELPERS

    private static Table BuildTable(IEnumerable<GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.Aggregate.Region> regions)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn(new TableColumn("[bold grey]ID[/]").Centered())
            .AddColumn(new TableColumn("[bold grey]Nombre[/]"))
            .AddColumn(new TableColumn("[bold grey]Tipo[/]"))
            .AddColumn(new TableColumn("[bold grey]ID país[/]").Centered());

        foreach (var r in regions)
            table.AddRow(
                $"[grey]{r.Id.Value}[/]",
                $"[white]{Markup.Escape(r.Name.Value)}[/]",
                $"[deepskyblue1]{Markup.Escape(r.Type.Value)}[/]",
                $"[grey]{r.CountryId.Value}[/]"
            );

        return table;
    }

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(
            new TextPrompt<string>("[grey]Presiona Enter para continuar...[/]")
                .AllowEmpty()
        );
    }

    private async Task<GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.Aggregate.Country?> SelectCountryAsync(
        string title,
        bool includeKeepCurrent,
        int? currentCountryId = null)
    {
        var countries = (await _countries.GetAllAsync()).OrderBy(c => c.Name.Value).ToList();
        if (countries.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay países disponibles.[/]");
            Pause();
            return null;
        }

        const string keepCurrent = "Mantener país actual";
        var choices = countries.Select(c => $"{c.Name.Value}  ({c.IsoCode.Value})").ToList();
        if (includeKeepCurrent)
            choices.Insert(0, keepCurrent);
        choices.Add(ConsoleMenuHelpers.VolverAlMenu);

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(10)
                .AddChoices(choices));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        if (selected == keepCurrent && currentCountryId.HasValue)
            return countries.First(c => c.Id.Value == currentCountryId.Value);

        return countries.First(c => $"{c.Name.Value}  ({c.IsoCode.Value})" == selected);
    }
}
