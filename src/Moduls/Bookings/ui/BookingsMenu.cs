using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;
using DomainBooking = GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Aggregate.Booking;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.ui;

public sealed class BookingsMenu : IModuleUI
{
    private readonly IBookingsService _service;

    public BookingsMenu(IBookingsService service)
    {
        _service = service;
    }

    public string Key => "bookings";
    public string Title => "Reservas";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Reservas [/]").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "Listar todas",
                        "Buscar por ID",
                        "Buscar por ID de cliente",
                        "Buscar por ID de estado de reserva",
                        "Buscar por rango de fechas",
                        "Crear reserva",
                        "Actualizar reserva",
                        "Confirmar reserva",
                        "Cancelar reserva",
                        "Eliminar por ID",
                        "Eliminar por ID de cliente",
                        "Volver"));

            switch (option)
            {
                case "Listar todas":
                    await ListAsync(cancellationToken);
                    break;
                case "Buscar por ID":
                    await SearchByIdAsync(cancellationToken);
                    break;
                case "Buscar por ID de cliente":
                    await SearchByClientIdAsync(cancellationToken);
                    break;
                case "Buscar por ID de estado de reserva":
                    await SearchByStatusIdAsync(cancellationToken);
                    break;
                case "Buscar por rango de fechas":
                    await SearchByRangeAsync(cancellationToken);
                    break;
                case "Crear reserva":
                    await CreateAsync(cancellationToken);
                    break;
                case "Actualizar reserva":
                    await UpdateAsync(cancellationToken);
                    break;
                case "Confirmar reserva":
                    await ConfirmAsync(cancellationToken);
                    break;
                case "Cancelar reserva":
                    await CancelAsync(cancellationToken);
                    break;
                case "Eliminar por ID":
                    await DeleteByIdAsync(cancellationToken);
                    break;
                case "Eliminar por ID de cliente":
                    await DeleteByClientIdAsync(cancellationToken);
                    break;
                case "Volver":
                    return;
            }
        }
    }

    private async Task ListAsync(CancellationToken cancellationToken)
    {
        RenderTable(await _service.GetAllAsync(cancellationToken), "Reservas");
        Pause();
    }

    private async Task SearchByIdAsync(CancellationToken cancellationToken)
    {
        var id = ConsoleMenuHelpers.PromptPositiveIntOrBack("ID:");
        if (id is null)
            return;

        var item = await _service.GetByIdAsync(id.Value, cancellationToken);
        RenderTable(item is null ? Array.Empty<DomainBooking>() : new[] { item }, $"Reserva {id.Value}");
        Pause();
    }

    private async Task SearchByClientIdAsync(CancellationToken cancellationToken)
    {
        var clientId = ConsoleMenuHelpers.PromptPositiveIntOrBack("client_id:");
        if (clientId is null)
            return;

        RenderTable(await _service.GetByClientIdAsync(clientId.Value, cancellationToken), $"Reservas del cliente {clientId.Value}");
        Pause();
    }

    private async Task SearchByStatusIdAsync(CancellationToken cancellationToken)
    {
        var statusId = ConsoleMenuHelpers.PromptPositiveIntOrBack("booking_status_id:");
        if (statusId is null)
            return;

        RenderTable(await _service.GetByStatusIdAsync(statusId.Value, cancellationToken), $"Reservas con estado {statusId.Value}");
        Pause();
    }

    private async Task SearchByRangeAsync(CancellationToken cancellationToken)
    {
        var from = PromptDateTimeOrBack("Fecha inicial (yyyy-MM-dd HH:mm:ss):", DateTime.UtcNow.AddDays(-7));
        if (from is null)
            return;

        var to = PromptDateTimeOrBack("Fecha final (yyyy-MM-dd HH:mm:ss):", DateTime.UtcNow);
        if (to is null)
            return;

        RenderTable(await _service.GetByBookedAtRangeAsync(from.Value, to.Value, cancellationToken), "Reservas por rango");
        Pause();
    }

    private async Task CreateAsync(CancellationToken cancellationToken)
    {
        if (!ConsoleMenuHelpers.TryBeginFormOrBack("Registrar reserva"))
            return;

        var clientId = ConsoleMenuHelpers.PromptPositiveIntOrBack("client_id:");
        if (clientId is null)
            return;

        var bookedAt = PromptDateTimeOrBack("booked_at (yyyy-MM-dd HH:mm:ss):", DateTime.UtcNow);
        if (bookedAt is null)
            return;

        var bookingStatusId = ConsoleMenuHelpers.PromptPositiveIntOrBack("booking_status_id:");
        if (bookingStatusId is null)
            return;

        var totalAmount = PromptAmountOrBack("total_amount:");
        if (totalAmount is null)
            return;

        var expiresAt = PromptOptionalDateTimeOrBack("expires_at (deja vacio si no aplica):");
        if (expiresAt.WentBack)
            return;

        try
        {
            await _service.CreateAsync(clientId.Value, bookedAt.Value, bookingStatusId.Value, totalAmount.Value, expiresAt.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Reserva creada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task UpdateAsync(CancellationToken cancellationToken)
    {
        var current = await PromptBookingSelectionAsync("Seleccione la reserva a actualizar:", cancellationToken);
        if (current is null)
            return;

        var clientId = ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("Nuevo client_id:", current.ClientId.Value);
        if (clientId is null)
            return;

        var bookedAt = PromptDateTimeOrBack("Nuevo booked_at (yyyy-MM-dd HH:mm:ss):", current.BookedAt.Value);
        if (bookedAt is null)
            return;

        var bookingStatusId = ConsoleMenuHelpers.PromptPositiveIntWithInitialOrBack("Nuevo booking_status_id:", current.BookingStatusId.Value);
        if (bookingStatusId is null)
            return;

        var totalAmount = PromptAmountOrBack("Nuevo total_amount:", current.TotalAmount.Value);
        if (totalAmount is null)
            return;

        var expiresAt = PromptOptionalDateTimeOrBack("Nuevo expires_at (deja vacio si no aplica):", current.ExpiresAt.Value);
        if (expiresAt.WentBack)
            return;

        try
        {
            await _service.UpdateAsync(current.Id!.Value, clientId.Value, bookedAt.Value, bookingStatusId.Value, totalAmount.Value, expiresAt.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Reserva actualizada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task ConfirmAsync(CancellationToken cancellationToken)
    {
        var current = await PromptBookingSelectionAsync("Seleccione la reserva a confirmar:", cancellationToken);
        if (current is null)
            return;

        try
        {
            await _service.ConfirmAsync(current.Id!.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Reserva confirmada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task CancelAsync(CancellationToken cancellationToken)
    {
        var current = await PromptBookingSelectionAsync("Seleccione la reserva a cancelar:", cancellationToken);
        if (current is null)
            return;

        try
        {
            await _service.CancelAsync(current.Id!.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Reserva cancelada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteByIdAsync(CancellationToken cancellationToken)
    {
        var current = await PromptBookingSelectionAsync("Seleccione la reserva a eliminar:", cancellationToken);
        if (current is null)
            return;

        if (!AnsiConsole.Confirm("¿Confirmas la eliminación?", false))
        {
            Pause();
            return;
        }

        try
        {
            await _service.DeleteByIdAsync(current.Id!.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Reserva eliminada correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private async Task DeleteByClientIdAsync(CancellationToken cancellationToken)
    {
        var clientId = ConsoleMenuHelpers.PromptPositiveIntOrBack("client_id a eliminar:");
        if (clientId is null)
            return;

        if (!AnsiConsole.Confirm("Se eliminaran todas las reservas de ese cliente. Confirmas?", false))
        {
            Pause();
            return;
        }

        try
        {
            await _service.DeleteByClientIdAsync(clientId.Value, cancellationToken);
            AnsiConsole.MarkupLine("\n[green]Reservas eliminadas correctamente.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(ex.Message)}[/]");
        }

        Pause();
    }

    private static void RenderTable(IEnumerable<DomainBooking> items, string title)
    {
        var list = items.ToList();
        if (list.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay registros para mostrar.[/]");
            return;
        }

        var table = new Table()
            .Title(title)
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold grey]ID[/]")
            .AddColumn("[bold grey]client_id[/]")
            .AddColumn("[bold grey]booked_at[/]")
            .AddColumn("[bold grey]booking_status_id[/]")
            .AddColumn("[bold grey]total_amount[/]")
            .AddColumn("[bold grey]expires_at[/]")
            .AddColumn("[bold grey]created_at[/]")
            .AddColumn("[bold grey]updated_at[/]");

        foreach (var item in list)
        {
            table.AddRow(
                item.Id?.Value.ToString() ?? "-",
                item.ClientId.Value.ToString(),
                item.BookedAt.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                item.BookingStatusId.Value.ToString(),
                item.TotalAmount.Value.ToString("0.00"),
                item.ExpiresAt.Value?.ToString("yyyy-MM-dd HH:mm:ss") ?? "-",
                item.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                item.UpdatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        AnsiConsole.Write(table);
    }

    private static decimal? PromptAmountOrBack(string label, decimal? current = null)
    {
        while (true)
        {
            var value = ConsoleMenuHelpers.PromptStringWithInitialOrBack(label, current?.ToString("0.00") ?? "0.00", allowEmpty: false);
            if (value is null)
                return null;

            if (!decimal.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var parsed)
                && !decimal.TryParse(value, out parsed))
            {
                AnsiConsole.MarkupLine("[red]Monto inválido.[/]");
                continue;
            }

            if (parsed < 0)
            {
                AnsiConsole.MarkupLine("[red]No puede ser negativo.[/]");
                continue;
            }

            return parsed;
        }
    }

    private static DateTime? PromptDateTimeOrBack(string label, DateTime current)
    {
        while (true)
        {
            var value = ConsoleMenuHelpers.PromptStringWithInitialOrBack(label, current.ToString("yyyy-MM-dd HH:mm:ss"), allowEmpty: false);
            if (value is null)
                return null;

            if (DateTime.TryParse(value, out var dt))
                return dt;

            AnsiConsole.MarkupLine("[red]Fecha inválida.[/]");
        }
    }

    private static (bool WentBack, DateTime? Value) PromptOptionalDateTimeOrBack(string label, DateTime? current = null)
    {
        while (true)
        {
            var value = ConsoleMenuHelpers.PromptStringWithInitialOrBack(label, current?.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty, allowEmpty: true);
            if (value is null)
                return (true, null);

            if (string.IsNullOrWhiteSpace(value))
                return (false, null);

            if (DateTime.TryParse(value, out var dt))
                return (false, dt);

            AnsiConsole.MarkupLine("[red]Fecha inválida.[/]");
        }
    }

    private async Task<DomainBooking?> PromptBookingSelectionAsync(string title, CancellationToken cancellationToken)
    {
        var bookings = (await _service.GetAllAsync(cancellationToken))
            .OrderByDescending(x => x.BookedAt.Value)
            .ToList();
        if (bookings.Count == 0)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay reservas registradas.[/]");
            Pause();
            return null;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .PageSize(12)
                .AddChoices(bookings.Select(FormatBookingChoice).Append(ConsoleMenuHelpers.VolverAlMenu).ToList()));

        if (selected == ConsoleMenuHelpers.VolverAlMenu)
            return null;

        return bookings.First(x => FormatBookingChoice(x) == selected);
    }

    private static string FormatBookingChoice(DomainBooking booking) =>
        $"{booking.Id?.Value ?? 0} · Cliente {booking.ClientId.Value} · {booking.BookedAt.Value:yyyy-MM-dd HH:mm}";

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Presiona Enter para continuar...[/]").AllowEmpty());
    }
}
