using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Session;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Baggage.UI;

public class BaggageConsoleUI
{
    private readonly IBaggageService _service;

    public BaggageConsoleUI(IBaggageService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Equipaje").Color(Color.Blue));

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Selecciona una opción:[/]")
                    .AddChoices(GetMenuOptions()));

            switch (option)
            {
                case "Listar equipajes":
                    await ListAllAsync();
                    break;
                case "Buscar por ID":
                    await GetByIdAsync();
                    break;
                case "Crear equipaje":
                    await CreateAsync();
                    break;
                case "Actualizar equipaje":
                    await UpdateAsync();
                    break;
                case "Eliminar equipaje":
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
            "Listar equipajes",
            "Buscar por ID"
        };

        if (UserSession.Current?.IsAdmin == true)
        {
            options.Add("Crear equipaje");
            options.Add("Actualizar equipaje");
            options.Add("Eliminar equipaje");
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
            AnsiConsole.MarkupLine("[grey]No hay equipajes registrados.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[blue]ID[/]")
            .AddColumn("[blue]Check-in ID[/]")
            .AddColumn("[blue]Tipo de equipaje ID[/]")
            .AddColumn("[blue]Peso (kg)[/]")
            .AddColumn("[blue]Precio cobrado[/]");

        foreach (var item in list)
        {
            table.AddRow(
                item.Id.Value.ToString(),
                item.CheckinId.Value.ToString(),
                item.BaggageTypeId.Value.ToString(),
                item.WeightKg.Value.ToString("F2"),
                item.ChargedPrice.Value.ToString("F2")
            );
        }

        AnsiConsole.Write(table);
    }

    private async Task GetByIdAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Buscar equipaje por ID[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null)
            return;

        try
        {
            var item = await _service.GetByIdAsync(id.Value);

            if (item is null)
            {
                AnsiConsole.MarkupLine("[red]Equipaje no encontrado.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[blue]Campo[/]")
                .AddColumn("[blue]Valor[/]");

            table.AddRow("ID",               item.Id.Value.ToString());
            table.AddRow("Check-in ID",      item.CheckinId.Value.ToString());
            table.AddRow("Tipo de equipaje ID", item.BaggageTypeId.Value.ToString());
            table.AddRow("Peso (kg)",        item.WeightKg.Value.ToString("F2"));
            table.AddRow("Precio cobrado",   item.ChargedPrice.Value.ToString("F2"));

            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
        }
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Crear nuevo equipaje[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null)
            return;

        var checkinId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]Check-in ID:[/]");
        if (checkinId is null)
            return;

        var baggageTypeId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]Tipo de equipaje ID:[/]");
        if (baggageTypeId is null)
            return;

        var weightKg = ConsoleMenuHelpers.PromptDecimalOrBack("[yellow]Peso (kg):[/]");
        if (weightKg is null)
            return;

        var chargedPrice = ConsoleMenuHelpers.PromptDecimalOrBack("[yellow]Precio cobrado:[/]");
        if (chargedPrice is null)
            return;

        await AnsiConsole.Status()
            .StartAsync("Creando equipaje...", async ctx =>
            {
                try
                {
                    var item = await _service.CreateAsync(id.Value, checkinId.Value, baggageTypeId.Value, weightKg.Value, chargedPrice.Value);
                    AnsiConsole.MarkupLine($"[green]Equipaje con ID '[bold]{item.Id.Value}[/]' creado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task UpdateAsync()
    {
        AnsiConsole.MarkupLine("[bold yellow]Actualizar equipaje[/]");

        var baggageItems = (await _service.GetAllAsync()).OrderBy(x => x.Id.Value).ToList();
        if (baggageItems.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]No hay equipajes registrados.[/]");
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione el equipaje a actualizar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(baggageItems.Select(b => $"{b.Id.Value} · Check-in {b.CheckinId.Value} · {b.WeightKg.Value:F2} kg").Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var existing = baggageItems.First(b => $"{b.Id.Value} · Check-in {b.CheckinId.Value} · {b.WeightKg.Value:F2} kg" == selected);
        var id = existing.Id.Value;

        AnsiConsole.MarkupLine($"[grey]Peso actual: {existing.WeightKg.Value:F2} kg[/]");
        AnsiConsole.MarkupLine($"[grey]Precio cobrado actual: {existing.ChargedPrice.Value:F2}[/]");

        var newWeightInput = ConsoleMenuHelpers.PromptStringWithInitialOrBack("[yellow]Nuevo peso kg (Enter para mantener):[/]", string.Empty, allowEmpty: true);
        if (newWeightInput is null)
            return;

        var newChargedPriceInput = ConsoleMenuHelpers.PromptStringWithInitialOrBack("[yellow]Nuevo precio cobrado (Enter para mantener):[/]", string.Empty, allowEmpty: true);
        if (newChargedPriceInput is null)
            return;

        decimal? newWeightKg = null;
        if (!string.IsNullOrWhiteSpace(newWeightInput))
        {
            if (!decimal.TryParse(newWeightInput.Trim(), out var parsed))
            {
                AnsiConsole.MarkupLine("[red]Peso inválido.[/]");
                return;
            }
            newWeightKg = parsed;
        }

        decimal? newChargedPrice = null;
        if (!string.IsNullOrWhiteSpace(newChargedPriceInput))
        {
            if (!decimal.TryParse(newChargedPriceInput.Trim(), out var parsed))
            {
                AnsiConsole.MarkupLine("[red]Precio inválido.[/]");
                return;
            }
            newChargedPrice = parsed;
        }

        await AnsiConsole.Status()
            .StartAsync("Actualizando equipaje...", async ctx =>
            {
                try
                {
                    var updated = await _service.UpdateAsync(id, newWeightKg, newChargedPrice);
                    AnsiConsole.MarkupLine($"[green]Equipaje con ID '[bold]{updated.Id.Value}[/]' actualizado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task DeleteAsync()
    {
        AnsiConsole.MarkupLine("[bold red]Eliminar equipaje[/]");

        var baggageItems = (await _service.GetAllAsync()).OrderBy(x => x.Id.Value).ToList();
        if (baggageItems.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]No hay equipajes registrados.[/]");
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione el equipaje a eliminar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(baggageItems.Select(b => $"{b.Id.Value} · Check-in {b.CheckinId.Value} · {b.WeightKg.Value:F2} kg").Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var baggage = baggageItems.First(b => $"{b.Id.Value} · Check-in {b.CheckinId.Value} · {b.WeightKg.Value:F2} kg" == selected);

        var confirm = AnsiConsole.Confirm($"[red]¿Estás seguro de eliminar el equipaje con ID {baggage.Id.Value}?[/]");

        if (!confirm)
        {
            AnsiConsole.MarkupLine("[grey]Operación cancelada.[/]");
            return;
        }

        await AnsiConsole.Status()
            .StartAsync("Eliminando equipaje...", async ctx =>
            {
                try
                {
                    var deleted = await _service.DeleteAsync(baggage.Id.Value);

                    if (deleted)
                        AnsiConsole.MarkupLine("[green]Equipaje eliminado correctamente.[/]");
                    else
                        AnsiConsole.MarkupLine("[red]Equipaje no encontrado.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }
}
