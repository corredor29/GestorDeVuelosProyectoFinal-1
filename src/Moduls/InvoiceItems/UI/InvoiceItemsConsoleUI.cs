using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Application.Interfaces;
using InvoiceItemAggregate = GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.Aggregate.InvoiceItem;
using GestorDeVuelosProyectoFinal.src.Shared.Session;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.UI;

public class InvoiceItemsConsoleUI
{
    private readonly IInvoiceItemsService _service;

    public InvoiceItemsConsoleUI(IInvoiceItemsService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Ítems de Factura").Color(Color.Blue));

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Selecciona una opción:[/]")
                    .AddChoices(GetMenuOptions()));

            switch (option)
            {
                case "Listar ítems por factura":
                    await GetByFacturaIdAsync();
                    break;
                case "Buscar por ID":
                    await GetByIdAsync();
                    break;
                case "Crear ítem de factura":
                    await CreateAsync();
                    break;
                case "Eliminar ítem de factura":
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
            "Listar ítems por factura",
            "Buscar por ID"
        };

        if (UserSession.Current?.IsAdmin == true)
        {
            options.Add("Crear ítem de factura");
            options.Add("Eliminar ítem de factura");
        }

        options.Add("Volver");
        return options;
    }

    private async Task GetByFacturaIdAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Listar ítems por ID de factura[/]");

        var facturaId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID de la factura:[/]");
        if (facturaId is null) return;

        try
        {
            var items = await _service.GetByFacturaIdAsync(facturaId.Value);
            var list = items.ToList();

            if (!list.Any())
            {
                AnsiConsole.MarkupLine("[grey]No hay ítems registrados para esta factura.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[blue]ID[/]")
                .AddColumn("[blue]Tipo de ítem[/]")
                .AddColumn("[blue]Descripción[/]")
                .AddColumn("[blue]Cantidad[/]")
                .AddColumn("[blue]Precio Unitario[/]")
                .AddColumn("[blue]Subtotal[/]")
                .AddColumn("[blue]Reserva Pasajero[/]");

            foreach (var item in list)
            {
                table.AddRow(
                    item.Id.Value.ToString(),
                    item.TipoItemId.Value.ToString(),
                    item.Descripcion.Value,
                    item.Cantidad.Value.ToString(),
                    item.PrecioUnitario.Value.ToString("C"),
                    item.Subtotal.Value.ToString("C"),
                    item.ReservaPasajeroId?.ToString() ?? "—"
                );
            }

            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
        }
    }

    private async Task GetByIdAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Buscar ítem de factura por ID[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null) return;

        try
        {
            var item = await _service.GetByIdAsync(id.Value);

            if (item is null)
            {
                AnsiConsole.MarkupLine("[red]Ítem de factura no encontrado.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[blue]Campo[/]")
                .AddColumn("[blue]Valor[/]");

            table.AddRow("ID", item.Id.Value.ToString());
            table.AddRow("Factura ID", item.FacturaId.Value.ToString());
            table.AddRow("Tipo de ítem ID", item.TipoItemId.Value.ToString());
            table.AddRow("Descripción", item.Descripcion.Value);
            table.AddRow("Cantidad", item.Cantidad.Value.ToString());
            table.AddRow("Precio unitario", item.PrecioUnitario.Value.ToString("C"));
            table.AddRow("Subtotal", item.Subtotal.Value.ToString("C"));
            table.AddRow("Reserva de pasajero", item.ReservaPasajeroId?.ToString() ?? "—");

            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
        }
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Crear nuevo ítem de factura[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null) return;
        var facturaId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID de la factura:[/]");
        if (facturaId is null) return;
        var tipoItemId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID del tipo de ítem:[/]");
        if (tipoItemId is null) return;
        var descripcion = ConsoleMenuHelpers.PromptRequiredStringOrBack("[yellow]Descripción:[/]");
        if (descripcion is null) return;
        var cantidad = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]Cantidad:[/]");
        if (cantidad is null) return;
        var precioUnitario = ConsoleMenuHelpers.PromptDecimalOrBack("[yellow]Precio unitario:[/]");
        if (precioUnitario is null) return;

        int? reservaPasajeroId = null;
        var tieneReserva = AnsiConsole.Confirm("[yellow]¿Desea asociar una reserva de pasajero?[/]");
        if (tieneReserva)
        {
            var reservaId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID de la reserva de pasajero:[/]");
            if (reservaId is null) return;
            reservaPasajeroId = reservaId.Value;
        }

        await AnsiConsole.Status()
            .StartAsync("Creando ítem de factura...", async ctx =>
            {
                try
                {
                    var item = await _service.CreateAsync(
                        id.Value,
                        facturaId.Value,
                        tipoItemId.Value,
                        descripcion,
                        cantidad.Value,
                        precioUnitario.Value,
                        reservaPasajeroId
                    );

                    AnsiConsole.MarkupLine($"[green]Ítem de factura '[bold]{item.Descripcion.Value}[/]' creado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task DeleteAsync()
    {
        AnsiConsole.MarkupLine("[bold red]Eliminar ítem de factura[/]");

        var item = await PromptInvoiceItemSelectionAsync("[yellow]Seleccione el ítem de factura a eliminar:[/]");
        if (item is null) return;

        var confirm = AnsiConsole.Confirm($"[red]¿Estás seguro de eliminar el ítem '{Markup.Escape(item.Descripcion.Value)}'?[/]");

        if (!confirm)
        {
            AnsiConsole.MarkupLine("[grey]Operación cancelada.[/]");
            return;
        }

        await AnsiConsole.Status()
            .StartAsync("Eliminando ítem de factura...", async ctx =>
            {
                try
                {
                    var deleted = await _service.DeleteAsync(item.Id.Value);

                    if (deleted)
                        AnsiConsole.MarkupLine("[green]Ítem de factura eliminado correctamente.[/]");
                    else
                        AnsiConsole.MarkupLine("[red]Ítem de factura no encontrado.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task<InvoiceItemAggregate?> PromptInvoiceItemSelectionAsync(string title)
    {
        var facturaId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID de la factura:[/]");
        if (facturaId is null)
            return null;

        var items = (await _service.GetByFacturaIdAsync(facturaId.Value)).OrderBy(x => x.FacturaId.Value).ThenBy(x => x.Descripcion.Value).ToList();
        if (!items.Any())
        {
            AnsiConsole.MarkupLine("[grey]No hay ítems de factura registrados.[/]");
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(items.Select(FormatInvoiceItemChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return items.First(x => FormatInvoiceItemChoice(x) == selected);
    }

    private static string FormatInvoiceItemChoice(InvoiceItemAggregate item) =>
        $"{item.Id.Value} · Factura {item.FacturaId.Value} · {Markup.Escape(item.Descripcion.Value)}";
}
