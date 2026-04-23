using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Session;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.UI;

public class PaymentStatusesConsoleUI
{
    private readonly IPaymentStatusesService _service;

    public PaymentStatusesConsoleUI(IPaymentStatusesService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Estados de Pago").Color(Color.Blue));

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Selecciona una opción:[/]")
                    .AddChoices(GetMenuOptions()));

            switch (option)
            {
                case "Listar estados de pago":
                    await ListAllAsync();
                    break;
                case "Buscar por ID":
                    await GetByIdAsync();
                    break;
                case "Crear estado de pago":
                    await CreateAsync();
                    break;
                case "Actualizar estado de pago":
                    await UpdateAsync();
                    break;
                case "Eliminar estado de pago":
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
            "Listar estados de pago",
            "Buscar por ID"
        };

        if (UserSession.Current?.IsAdmin == true)
        {
            options.Add("Crear estado de pago");
            options.Add("Actualizar estado de pago");
            options.Add("Eliminar estado de pago");
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
            AnsiConsole.MarkupLine("[grey]No hay estados de pago registrados.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[blue]ID[/]")
            .AddColumn("[blue]Nombre[/]");

        foreach (var item in list)
            table.AddRow(item.Id.Value.ToString(), item.Name.Value);

        AnsiConsole.Write(table);
    }

    private async Task GetByIdAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Buscar estado de pago por ID[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null) return;

        try
        {
            var item = await _service.GetByIdAsync(id.Value);

            if (item is null)
            {
                AnsiConsole.MarkupLine("[red]Estado de pago no encontrado.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[blue]Campo[/]")
                .AddColumn("[blue]Valor[/]");

            table.AddRow("ID",     item.Id.Value.ToString());
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
        AnsiConsole.MarkupLine("[bold blue]Crear nuevo estado de pago[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null) return;
        var name = ConsoleMenuHelpers.PromptRequiredStringOrBack("[yellow]Nombre:[/]");
        if (name is null) return;

        await AnsiConsole.Status()
            .StartAsync("Creando estado de pago...", async ctx =>
            {
                try
                {
                    var item = await _service.CreateAsync(id.Value, name);
                    AnsiConsole.MarkupLine($"[green]Estado de pago '[bold]{item.Name.Value}[/]' creado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task UpdateAsync()
    {
        AnsiConsole.MarkupLine("[bold yellow]Actualizar estado de pago[/]");

        var existing = await PromptPaymentStatusSelectionAsync("[yellow]Seleccione el estado de pago a actualizar:[/]");
        if (existing is null)
            return;

        AnsiConsole.MarkupLine($"[grey]Nombre actual: {existing.Name.Value}[/]");

        var newName = ConsoleMenuHelpers.PromptStringWithInitialOrBack("[yellow]Nuevo nombre:[/]", existing.Name.Value, allowEmpty: false);
        if (newName is null) return;

        await AnsiConsole.Status()
            .StartAsync("Actualizando estado de pago...", async ctx =>
            {
                try
                {
                    var updated = await _service.UpdateAsync(
                        existing.Id.Value,
                        string.IsNullOrWhiteSpace(newName) ? null : newName);

                    AnsiConsole.MarkupLine($"[green]Estado de pago '[bold]{updated.Name.Value}[/]' actualizado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task DeleteAsync()
    {
        AnsiConsole.MarkupLine("[bold red]Eliminar estado de pago[/]");

        var existing = await PromptPaymentStatusSelectionAsync("[yellow]Seleccione el estado de pago a eliminar:[/]");
        if (existing is null) return;

        var confirm = AnsiConsole.Confirm($"[red]¿Estás seguro de eliminar el estado de pago '{Markup.Escape(existing.Name.Value)}'?[/]");

        if (!confirm)
        {
            AnsiConsole.MarkupLine("[grey]Operación cancelada.[/]");
            return;
        }

        await AnsiConsole.Status()
            .StartAsync("Eliminando estado de pago...", async ctx =>
            {
                try
                {
                    var deleted = await _service.DeleteAsync(existing.Id.Value);

                    if (deleted)
                        AnsiConsole.MarkupLine("[green]Estado de pago eliminado correctamente.[/]");
                    else
                        AnsiConsole.MarkupLine("[red]Estado de pago no encontrado.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task<dynamic?> PromptPaymentStatusSelectionAsync(string title)
    {
        var items = (await _service.GetAllAsync()).OrderBy(x => x.Name.Value).ToList();
        if (!items.Any())
        {
            AnsiConsole.MarkupLine("[grey]No hay estados de pago registrados.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(items.Select(FormatPaymentStatusChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return items.First(x => FormatPaymentStatusChoice(x) == selected);
    }

    private static string FormatPaymentStatusChoice(dynamic item) =>
        $"{item.Id.Value} · {Markup.Escape(item.Name.Value)}";
}
