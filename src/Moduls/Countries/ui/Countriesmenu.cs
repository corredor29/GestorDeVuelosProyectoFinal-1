using GestorDeVuelosProyectoFinal.Moduls.Continents.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Countries.ui;

public sealed class CountriesMenu : IModuleUI
{
    private readonly ICountryService _service;
    private readonly IContinentService _continents;

    public string Key => "countries";
    public string Title => "Países";

    public CountriesMenu(ICountryService service, IContinentService continents)
    {
        _service = service;
        _continents = continents;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Gestión de países [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Navegue con las flechas y pulse Enter[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Listar todos los países",
                        "Filtrar por continente",
                        "Crear país",
                        "Actualizar país",
                        "Eliminar país",
                        ConsoleMenuHelpers.VolverAlMenu
                    )
            );

            switch (option)
            {
                case "Listar todos los países": await ListAllAsync(); break;
                case "Filtrar por continente": await ListByContinentAsync(); break;
                case "Crear país": await CreateAsync(); break;
                case "Actualizar país": await UpdateAsync(); break;
                case "Eliminar país": await DeleteAsync(); break;
                default: return;
            }
        }
    }

    private async Task ListAllAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Todos los países[/]").LeftJustified());

        var countries = (await _service.GetAllAsync()).ToList();

        if (countries.Count == 0)
            AnsiConsole.MarkupLine("\n[yellow]No hay países registrados.[/]");
        else
            RenderCountryTable(countries);

        Pause();
    }

    private async Task ListByContinentAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Países por continente[/]").LeftJustified());
        AnsiConsole.WriteLine();

        // Aquí se aprovecha el catálogo de continentes para guiar el filtro sin pedir ids.
        var continents = (await _continents.GetAllAsync()).OrderBy(c => c.Name.Value).ToList();
        if (continents.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay continentes. Cree continentes primero.[/]");
            Pause();
            return;
        }

        var choices = continents.Select(c => c.Name.Value).Append(ConsoleMenuHelpers.VolverAlMenu).ToList();
        var pick = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[deepskyblue1]Seleccione el continente[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices(choices));

        if (pick == ConsoleMenuHelpers.VolverAlMenu)
            return;

        List<GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.Aggregate.Country> countries;
        try
        {
            countries = (await _service.GetByContinentNameAsync(pick)).ToList();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
            Pause();
            return;
        }

        if (countries.Count == 0)
            AnsiConsole.MarkupLine("\n[yellow]No hay países en ese continente.[/]");
        else
            RenderCountryTable(countries);

        Pause();
    }

    private async Task CreateAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Crear país[/]").LeftJustified());
        AnsiConsole.WriteLine();

        // El formulario primero valida nombre e ISO antes de pasar a la relación con continente.
        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Crear país"))
        {
            Pause();
            return;
        }

        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack(
            "[deepskyblue1]Nombre del país:[/]",
            n =>
            {
                if (n.Length < 2 || n.Length > 100)
                    return "El nombre debe tener entre 2 y 100 caracteres.";
                if (!n.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                    return "El nombre solo puede contener letras y espacios.";
                return null;
            });

        if (name is null)
        {
            AnsiConsole.MarkupLine("[yellow]Operación cancelada.[/]");
            Pause();
            return;
        }

        var isoCode = ConsoleMenuHelpers.PromptRequiredStringOrBack(
            "[deepskyblue1]Código ISO (2–3 letras, ej. COL):[/]",
            c =>
            {
                var cleaned = c.ToUpper();
                    if (cleaned.Length < 2 || cleaned.Length > 3)
                    return "El código ISO debe tener 2 o 3 letras.";
                    if (!cleaned.All(char.IsLetter))
                    return "El código ISO solo debe contener letras.";
                return null;
            });

        if (isoCode is null)
        {
            AnsiConsole.MarkupLine("[yellow]Operación cancelada.[/]");
            Pause();
            return;
        }

        var continentId = await PromptPickContinentIdAsync();
        if (continentId is null)
        {
            Pause();
            return;
        }

        AnsiConsole.WriteLine();
        var panel = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[grey]Campo[/]")
            .AddColumn("[grey]Valor[/]")
            .AddRow("Nombre", $"[white]{name}[/]")
            .AddRow("ISO", $"[deepskyblue1]{isoCode.ToUpper()}[/]")
            .AddRow("Continente (id)", $"[white]{continentId}[/]");

        AnsiConsole.Write(panel);

        var confirm = ConsoleMenuHelpers.PromptSaveCancelOrBack("\n¿Confirmar creación?");
        if (confirm == ConsoleMenuHelpers.SaveChoice.VolverAlMenu)
        {
            Pause();
            return;
        }

        if (confirm == ConsoleMenuHelpers.SaveChoice.Guardar)
        {
            try
            {
                await AnsiConsole.Status()
                    .StartAsync("Guardando…", async _ =>
                        await _service.CreateAsync(name, isoCode, continentId.Value));

                AnsiConsole.MarkupLine("\n[green]País creado correctamente.[/]");
            }
            catch (InvalidOperationException ex)
            {
                AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
            }
        }
        else
            AnsiConsole.MarkupLine("\n[yellow]Operación cancelada.[/]");

        Pause();
    }

    private async Task UpdateAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Actualizar país[/]").LeftJustified());

        // Se presenta una etiqueta compuesta para que la selección sea más clara que solo ver un id.
        var countries = (await _service.GetAllAsync()).ToList();

        if (countries.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay países para actualizar.[/]");
            Pause();
            return;
        }

        var choices = countries.Select(c => $"{c.IsoCode.Value}  —  {c.Name.Value}").ToList();
        choices.Add(ConsoleMenuHelpers.VolverAlMenu);

        AnsiConsole.WriteLine();
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[deepskyblue1]Seleccione el país a editar[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(10)
                .AddChoices(choices)
        );

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var currentIsoCode = selected.Split("  —  ")[0].Trim();
        var currentName = selected.Split("  —  ")[1].Trim();

        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Actualizar país"))
        {
            Pause();
            return;
        }

        var currentRow = countries.First(c => c.IsoCode.Value == currentIsoCode);

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[grey]Editando:[/] [bold]{currentName}[/] [grey]({currentIsoCode})[/]");
        AnsiConsole.WriteLine();

        var newName = ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            $"[deepskyblue1]Nuevo nombre[/] [grey](actual: {Markup.Escape(currentName)})[/]:",
            currentName,
            validate: n =>
            {
                if (n.Length < 2 || n.Length > 100)
                    return "Entre 2 y 100 caracteres.";
                if (!n.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                    return "Solo puede contener letras y espacios.";
                return null;
            });

        if (newName is null)
        {
            Pause();
            return;
        }

        var newIso = ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            $"[deepskyblue1]Nuevo código ISO[/] [grey](actual: {currentIsoCode})[/]:",
            currentIsoCode,
            validate: c =>
            {
                var cleaned = c.Trim().ToUpper();
                if (cleaned.Length < 2 || cleaned.Length > 3)
                    return "2 o 3 letras.";
                if (!cleaned.All(char.IsLetter))
                    return "Solo letras.";
                return null;
            });

        if (newIso is null)
        {
            Pause();
            return;
        }

        var continentsList = (await _continents.GetAllAsync()).OrderBy(c => c.Name.Value).ToList();
        var currentContName = continentsList.FirstOrDefault(x => x.Id.Value == currentRow.ContinentId.Value)?.Name.Value
            ?? currentRow.ContinentId.Value.ToString();
        var keepLabel = $"— Mantener continente ({currentContName}) —";
        var contChoices = new List<string> { keepLabel };
        contChoices.AddRange(continentsList.Select(c => c.Name.Value));
        contChoices.Add(ConsoleMenuHelpers.VolverAlMenu);

        AnsiConsole.WriteLine();
        var contPick = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[deepskyblue1]Continente[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices(contChoices)
        );

        if (contPick == ConsoleMenuHelpers.VolverAlMenu)
        {
            Pause();
            return;
        }

        var finalContinent = contPick == keepLabel
            ? 0
            : continentsList.First(c => c.Name.Value == contPick).Id.Value;

        var finalName = newName.Trim();
        var finalIso = newIso.Trim().ToUpper();

        AnsiConsole.WriteLine();
        var preview = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[grey]Campo[/]")
            .AddColumn("[grey]Valor[/]")
            .AddRow("Nombre", $"[white]{finalName}[/]")
            .AddRow("ISO", $"[deepskyblue1]{finalIso}[/]")
            .AddRow(
                "Continente",
                finalContinent > 0
                    ? $"[white]{continentsList.First(c => c.Id.Value == finalContinent).Name.Value}[/]"
                    : $"[grey]sin cambios ({currentContName})[/]");

        AnsiConsole.Write(preview);

        var confirm = ConsoleMenuHelpers.PromptSaveCancelOrBack("\n¿Confirmar actualización?");
        if (confirm == ConsoleMenuHelpers.SaveChoice.VolverAlMenu)
        {
            Pause();
            return;
        }

        if (confirm == ConsoleMenuHelpers.SaveChoice.Guardar)
        {
            try
            {
                await AnsiConsole.Status()
                    .StartAsync("Actualizando…", async _ =>
                        await _service.UpdateAsync(currentIsoCode, finalName, finalIso, finalContinent));

                AnsiConsole.MarkupLine("\n[green]País actualizado correctamente.[/]");
            }
            catch (InvalidOperationException ex)
            {
                AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
            }
        }
        else
            AnsiConsole.MarkupLine("\n[yellow]Operación cancelada.[/]");

        Pause();
    }

    private async Task DeleteAsync()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("[bold]Eliminar país[/]").LeftJustified());
        AnsiConsole.WriteLine();

        var method = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[deepskyblue1]Criterio[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices("Por nombre (lista)", "Por código ISO", ConsoleMenuHelpers.VolverAlMenu)
        );

        if (method == ConsoleMenuHelpers.VolverAlMenu)
            return;

        string? deleteByIso = null;

        if (method == "Por nombre (lista)")
        {
            var countries = (await _service.GetAllAsync()).ToList();

            if (countries.Count == 0)
            {
                AnsiConsole.MarkupLine("\n[yellow]No hay países.[/]");
                Pause();
                return;
            }

            // OJO: SelectionPrompt interpreta strings como markup; evitamos [] que rompen el render.
            var choices = countries.Select(c => $"{c.Name.Value}  ({c.IsoCode.Value})").ToList();
            choices.Add(ConsoleMenuHelpers.VolverAlMenu);

            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[deepskyblue1]Seleccione el país[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .PageSize(10)
                    .AddChoices(choices)
            );

            if (selected == ConsoleMenuHelpers.VolverAlMenu)
                return;

            var match = countries.FirstOrDefault(c =>
                string.Equals($"{c.Name.Value}  ({c.IsoCode.Value})", selected, StringComparison.Ordinal));

            if (match is null)
            {
                AnsiConsole.MarkupLine("[red]No se pudo identificar el país seleccionado. Intente de nuevo.[/]");
                Pause();
                return;
            }

            deleteByIso = match.IsoCode.Value;
        }
        else
        {
            var isoInput = ConsoleMenuHelpers.PromptRequiredStringOrBack(
                "[deepskyblue1]Código ISO:[/]",
                c =>
                {
                    var cleaned = c.ToUpper();
                        return cleaned.Length is >= 2 and <= 3 && cleaned.All(char.IsLetter)
                        ? null
                        : "Código ISO inválido.";
                });

            if (isoInput is null)
            {
                AnsiConsole.MarkupLine("[yellow]Operación cancelada.[/]");
                Pause();
                return;
            }

            deleteByIso = isoInput.ToUpper();
        }

        var targetLabel = deleteByIso ?? "";

        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Panel($"[bold red]{Markup.Escape(targetLabel)}[/]")
            .Header("[red]Confirmar eliminación[/]")
            .BorderColor(Color.Red));

        AnsiConsole.MarkupLine("\n[grey]Si existen regiones o ciudades ligadas a este país, la base de datos puede rechazar el borrado.[/]");

        var confirm = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\n¿Confirma eliminar?")
                .HighlightStyle(new Style(foreground: Color.Red))
                .AddChoices("Sí, eliminar", "No, cancelar", ConsoleMenuHelpers.VolverAlMenu)
        );

        if (confirm == "No, cancelar" || confirm == ConsoleMenuHelpers.VolverAlMenu)
        {
            AnsiConsole.MarkupLine("\n[yellow]Operación cancelada.[/]");
            Pause();
            return;
        }

            try
            {
                await AnsiConsole.Status()
                .StartAsync("Eliminando…", async _ =>
                {
                    await _service.DeleteByIsoCodeAsync(deleteByIso!);
                });

            AnsiConsole.MarkupLine("\n[green]País eliminado.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task<int?> PromptPickContinentIdAsync()
    {
        var continents = (await _continents.GetAllAsync()).OrderBy(c => c.Name.Value).ToList();
        if (continents.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No hay continentes registrados.[/]");
            return null;
        }

        var choices = continents.Select(c => c.Name.Value).Append(ConsoleMenuHelpers.VolverAlMenu).ToList();
        var pick = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[deepskyblue1]Seleccione el continente[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices(choices));

        if (pick == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return continents.First(c => c.Name.Value == pick).Id.Value;
    }

    private static void RenderCountryTable(
        IReadOnlyList<GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.Aggregate.Country> countries)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn(new TableColumn("[bold grey]#[/]").Centered())
            .AddColumn(new TableColumn("[bold grey]Nombre[/]"))
            .AddColumn(new TableColumn("[bold grey]ISO[/]").Centered());

        for (var i = 0; i < countries.Count; i++)
            table.AddRow(
                $"[grey]{i + 1}[/]",
                $"[white]{countries[i].Name.Value}[/]",
                $"[deepskyblue1]{countries[i].IsoCode.Value}[/]"
            );

        AnsiConsole.WriteLine();
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine($"\n[grey]Total: {countries.Count}[/]");
    }

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Pulse Enter para continuar…[/]").AllowEmpty());
    }
}
