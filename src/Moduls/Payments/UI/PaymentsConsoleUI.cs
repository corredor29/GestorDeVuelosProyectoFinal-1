using Spectre.Console;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Session;
using GestorDeVuelosProyectoFinal.src.Shared.ui;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Payments.UI;

public class PaymentsConsoleUI
{
    private readonly IPaymentsService _service;

    public PaymentsConsoleUI(IPaymentsService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Pagos").Color(Color.Blue));

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Selecciona una opción:[/]")
                    .AddChoices(GetMenuOptions()));

            switch (option)
            {
                case "Listar pagos":
                    await ListAllAsync();
                    break;
                case "Buscar por ID":
                    await GetByIdAsync();
                    break;
                case "Registrar pago":
                    await CreateAsync();
                    break;
                case "Eliminar pago":
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
            "Listar pagos",
            "Buscar por ID"
        };

        if (UserSession.Current?.IsAdmin == true)
        {
            options.Add("Registrar pago");
            options.Add("Eliminar pago");
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
            AnsiConsole.MarkupLine("[grey]No hay pagos registrados.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[blue]ID[/]")
            .AddColumn("[blue]Reserva ID[/]")
            .AddColumn("[blue]Monto[/]")
            .AddColumn("[blue]Fecha de pago[/]")
            .AddColumn("[blue]Estado ID[/]")
            .AddColumn("[blue]Método ID[/]");

        foreach (var item in list)
            table.AddRow(
                item.Id.Value.ToString(),
                item.BookingId.Value.ToString(),
                item.Amount.Value.ToString("C"),
                item.Date.Value.ToString("yyyy-MM-dd HH:mm"),
                item.PaymentStatusId.Value.ToString(),
                item.PaymentMethodId.Value.ToString());

        AnsiConsole.Write(table);
    }

    private async Task GetByIdAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Buscar pago por ID[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null)
            return;

        try
        {
            var item = await _service.GetByIdAsync(id.Value);

            if (item is null)
            {
                AnsiConsole.MarkupLine("[red]Pago no encontrado.[/]");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn("[blue]Campo[/]")
                .AddColumn("[blue]Valor[/]");

            table.AddRow("ID",           item.Id.Value.ToString());
            table.AddRow("Reserva ID",   item.BookingId.Value.ToString());
            table.AddRow("Monto",        item.Amount.Value.ToString("C"));
            table.AddRow("Fecha de pago",   item.Date.Value.ToString("yyyy-MM-dd HH:mm"));
            table.AddRow("Estado ID",    item.PaymentStatusId.Value.ToString());
            table.AddRow("Método ID",    item.PaymentMethodId.Value.ToString());
            table.AddRow("Creado",       item.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm"));
            table.AddRow("Actualizado",  item.UpdatedAt.Value.ToString("yyyy-MM-dd HH:mm"));

            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
        }
    }

    private async Task CreateAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Registrar nuevo pago[/]");

        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID:[/]");
        if (id is null)
            return;

        var bookingId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]Reserva ID:[/]");
        if (bookingId is null)
            return;

        var amount = ConsoleMenuHelpers.PromptDecimalOrBack("[yellow]Monto:[/]");
        if (amount is null)
            return;

        var paidAt = ConsoleMenuHelpers.PromptDateTimeOrBack("[yellow]Fecha de pago (yyyy-MM-dd HH:mm):[/]", "yyyy-MM-dd HH:mm");
        if (paidAt is null)
            return;

        var paymentStatusId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID Estado de pago:[/]");
        if (paymentStatusId is null)
            return;

        var paymentMethodId = ConsoleMenuHelpers.PromptPositiveIntOrBack("[yellow]ID del método de pago:[/]");
        if (paymentMethodId is null)
            return;

        await AnsiConsole.Status()
            .StartAsync("Registrando pago...", async ctx =>
            {
                try
                {
                    var item = await _service.CreateAsync(id.Value, bookingId.Value, amount.Value, paidAt.Value, paymentStatusId.Value, paymentMethodId.Value);
                    AnsiConsole.MarkupLine($"[green]Pago '[bold]{item.Id.Value}[/]' registrado correctamente.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }

    private async Task DeleteAsync()
    {
        AnsiConsole.MarkupLine("[bold red]Eliminar pago[/]");

        var payments = (await _service.GetAllAsync()).OrderBy(x => x.Id.Value).ToList();
        if (payments.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]No hay pagos registrados.[/]");
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Seleccione el pago a eliminar:[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(payments.Select(p => $"{p.Id.Value} · Reserva {p.BookingId.Value} · {p.Amount.Value:C}").Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return;

        var payment = payments.First(p => $"{p.Id.Value} · Reserva {p.BookingId.Value} · {p.Amount.Value:C}" == selected);

        var confirm = AnsiConsole.Confirm($"[red]¿Estás seguro de eliminar el pago con ID {payment.Id.Value}?[/]");

        if (!confirm)
        {
            AnsiConsole.MarkupLine("[grey]Operación cancelada.[/]");
            return;
        }

        await AnsiConsole.Status()
            .StartAsync("Eliminando pago...", async ctx =>
            {
                try
                {
                    var deleted = await _service.DeleteAsync(payment.Id.Value);

                    if (deleted)
                        AnsiConsole.MarkupLine("[green]Pago eliminado correctamente.[/]");
                    else
                        AnsiConsole.MarkupLine("[red]Pago no encontrado.[/]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }
            });
    }
}
