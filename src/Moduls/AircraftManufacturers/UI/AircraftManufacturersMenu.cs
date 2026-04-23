using System.Linq;
using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using AircraftManufacturerAggregate = GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate.AircraftManufacturers;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.UI;

public sealed class AircraftManufacturersMenu : IModuleUI
{
    private readonly IAircraftManufacturersService _service;

    public AircraftManufacturersMenu(IAircraftManufacturersService service)
    {
        _service = service;
    }

    public string Key => "aircraft-manufacturers";
    public string Title => "Fabricantes de Aeronaves";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold yellow]Fabricantes de Aeronaves[/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Seleccione una opción:")
                    .PageSize(10)
                    .AddChoices(
                        "1. Listar todos",
                        "2. Buscar por ID",
                        "3. Buscar por nombre",
                        "4. Buscar por país",
                        "5. Crear fabricante",
                        "6. Actualizar fabricante",
                        "7. Eliminar por ID",
                        "8. Eliminar por nombre",
                        "9. Eliminar por país",
                        "0. Volver"));

            if (option.StartsWith("0"))
                return;

            try
            {
                switch (option.Split('.')[0])
                {
                    case "1": await ListAllAsync(cancellationToken); break;
                    case "2": await SearchByIdAsync(cancellationToken); break;
                    case "3": await SearchByNameAsync(cancellationToken); break;
                    case "4": await SearchByCountryAsync(cancellationToken); break;
                    case "5": await CreateAsync(cancellationToken); break;
                    case "6": await UpdateAsync(cancellationToken); break;
                    case "7": await DeleteByIdAsync(cancellationToken); break;
                    case "8": await DeleteByNameAsync(cancellationToken); break;
                    case "9": await DeleteByCountryAsync(cancellationToken); break;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"\n[red]{ex.Message}[/]");
                Pause();
            }
        }
    }

    private async Task ListAllAsync(CancellationToken cancellationToken)
    {
        var items = await _service.GetAllAsync(cancellationToken);
        ShowTable(items, "Todos los fabricantes");
        Pause();
    }

    private async Task SearchByIdAsync(CancellationToken cancellationToken)
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("Ingrese el [green]ID[/] del fabricante:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value, cancellationToken);

        if (item is null)
            AnsiConsole.MarkupLine("[yellow]No se encontró ningún fabricante con ese ID.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para ID {id.Value}");

        Pause();
    }

    private async Task SearchByNameAsync(CancellationToken cancellationToken)
    {
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Ingrese el [green]nombre[/] del fabricante:");
        if (name is null)
            return;

        var item = await _service.GetByNameAsync(name, cancellationToken);

        if (item is null)
            AnsiConsole.MarkupLine("[yellow]No se encontró ningún fabricante con ese nombre.[/]");
        else
            ShowTable(new[] { item }, $"Resultado para {name}");

        Pause();
    }

    private async Task SearchByCountryAsync(CancellationToken cancellationToken)
    {
        var country = ConsoleMenuHelpers.PromptRequiredStringOrBack("Ingrese el [green]país[/] del fabricante:");
        if (country is null)
            return;

        var items = await _service.GetByCountryAsync(country, cancellationToken);
        ShowTable(items, $"Fabricantes en {country}");
        Pause();
    }

    private async Task CreateAsync(CancellationToken cancellationToken)
    {
        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Registrar fabricante"))
            return;

        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("Nombre del fabricante:");
        if (name is null)
            return;

        var country = ConsoleMenuHelpers.PromptRequiredStringOrBack("País del fabricante:");
        if (country is null)
            return;

        await _service.CreateAsync(name, country, cancellationToken);
        AnsiConsole.MarkupLine("[green]Fabricante creado correctamente.[/]");
        Pause();
    }

    private async Task UpdateAsync(CancellationToken cancellationToken)
    {
        var existing = await PromptManufacturerSelectionAsync("Seleccione el fabricante a actualizar:", cancellationToken);
        if (existing is null)
        {
            return;
        }

        var name = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nuevo nombre:", existing.Name.Value);
        if (name is null)
            return;

        var country = ConsoleMenuHelpers.PromptStringWithInitialOrBack("Nuevo país:", existing.Country.Value);
        if (country is null)
            return;

        try
        {
            await _service.UpdateAsync(existing.Id.Value, name, country, cancellationToken);
            AnsiConsole.MarkupLine("[green]Fabricante actualizado correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task<AircraftManufacturerAggregate?> PromptManufacturerSelectionAsync(string title, CancellationToken cancellationToken)
    {
        var manufacturers = (await _service.GetAllAsync(cancellationToken))
            .OrderBy(m => m.Name.Value)
            .ToList();
        if (manufacturers.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay fabricantes registrados.[/]");
            Pause();
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(15)
                .AddChoices(manufacturers.Select(FormatManufacturerChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return manufacturers.First(m => FormatManufacturerChoice(m) == selected);
    }

    private async Task DeleteByIdAsync(CancellationToken cancellationToken)
    {
        var existing = await PromptManufacturerSelectionAsync("Seleccione el fabricante a eliminar:", cancellationToken);
        if (existing is null)
            return;

        if (!AnsiConsole.Confirm("¿Confirma la eliminación?", false))
        {
            Pause();
            return;
        }

        try
        {
            var ok = await _service.DeleteByIdAsync(existing.Id.Value, cancellationToken);
            if (ok)
                AnsiConsole.MarkupLine("[green]Fabricante eliminado.[/]");
            else
                AnsiConsole.MarkupLine("[yellow]No se eliminó ningún registro (¿existe ese ID?).[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteByNameAsync(CancellationToken cancellationToken)
    {
        var existing = await PromptManufacturerSelectionAsync("Seleccione el fabricante a eliminar:", cancellationToken);
        if (existing is null)
            return;

        if (!AnsiConsole.Confirm("¿Confirma la eliminación?", false))
        {
            Pause();
            return;
        }

        try
        {
            var ok = await _service.DeleteByNameAsync(existing.Name.Value, cancellationToken);
            if (ok)
                AnsiConsole.MarkupLine("[green]Fabricante eliminado.[/]");
            else
                AnsiConsole.MarkupLine("[yellow]No se eliminó ningún registro.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteByCountryAsync(CancellationToken cancellationToken)
    {
        var country = ConsoleMenuHelpers.PromptRequiredStringOrBack("País cuyos fabricantes desea eliminar:");
        if (country is null)
            return;

        if (!AnsiConsole.Confirm("¿Confirma la eliminación masiva?", false))
        {
            Pause();
            return;
        }

        try
        {
            var deleted = await _service.DeleteByCountryAsync(country, cancellationToken);
            AnsiConsole.MarkupLine($"[green]Se eliminaron {deleted} fabricante(s).[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private static void ShowTable(IEnumerable<AircraftManufacturerAggregate> items, string title)
    {
        var list = items.ToList();
        if (list.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay registros para mostrar.[/]");
            return;
        }

        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .AddColumn("[yellow]ID[/]")
            .AddColumn("[green]Nombre[/]")
            .AddColumn("[blue]País[/]");

        foreach (var item in list)
        {
            table.AddRow(
                item.Id.Value.ToString(),
                item.Name.Value,
                item.Country.Value);
        }

        AnsiConsole.Write(table);
    }

    private static void Pause()
    {
        AnsiConsole.MarkupLine("\n[grey]Presione ENTER para continuar...[/]");
        Console.ReadLine();
    }

    private static string FormatManufacturerChoice(AircraftManufacturerAggregate item) =>
        $"{item.Id.Value} · {Markup.Escape(item.Name.Value)} ({Markup.Escape(item.Country.Value)})";
}
