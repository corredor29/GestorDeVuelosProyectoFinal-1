using GestorDeVuelosProyectoFinal.Moduls.Seasons.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.Moduls.Seasons.ui;

public sealed class SeasonsMenu : IModuleUI
{
    private readonly ISeasonsService _service;

    public string Key => "seasons";
    public string Title => "Gestión de Temporadas";

    public SeasonsMenu(ISeasonsService service)
    {
        _service = service;
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
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
                        "1. Listar todas las temporadas",
                        "2. Buscar por ID",
                        "3. Buscar por nombre",
                        "4. Registrar temporada",
                        "5. Actualizar temporada",
                        "6. Eliminar temporada",
                        "0. Volver al menú principal"
                    }));

            if (option.StartsWith("0"))
                break;

            switch (option.Split('.')[0])
            {
                case "1": await ListAllAsync(); break;
                case "2": await SearchByIdAsync(); break;
                case "3": await SearchByNameAsync(); break;
                case "4": await CreateAsync(); break;
                case "5": await UpdateAsync(); break;
                case "6": await DeleteMenuAsync(); break;
            }
        }
    }

    private async Task ListAllAsync()
    {
        var items = await _service.GetAllAsync();
        ShowTable(items, "Todas las temporadas");
        Pause();
    }

    private async Task SearchByIdAsync()
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("Ingrese el [green]ID[/] de la temporada:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value);

        if (item is null)
            AnsiConsole.MarkupLine("[red]No se encontró ninguna temporada con ese ID.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para ID: {id.Value}");

        Pause();
    }

    private async Task SearchByNameAsync()
    {
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Ingrese el [green]nombre[/] de la temporada:");
        if (name is null)
            return;

        var item = await _service.GetByNameAsync(name);

        if (item is null)
            AnsiConsole.MarkupLine("[red]No se encontró ninguna temporada con ese nombre.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para: {name}");

        Pause();
    }

    private async Task CreateAsync()
    {
        try
        {
            AnsiConsole.MarkupLine("[bold blue]Registrar temporada[/]");
            var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre:");
            if (name is null)
            {
                Pause();
                return;
            }

            var description = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Descripción (opcional):", string.Empty, allowEmpty: true);
            if (description is null)
            {
                Pause();
                return;
            }

            var priceFactor = PromptDecimalOrBack("Factor de precio:");
            if (priceFactor is null)
            {
                Pause();
                return;
            }

            if (AnsiConsole.Confirm("Desea guardar los cambios?"))
            {
                await _service.CreateAsync(name, string.IsNullOrWhiteSpace(description) ? null : description, priceFactor.Value);
                AnsiConsole.MarkupLine("[green]Temporada registrada correctamente.[/]");
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
        var seasons = (await _service.GetAllAsync()).OrderBy(x => x.Name.Value).ToList();
        if (seasons.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay temporadas registradas.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione la temporada a modificar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(seasons.Select(FormatSeasonChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var item = seasons.First(x => FormatSeasonChoice(x) == selected);
        var id = item.Id!.Value;

        try
        {
            var name = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nuevo nombre:", item.Name.Value);
            if (name is null)
            {
                Pause();
                return;
            }

            var description = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nueva descripción (opcional):", item.Description.Value ?? string.Empty, allowEmpty: true);
            if (description is null)
            {
                Pause();
                return;
            }

            var priceFactor = PromptDecimalWithInitialOrBack("Nuevo factor de precio:", item.PriceFactor.Value);
            if (priceFactor is null)
            {
                Pause();
                return;
            }

            await _service.UpdateAsync(id, name, string.IsNullOrWhiteSpace(description) ? null : description, priceFactor.Value);
            AnsiConsole.MarkupLine("[green]Temporada actualizada.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }

        Pause();
    }

    private async Task DeleteMenuAsync()
    {
        var seasons = (await _service.GetAllAsync()).OrderBy(x => x.Name.Value).ToList();
        if (seasons.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay temporadas registradas.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red]Seleccione la temporada a eliminar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(seasons.Select(FormatSeasonChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var season = seasons.First(x => FormatSeasonChoice(x) == selected);

        try
        {
            if (AnsiConsole.Confirm($"¿Estás seguro de eliminar la temporada {season.Name.Value}?"))
            {
                await _service.DeleteByIdAsync(season.Id!.Value);
                AnsiConsole.MarkupLine("[green]Operación procesada.[/]");
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
        }

        Pause();
    }

    private static void ShowTable(IEnumerable<Season> items, string title)
    {
        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .AddColumn("[yellow]ID[/]")
            .AddColumn("[green]Nombre[/]")
            .AddColumn("[blue]Descripción[/]")
            .AddColumn("[green]Factor de precio[/]");

        foreach (var item in items)
        {
            table.AddRow(
                (item.Id?.Value ?? 0).ToString(),
                item.Name.Value,
                item.Description.Value ?? "-",
                item.PriceFactor.Value.ToString("0.0000"));
        }

        AnsiConsole.Write(table);
    }

    private static decimal? PromptDecimalOrBack(string label) =>
        ConsoleMenuHelpers.PromptDecimalOrBack(label);

    private static decimal? PromptDecimalWithInitialOrBack(string label, decimal currentValue)
    {
        var raw = ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            label,
            currentValue.ToString(System.Globalization.CultureInfo.InvariantCulture),
            allowEmpty: false,
            validate: value =>
            {
                if (decimal.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out _))
                    return null;
                if (decimal.TryParse(value, out _))
                    return null;
                return "Número decimal no válido.";
            });

        if (raw is null)
            return null;

        return decimal.TryParse(raw, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var value)
            ? value
            : decimal.Parse(raw);
    }

    private static string FormatSeasonChoice(Season season) =>
        $"{season.Id?.Value ?? 0} · {Markup.Escape(season.Name.Value)}";

    private static void Pause()
    {
        AnsiConsole.MarkupLine("\nPresione [grey]ENTER[/] para continuar...");
        Console.ReadLine();
    }
}
