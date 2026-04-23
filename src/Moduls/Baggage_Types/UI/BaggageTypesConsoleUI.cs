using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Session;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.UI;

public class BaggageTypesConsoleUI
{
    private readonly IBaggageTypesService _service;

    public BaggageTypesConsoleUI(IBaggageTypesService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Tipos de Equipaje").Color(Color.Blue));

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Selecciona una opción:[/]")
                    .AddChoices(GetMenuOptions()));

            switch (option)
            {
                case "Listar tipos de equipaje":
                    await ListAllAsync();
                    break;
                case "Buscar por ID":
                    await GetByIdAsync();
                    break;
                case "Crear tipo de equipaje":
                    await CreateAsync();
                    break;
                case "Actualizar tipo de equipaje":
                    await UpdateAsync();
                    break;
                case "Eliminar tipo de equipaje":
                    await DeleteAsync();
                    break;
                case "Volver":
                    return;
            }

            AnsiConsole.WriteLine();
            AnsiConsole.Markup("[grey]Presiona cualquier tecla para continuar...[/]");
            Console.ReadKey();
        }
    }

    private IEnumerable<string> GetMenuOptions()
    {
        var options = new List<string>
        {
            "Listar tipos de equipaje",
            "Buscar por ID"
        };

        if (UserSession.Current?.IsAdmin == true)
        {
            options.Add("Crear tipo de equipaje");
            options.Add("Actualizar tipo de equipaje");
            options.Add("Eliminar tipo de equipaje");
        }

        options.Add("Volver");
        return options;
    }

    private async Task ListAllAsync()
    {
        var items = await _service.GetAllAsync();

        var list = items.ToList();

        if (!list.Any())
        {
            AnsiConsole.MarkupLine("[grey]No hay tipos de equipaje registrados.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[blue]ID[/]")
            .AddColumn("[blue]Nombre[/]")
            .AddColumn("[blue]Peso máx. (kg)[/]")
            .AddColumn("[blue]Precio base[/]");

        foreach (var item in list)
        {
            table.AddRow(
                item.Id.Value.ToString(),
                item.Name.Value,
                item.MaxWeightKg.Value.ToString("F2"),
                item.BasePrice.Value.ToString("F2")
            );
        }

        AnsiConsole.Write(table);
    }

    private async Task GetByIdAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Buscar tipo de equipaje por ID[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null)
            return;

        try
        {
            var item = await _service.GetByIdAsync(id.Value);

            if (item is null)
            {
                AnsiConsole.MarkupLine("[red]Tipo de equipaje no encontrado.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[blue]Campo[/]")
                .AddColumn("[blue]Valor[/]");

            table.AddRow("ID", item.Id.Value.ToString());
            table.AddRow("Nombre", item.Name.Value);
            table.AddRow("Peso máx. (kg)", item.MaxWeightKg.Value.ToString("F2"));
            table.AddRow("Precio base", item.BasePrice.Value.ToString("F2"));

            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
        }
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Crear nuevo tipo de equipaje[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null)
            return;

        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("[yellow]Nombre:[/]");
        if (name is null)
            return;

        var maxWeight = ConsoleMenuHelpers.PromptDecimalOrBack("[yellow]Peso máximo (kg):[/]");
        if (maxWeight is null)
            return;

        var basePrice = ConsoleMenuHelpers.PromptDecimalOrBack("[yellow]Precio base:[/]");
        if (basePrice is null)
            return;

        await AnsiConsole.Status()
            .StartAsync("Creando tipo de equipaje...", async ctx =>
            {
                try
                {
                    var item = await _service.CreateAsync(id.Value, name, maxWeight.Value, basePrice.Value);
                    AnsiConsole.MarkupLine($"[green]Tipo de equipaje '[bold]{item.Name.Value}[/]' creado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task UpdateAsync()
    {
        AnsiConsole.MarkupLine("[bold yellow]Actualizar tipo de equipaje[/]");

        var baggageTypes = (await _service.GetAllAsync()).OrderBy(x => x.Id.Value).ToList();
        if (baggageTypes.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]No hay tipos de equipaje registrados.[/]");
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione el tipo de equipaje a actualizar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(baggageTypes.Select(b => $"{b.Id.Value} · {Markup.Escape(b.Name.Value)}").Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var existing = baggageTypes.First(b => $"{b.Id.Value} · {Markup.Escape(b.Name.Value)}" == selected);
        var id = existing.Id.Value;

        AnsiConsole.MarkupLine($"[grey]Nombre actual: {existing.Name.Value}[/]");
        AnsiConsole.MarkupLine($"[grey]Peso máximo actual: {existing.MaxWeightKg.Value:F2} kg[/]");
        AnsiConsole.MarkupLine($"[grey]Precio base actual: {existing.BasePrice.Value:F2}[/]");

        var newName = ConsoleMenuHelpers.PromptStringWithInitialOrBack("[yellow]Nuevo nombre (Enter para mantener):[/]", string.Empty, allowEmpty: true);
        if (newName is null)
            return;

        var newMaxWeightInput = ConsoleMenuHelpers.PromptStringWithInitialOrBack("[yellow]Nuevo peso máximo (Enter para mantener):[/]", string.Empty, allowEmpty: true);
        if (newMaxWeightInput is null)
            return;

        var newBasePriceInput = ConsoleMenuHelpers.PromptStringWithInitialOrBack("[yellow]Nuevo precio base (Enter para mantener):[/]", string.Empty, allowEmpty: true);
        if (newBasePriceInput is null)
            return;

        decimal? newMaxWeight = null;
        if (!string.IsNullOrWhiteSpace(newMaxWeightInput))
        {
            if (!decimal.TryParse(newMaxWeightInput.Trim(), out var parsed))
            {
                AnsiConsole.MarkupLine("[red]Peso máximo inválido.[/]");
                return;
            }
            newMaxWeight = parsed;
        }

        decimal? newBasePrice = null;
        if (!string.IsNullOrWhiteSpace(newBasePriceInput))
        {
            if (!decimal.TryParse(newBasePriceInput.Trim(), out var parsed))
            {
                AnsiConsole.MarkupLine("[red]Precio base inválido.[/]");
                return;
            }
            newBasePrice = parsed;
        }

        await AnsiConsole.Status()
            .StartAsync("Actualizando tipo de equipaje...", async ctx =>
            {
                try
                {
                    var updated = await _service.UpdateAsync(
                        id,
                        string.IsNullOrWhiteSpace(newName) ? null : newName,
                        newMaxWeight,
                        newBasePrice
                    );
                    AnsiConsole.MarkupLine($"[green]Tipo de equipaje '[bold]{updated.Name.Value}[/]' actualizado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task DeleteAsync()
    {
        AnsiConsole.MarkupLine("[bold red]Eliminar tipo de equipaje[/]");

        var baggageTypes = (await _service.GetAllAsync()).OrderBy(x => x.Id.Value).ToList();
        if (baggageTypes.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]No hay tipos de equipaje registrados.[/]");
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione el tipo de equipaje a eliminar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(baggageTypes.Select(b => $"{b.Id.Value} · {Markup.Escape(b.Name.Value)}").Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var baggageType = baggageTypes.First(b => $"{b.Id.Value} · {Markup.Escape(b.Name.Value)}" == selected);

        var confirm = AnsiConsole.Confirm($"[red]¿Estás seguro de eliminar el tipo de equipaje con ID {baggageType.Id.Value}?[/]");

        if (!confirm)
        {
            AnsiConsole.MarkupLine("[grey]Operación cancelada.[/]");
            return;
        }

        await AnsiConsole.Status()
            .StartAsync("Eliminando tipo de equipaje...", async ctx =>
            {
                try
                {
                    var deleted = await _service.DeleteAsync(baggageType.Id.Value);

                    if (deleted)
                        AnsiConsole.MarkupLine("[green]Tipo de equipaje eliminado correctamente.[/]");
                    else
                        AnsiConsole.MarkupLine("[red]Tipo de equipaje no encontrado.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }
}
