using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Session;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.UI;

public class PaymentMethodsConsoleUI
{
    private readonly IPaymentMethodsService _service;

    public PaymentMethodsConsoleUI(IPaymentMethodsService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Métodos de Pago").Color(Color.Blue));

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Selecciona una opción:[/]")
                    .AddChoices(GetMenuOptions()));

            switch (option)
            {
                case "Listar métodos de pago":
                    await ListAllAsync();
                    break;
                case "Buscar por ID":
                    await GetByIdAsync();
                    break;
                case "Crear método de pago":
                    await CreateAsync();
                    break;
                case "Actualizar método de pago":
                    await UpdateAsync();
                    break;
                case "Eliminar método de pago":
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
            "Listar métodos de pago",
            "Buscar por ID"
        };

        if (UserSession.Current?.IsAdmin == true)
        {
            options.Add("Crear método de pago");
            options.Add("Actualizar método de pago");
            options.Add("Eliminar método de pago");
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
            AnsiConsole.MarkupLine("[grey]No hay métodos de pago registrados.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[blue]ID[/]")
            .AddColumn("[blue]Nombre[/]");

        foreach (var item in list)
            table.AddRow(item.Id.Value.ToString(), item.DisplayName.Value);

        AnsiConsole.Write(table);
    }

    private async Task GetByIdAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Buscar método de pago por ID[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null) return;

        try
        {
            var item = await _service.GetByIdAsync(id.Value);

            if (item is null)
            {
                AnsiConsole.MarkupLine("[red]Método de pago no encontrado.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[blue]Campo[/]")
                .AddColumn("[blue]Valor[/]");

            table.AddRow("ID",     item.Id.Value.ToString());
            table.AddRow("Nombre", item.DisplayName.Value);

            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
        }
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Crear nuevo método de pago[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null) return;
        var displayName = ConsoleMenuHelpers.PromptRequiredStringOrBack("[yellow]Nombre:[/]");
        if (displayName is null) return;

        await AnsiConsole.Status()
            .StartAsync("Creando método de pago...", async ctx =>
            {
                try
                {
                    var item = await _service.CreateAsync(id.Value, displayName);
                    AnsiConsole.MarkupLine($"[green]Método de pago '[bold]{item.DisplayName.Value}[/]' creado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task UpdateAsync()
    {
        AnsiConsole.MarkupLine("[bold yellow]Actualizar método de pago[/]");

        var existing = await PromptPaymentMethodSelectionAsync("[yellow]Seleccione el método de pago a actualizar:[/]");
        if (existing is null)
            return;

        AnsiConsole.MarkupLine($"[grey]Nombre actual: {existing.DisplayName.Value}[/]");

        var newDisplayName = ConsoleMenuHelpers.PromptStringWithInitialOrBack("[yellow]Nuevo nombre:[/]", existing.DisplayName.Value, allowEmpty: false);
        if (newDisplayName is null) return;

        await AnsiConsole.Status()
            .StartAsync("Actualizando método de pago...", async ctx =>
            {
                try
                {
                    var updated = await _service.UpdateAsync(
                        existing.Id.Value,
                        string.IsNullOrWhiteSpace(newDisplayName) ? existing.DisplayName.Value : newDisplayName);

                    AnsiConsole.MarkupLine($"[green]Método de pago '[bold]{updated.DisplayName.Value}[/]' actualizado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task DeleteAsync()
    {
        AnsiConsole.MarkupLine("[bold red]Eliminar método de pago[/]");

        var existing = await PromptPaymentMethodSelectionAsync("[yellow]Seleccione el método de pago a eliminar:[/]");
        if (existing is null) return;

        var confirm = AnsiConsole.Confirm($"[red]¿Estás seguro de eliminar el método de pago '{Markup.Escape(existing.DisplayName.Value)}'?[/]");

        if (!confirm)
        {
            AnsiConsole.MarkupLine("[grey]Operación cancelada.[/]");
            return;
        }

        await AnsiConsole.Status()
            .StartAsync("Eliminando método de pago...", async ctx =>
            {
                try
                {
                    var deleted = await _service.DeleteAsync(existing.Id.Value);

                    if (deleted)
                        AnsiConsole.MarkupLine("[green]Método de pago eliminado correctamente.[/]");
                    else
                        AnsiConsole.MarkupLine("[red]Método de pago no encontrado.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task<dynamic?> PromptPaymentMethodSelectionAsync(string title)
    {
        var items = (await _service.GetAllAsync()).OrderBy(x => x.DisplayName.Value).ToList();
        if (!items.Any())
        {
            AnsiConsole.MarkupLine("[grey]No hay métodos de pago registrados.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(items.Select(FormatPaymentMethodChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return items.First(x => FormatPaymentMethodChoice(x) == selected);
    }

    private static string FormatPaymentMethodChoice(dynamic item) =>
        $"{item.Id.Value} · {Markup.Escape(item.DisplayName.Value)}";
}
