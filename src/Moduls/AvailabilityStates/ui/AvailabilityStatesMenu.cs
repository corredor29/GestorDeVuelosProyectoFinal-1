using System.Linq;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.ui;

public sealed class AvailabilityStatesMenu : IModuleUI
{
    private readonly IAvailabilityStatesService _service;

    public string Key => "availability_statuses";
    public string Title => "📅  Gestión de Estados de Disponibilidad";

    public AvailabilityStatesMenu(IAvailabilityStatesService service)
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
                        "1. Listar todos los estados",
                        "2. Buscar por ID",
                        "3. Buscar por Nombre",
                        "4. Buscar por ID de personal",
                        "5. Registrar nuevo Estado",
                        "6. Actualizar Estado",
                        "7. Eliminar Estado",
                        "0. Volver al menú principal"
                    }));

            if (option.StartsWith("0"))
                break;

            switch (option[0])
            {
                case '1': await ListAllAsync(); break;
                case '2': await SearchByIdAsync(); break;
                case '3': await SearchByNameAsync(); break;
                case '4': await SearchByStaffIdAsync(); break;
                case '5': await CreateAsync(); break;
                case '6': await UpdateAsync(); break;
                case '7': await DeleteMenuAsync(); break;
            }
        }
    }

    private async Task ListAllAsync()
    {
        var items = await _service.GetAllAsync();
        ShowTable(items, "Todos los Estados de Disponibilidad");
        Pause();
    }

    private async Task SearchByIdAsync()
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("Ingrese el [green]ID[/] del estado:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value);

        if (item == null)
            AnsiConsole.MarkupLine("[red]❌ No se encontró ningún estado con ese ID.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para ID: {id.Value}");

        Pause();
    }

    private async Task SearchByNameAsync()
    {
        var name = ConsoleMenuHelpers.PromptStringWithInitialOrBack(
            "Ingrese el [green]nombre[/] del estado [dim](mínimo 2 caracteres)[/]:",
            string.Empty,
            allowEmpty: true);

        if (name is null)
            return;

        var trimmed = name.Trim();
        if (trimmed.Length < 2)
        {
            AnsiConsole.MarkupLine(
                "[yellow]Use al menos 2 letras del nombre. Si solo tiene el número, elija «Buscar por ID».[/]");
            Pause();
            return;
        }

        if (trimmed.All(char.IsDigit))
        {
            AnsiConsole.MarkupLine(
                "[yellow]Eso parece un número: use la opción «Buscar por ID» en el menú anterior.[/]");
            Pause();
            return;
        }

        try
        {
            var item = await _service.GetByNameAsync(trimmed);

            if (item == null)
                AnsiConsole.MarkupLine("[red]❌ No se encontró ningún estado con ese nombre.[/]");
            else
                ShowTable(new[] { item }, $"Resultado para: {trimmed}");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Entrada no válida: {Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task SearchByStaffIdAsync()
    {
        var staffId = ConsoleMenuHelpers.PromptPositiveIntOrBack("Ingrese el [green]ID del personal[/]:");
        if (staffId is null)
            return;

        var items = await _service.GetByStaffIdAsync(staffId.Value);
        ShowSearchResults(items, $"Estados de disponibilidad para el personal #{staffId.Value}");
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Registrar Nuevo Estado de Disponibilidad[/]");
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre del estado:");
        if (name is null)
        {
            Pause();
            return;
        }

        if (AnsiConsole.Confirm("¿Desea guardar los cambios?"))
        {
            try
            {
                await _service.CreateAsync(name);
                AnsiConsole.MarkupLine("[green]✅ Estado registrado correctamente.[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
            }
        }

        Pause();
    }

    private async Task UpdateAsync()
    {
        var states = (await _service.GetAllAsync()).OrderBy(x => x.Name.Value).ToList();
        if (states.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay estados registrados.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione el estado a modificar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(states.Select(FormatStateChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var item = states.First(x => FormatStateChoice(x) == selected);
        var id = item.Id!.Value;

        var newName = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nuevo nombre:", item.Name.Value);
        if (newName is null)
        {
            Pause();
            return;
        }

        try
        {
            await _service.UpdateAsync(id, newName);
            AnsiConsole.MarkupLine("[green]✅ Estado actualizado.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private async Task DeleteMenuAsync()
    {
        var states = (await _service.GetAllAsync()).OrderBy(x => x.Name.Value).ToList();
        if (states.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay estados registrados.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red]Seleccione el estado a eliminar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(states.Select(FormatStateChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var state = states.First(x => FormatStateChoice(x) == selected);

        try
        {
            if (AnsiConsole.Confirm($"[red]¿Está seguro de eliminar el estado {Markup.Escape(state.Name.Value)}?[/]"))
            {
                await _service.DeleteByIdAsync(state.Id!.Value);
                AnsiConsole.MarkupLine("[green]✅ Operación procesada.[/]");
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private static void ShowSearchResults(IEnumerable<AvailabilityState> items, string title)
    {
        var list = items.ToList();
        if (list.Count == 0)
            AnsiConsole.MarkupLine("[red]❌ No se encontraron registros.[/]");
        else
            ShowTable(list, title);

        Pause();
    }

    private static void ShowTable(IEnumerable<AvailabilityState> items, string title)
    {
        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .AddColumn("[yellow]ID[/]")
            .AddColumn("[green]Nombre[/]");

        foreach (var item in items)
            table.AddRow((item.Id?.Value ?? 0).ToString(), item.Name.Value);

        AnsiConsole.Write(table);
    }

    private static void Pause()
    {
        AnsiConsole.MarkupLine("\nPresione [grey]ENTER[/] para continuar...");
        Console.ReadLine();
    }

    private static string FormatStateChoice(AvailabilityState item) =>
        $"{item.Id?.Value ?? 0} · {Markup.Escape(item.Name.Value)}";
}
