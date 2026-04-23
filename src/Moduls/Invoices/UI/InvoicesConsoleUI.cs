using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Session;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Invoices.UI;

public class InvoicesConsoleUI
{
    private readonly IInvoicesService _service;

    public InvoicesConsoleUI(IInvoicesService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Facturas").Color(Color.Blue));

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Selecciona una opción:[/]")
                    .AddChoices(GetMenuOptions()));

            switch (option)
            {
                case "Buscar por ID":
                    await GetByIdAsync();
                    break;
                case "Buscar por número de factura":
                    await GetByNumberAsync();
                    break;
                case "Buscar por reserva":
                    await GetByReservationAsync();
                    break;
                case "Crear factura":
                    await CreateAsync();
                    break;
                case "Eliminar factura":
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
            "Buscar por ID",
            "Buscar por número de factura",
            "Buscar por reserva"
        };

        if (UserSession.Current?.IsAdmin == true)
        {
            options.Add("Crear factura");
            options.Add("Eliminar factura");
        }

        options.Add("Volver");
        return options;
    }

    private async Task GetByIdAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Buscar factura por ID[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null) return;

        try
        {
            var item = await _service.GetByIdAsync(id.Value);

            if (item is null)
            {
                AnsiConsole.MarkupLine("[red]Factura no encontrada.[/]");
                return;
            }

            RenderDetail(item);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
        }
    }

    private async Task GetByNumberAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Buscar factura por número[/]");

        var number = ConsoleMenuHelpers.PromptRequiredStringOrBack("[yellow]Número de factura:[/]");
        if (number is null) return;

        try
        {
            var item = await _service.GetByInvoiceNumberAsync(number);

            if (item is null)
            {
                AnsiConsole.MarkupLine("[red]Factura no encontrada.[/]");
                return;
            }

            RenderDetail(item);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
        }
    }

    private async Task GetByReservationAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Buscar factura por reserva[/]");

        var reservaId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID de la reserva:[/]");
        if (reservaId is null) return;

        try
        {
            var item = await _service.GetByReservationIdAsync(reservaId.Value);

            if (item is null)
            {
                AnsiConsole.MarkupLine("[red]Factura no encontrada para esta reserva.[/]");
                return;
            }

            RenderDetail(item);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
        }
    }

    private void RenderDetail(Domain.Aggregate.Invoice invoice)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[blue]Campo[/]")
            .AddColumn("[blue]Valor[/]");

        table.AddRow("ID", invoice.Id.Value.ToString());
        table.AddRow("Reserva ID", invoice.ReservaId.Value.ToString());
        table.AddRow("Número de factura", invoice.NumeroFactura.Value);
        table.AddRow("Fecha de emisión", invoice.FechaEmision.Value.ToString("yyyy-MM-dd HH:mm"));
        table.AddRow("Subtotal", invoice.Subtotal.Value.ToString("F2"));
        table.AddRow("Impuestos", invoice.Impuestos.Value.ToString("F2"));
        table.AddRow("Total", invoice.Total.Value.ToString("F2"));

        AnsiConsole.Write(table);
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Crear nueva factura[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null) return;
        var reservaId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID de la reserva:[/]");
        if (reservaId is null) return;
        var numero = ConsoleMenuHelpers.PromptRequiredStringOrBack("[yellow]Número de factura:[/]");
        if (numero is null) return;
        var subtotal = ConsoleMenuHelpers.PromptDecimalOrBack("[yellow]Subtotal:[/]");
        if (subtotal is null) return;
        var impuestos = ConsoleMenuHelpers.PromptDecimalOrBack("[yellow]Impuestos:[/]");
        if (impuestos is null) return;
        var total = subtotal.Value + impuestos.Value;

        AnsiConsole.MarkupLine($"[grey]Total calculado: {total:F2}[/]");

        await AnsiConsole.Status()
            .StartAsync("Creando factura...", async ctx =>
            {
                try
                {
                    var item = await _service.CreateAsync(
                        id.Value,
                        reservaId.Value,
                        numero,
                        subtotal.Value,
                        impuestos.Value,
                        total
                    );
                    AnsiConsole.MarkupLine($"[green]Factura '[bold]{item.NumeroFactura.Value}[/]' creada correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task DeleteAsync()
    {
        AnsiConsole.MarkupLine("[bold red]Eliminar factura[/]");

        var invoice = await PromptInvoiceSelectionAsync("[yellow]Seleccione la factura a eliminar:[/]");
        if (invoice is null) return;

        var confirm = AnsiConsole.Confirm($"[red]¿Estás seguro de eliminar la factura '{Markup.Escape(invoice.NumeroFactura.Value)}'?[/]");

        if (!confirm)
        {
            AnsiConsole.MarkupLine("[grey]Operación cancelada.[/]");
            return;
        }

        await AnsiConsole.Status()
            .StartAsync("Eliminando factura...", async ctx =>
            {
                try
                {
                    var deleted = await _service.DeleteAsync(invoice.Id.Value);

                    if (deleted)
                        AnsiConsole.MarkupLine("[green]Factura eliminada correctamente.[/]");
                    else
                        AnsiConsole.MarkupLine("[red]Factura no encontrada.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task<Domain.Aggregate.Invoice?> PromptInvoiceSelectionAsync(string title)
    {
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(
                    "Buscar por ID",
                    "Buscar por número de factura",
                    "Buscar por reserva",
                    ConsoleMenuHelpers.VolverAlMenu));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        if (selected == "Buscar por ID")
        {
            var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID de la factura:[/]");
            if (id is null)
                return null;

            return await _service.GetByIdAsync(id.Value);
        }

        if (selected == "Buscar por número de factura")
        {
            var number = ConsoleMenuHelpers.PromptRequiredStringOrBack("[yellow]Número de factura:[/]");
            if (number is null)
                return null;

            return await _service.GetByInvoiceNumberAsync(number);
        }

        var bookingId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID de la reserva:[/]");
        if (bookingId is null)
            return null;

        return await _service.GetByReservationIdAsync(bookingId.Value);
    }
}
