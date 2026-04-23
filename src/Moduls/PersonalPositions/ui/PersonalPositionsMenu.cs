using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.ui;

public sealed class PersonalPositionsMenu : IModuleUI
{
    private readonly IPersonalPositionsService _service;

    public string Key => "staff_positions";
    public string Title => "🧑‍✈️  Gestión de Cargos del Personal";

    public PersonalPositionsMenu(IPersonalPositionsService service)
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
                        "1. Listar todos los cargos",
                        "2. Buscar por ID",
                        "3. Buscar por Nombre",
                        "4. Registrar nuevo Cargo",
                        "5. Actualizar Cargo",
                        "6. Eliminar Cargo",
                        "0. Volver al menú principal"
                    }));

            if (option.StartsWith("0"))
                break;

            switch (option[0])
            {
                case '1': await ListAllAsync(); break;
                case '2': await SearchByIdAsync(); break;
                case '3': await SearchByNameAsync(); break;
                case '4': await CreateAsync(); break;
                case '5': await UpdateAsync(); break;
                case '6': await DeleteMenuAsync(); break;
            }
        }
    }

    private async Task ListAllAsync()
    {
        var items = await _service.GetAllAsync();
        ShowTable(items, "Todos los Cargos");
        Pause();
    }

    private async Task SearchByIdAsync()
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("Ingrese el [green]ID[/] del cargo:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value);

        if (item == null)
            AnsiConsole.MarkupLine("[red]❌ No se encontró ningún cargo con ese ID.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para ID: {id.Value}");

        Pause();
    }

    private async Task SearchByNameAsync()
    {
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Ingrese el [green]nombre[/] del cargo:");
        if (name is null)
            return;

        var item = await _service.GetByNameAsync(name);

        if (item == null)
            AnsiConsole.MarkupLine("[red]❌ No se encontró ningún cargo con ese nombre.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para: {name}");

        Pause();
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Registrar Nuevo Cargo[/]");
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre del cargo:");
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
                AnsiConsole.MarkupLine("[green]✅ Cargo registrado correctamente.[/]");
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
        var positions = (await _service.GetAllAsync()).OrderBy(x => x.Name.Value).ToList();
        if (positions.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay cargos registrados.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione el cargo a modificar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(positions.Select(FormatPositionChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var item = positions.First(x => FormatPositionChoice(x) == selected);
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
            AnsiConsole.MarkupLine("[green]✅ Cargo actualizado.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private async Task DeleteMenuAsync()
    {
        var positions = (await _service.GetAllAsync()).OrderBy(x => x.Name.Value).ToList();
        if (positions.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay cargos registrados.[/]");
            Pause();
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red]Seleccione el cargo a eliminar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(positions.Select(FormatPositionChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var item = positions.First(x => FormatPositionChoice(x) == selected);

        try
        {
            if (AnsiConsole.Confirm($"[red]¿Está seguro de eliminar el cargo {Markup.Escape(item.Name.Value)}?[/]"))
            {
                await _service.DeleteByIdAsync(item.Id!.Value);
                AnsiConsole.MarkupLine("[green]✅ Operación procesada.[/]");
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]❌ {ex.Message}[/]");
        }

        Pause();
    }

    private static void ShowTable(IEnumerable<PersonalPosition> items, string title)
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

    private static string FormatPositionChoice(PersonalPosition item) =>
        $"{item.Id?.Value ?? 0} · {Markup.Escape(item.Name.Value)}";
}
