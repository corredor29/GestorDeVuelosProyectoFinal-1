using Spectre.Console;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Session;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.UI;

public class InvoiceItemTypesConsoleUI
{
    private readonly IInvoiceItemTypesService _service;

    public InvoiceItemTypesConsoleUI(IInvoiceItemTypesService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Tipos de Item Factura").Color(Color.Blue));

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Selecciona una opción:[/]")
                    .AddChoices(GetMenuOptions()));

            switch (option)
            {
                case "Listar tipos de artículo":
                    await ListAllAsync();
                    break;
                case "Buscar por ID":
                    await GetByIdAsync();
                    break;
                case "Crear tipo de artículo":
                    await CreateAsync();
                    break;
                case "Actualizar tipo de artículo":
                    await UpdateAsync();
                    break;
                case "Eliminar tipo de artículo":
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
            "Listar tipos de artículo",
            "Buscar por ID"
        };

        if (UserSession.Current?.IsAdmin == true)
        {
            options.Add("Crear tipo de artículo");
            options.Add("Actualizar tipo de artículo");
            options.Add("Eliminar tipo de artículo");
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
            AnsiConsole.MarkupLine("[grey]No hay tipos de artículo registrados.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[blue]ID[/]")
            .AddColumn("[blue]Nombre[/]");

        foreach (var item in list)
        {
            table.AddRow(
                item.Id.Value.ToString(),
                item.Name.Value
            );
        }

        AnsiConsole.Write(table);
    }

    private async Task GetByIdAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Buscar tipo de artículo por ID[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null) return;

        try
        {
            var item = await _service.GetByIdAsync(id.Value);

            if (item is null)
            {
                AnsiConsole.MarkupLine("[red]Tipo de artículo no encontrado.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[blue]Campo[/]")
                .AddColumn("[blue]Valor[/]");

            table.AddRow("ID", item.Id.Value.ToString());
            table.AddRow("Nombre", item.Name.Value);

            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
        }
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Crear nuevo tipo de artículo[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null) return;
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("[yellow]Nombre:[/]");
        if (name is null) return;

        await AnsiConsole.Status()
            .StartAsync("Creando tipo de artículo...", async ctx =>
            {
                try
                {
                    var item = await _service.CreateAsync(id.Value, name);
                    AnsiConsole.MarkupLine($"[green]Tipo de artículo '[bold]{item.Name.Value}[/]' creado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task UpdateAsync()
    {
        AnsiConsole.MarkupLine("[bold yellow]Actualizar tipo de artículo[/]");

        var existing = await PromptInvoiceItemTypeSelectionAsync("[yellow]Seleccione el tipo de artículo a actualizar:[/]");
        if (existing is null)
            return;

        AnsiConsole.MarkupLine($"[grey]Nombre actual: {existing.Name.Value}[/]");

        var newName = ConsoleMenuHelpers.PromptStringWithInitialOrBack("[yellow]Nuevo nombre:[/]", existing.Name.Value, allowEmpty: false);
        if (newName is null) return;

        await AnsiConsole.Status()
            .StartAsync("Actualizando tipo de artículo...", async ctx =>
            {
                try
                {
                    var updated = await _service.UpdateAsync(existing.Id.Value, newName);
                    AnsiConsole.MarkupLine($"[green]Tipo de artículo '[bold]{updated.Name.Value}[/]' actualizado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task DeleteAsync()
    {
        AnsiConsole.MarkupLine("[bold red]Eliminar tipo de artículo[/]");

        var existing = await PromptInvoiceItemTypeSelectionAsync("[yellow]Seleccione el tipo de artículo a eliminar:[/]");
        if (existing is null) return;

        var confirm = AnsiConsole.Confirm($"[red]¿Estás seguro de eliminar el tipo de artículo '{Markup.Escape(existing.Name.Value)}'?[/]");

        if (!confirm)
        {
            AnsiConsole.MarkupLine("[grey]Operación cancelada.[/]");
            return;
        }

        await AnsiConsole.Status()
            .StartAsync("Eliminando tipo de artículo...", async ctx =>
            {
                try
                {
                    var deleted = await _service.DeleteAsync(existing.Id.Value);

                    if (deleted)
                        AnsiConsole.MarkupLine("[green]Tipo de artículo eliminado correctamente.[/]");
                    else
                        AnsiConsole.MarkupLine("[red]Tipo de artículo no encontrado.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task<dynamic?> PromptInvoiceItemTypeSelectionAsync(string title)
    {
        var items = (await _service.GetAllAsync()).OrderBy(x => x.Name.Value).ToList();
        if (!items.Any())
        {
            AnsiConsole.MarkupLine("[grey]No hay tipos de artículo registrados.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(items.Select(FormatInvoiceItemTypeChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return items.First(x => FormatInvoiceItemTypeChoice(x) == selected);
    }

    private static string FormatInvoiceItemTypeChoice(dynamic item) =>
        $"{item.Id.Value} · {Markup.Escape(item.Name.Value)}";
}
