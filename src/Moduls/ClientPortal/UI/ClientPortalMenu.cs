using GestorDeVuelosProyectoFinal.src.Moduls.ClientPortal.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.ClientPortal.Application.Models;
using GestorDeVuelosProyectoFinal.src.Moduls.ClientPortal.Application.Support;
using GestorDeVuelosProyectoFinal.src.Shared.ui;
using Spectre.Console;

namespace GestorDeVuelosProyectoFinal.src.Moduls.ClientPortal.UI;

public sealed class ClientPortalMenu : IModuleUI
{
    private readonly IClientPortalService _service;

    private sealed class BackNavigationException : Exception
    {
    }

    public ClientPortalMenu(IClientPortalService service)
    {
        _service = service;
    }

    public string Key => "client_portal";
    public string Title => "Portal del cliente";

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] PORTAL DEL CLIENTE [/] ").LeftJustified());

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[grey]Usa las flechas para navegar[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        "1. Buscar y comprar vuelos",
                        "2. Mis reservas",
                        "3. Hacer check-in en línea",
                        "4. Mis tiquetes",
                        "5. Cerrar sesión"));

            switch (option)
            {
                case "1. Buscar y comprar vuelos":
                    await SearchAndBuyAsync(cancellationToken);
                    break;
                case "2. Mis reservas":
                    await MyBookingsAsync(cancellationToken);
                    break;
                case "3. Hacer check-in en línea":
                    await OnlineCheckinAsync(cancellationToken);
                    break;
                case "4. Mis tiquetes":
                    await MyTicketsAsync(cancellationToken);
                    break;
                case "5. Cerrar sesión":
                    return;
            }
        }
    }

    private static string FormatError(Exception ex)
    {
        var msg = ex.Message;
        var inner = ex.InnerException;
        var depth = 0;
        while (inner is not null && depth < 5)
        {
            msg += $" | Inner: {inner.Message}";
            inner = inner.InnerException;
            depth++;
        }
        return msg;
    }

    private async Task SearchAndBuyAsync(CancellationToken cancellationToken)
    {
        try
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Buscar y comprar vuelos [/]").LeftJustified());

            _ = await _service.EnsureClientContextAsync(cancellationToken);

            // Tabla inicial con todos los vuelos disponibles
            var allFlights = (await _service.ListAvailableFlightsAsync(cancellationToken)).ToList();
            if (allFlights.Count > 0)
            {
                var tableAll = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Grey)
                    .AddColumn("[bold grey]Vuelo[/]")
                    .AddColumn("[bold grey]Origen[/]")
                    .AddColumn("[bold grey]Destino[/]")
                    .AddColumn("[bold grey]Salida[/]")
                    .AddColumn("[bold grey]Llegada[/]")
                    .AddColumn("[bold grey]Cupos[/]");

                foreach (var f in allFlights.Take(30))
                    tableAll.AddRow(
                        Markup.Escape(f.FlightCode),
                        Markup.Escape(f.Origin),
                        Markup.Escape(f.Destination),
                        f.DepartureAt.ToString("yyyy-MM-dd HH:mm"),
                        f.ArrivalAt.ToString("yyyy-MM-dd HH:mm"),
                        f.AvailableSeats.ToString());

                AnsiConsole.Write(new Rule("[grey]Vuelos disponibles (muestra hasta 30)[/]").LeftJustified());
                AnsiConsole.Write(tableAll);
                AnsiConsole.WriteLine();
            }

            // Catálogo de ciudades (nombre + región + país) para facilitar la búsqueda
            var allCities = (await _service.ListAllCitiesAsync(cancellationToken)).ToList();
            if (allCities.Count > 0)
            {
                var cityTable = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Grey)
                    .AddColumn("[bold grey]Ciudad[/]")
                    .AddColumn("[bold grey]Región[/]")
                    .AddColumn("[bold grey]País[/]");

                foreach (var c in allCities.Take(200))
                    cityTable.AddRow(Markup.Escape(c.City), Markup.Escape(c.Region), Markup.Escape(c.Country));

                AnsiConsole.Write(new Rule("[grey]Ciudades disponibles (muestra hasta 200)[/]").LeftJustified());
                AnsiConsole.Write(cityTable);
                if (allCities.Count > 200)
                    AnsiConsole.MarkupLine($"\n[yellow]Hay {allCities.Count} ciudades en total. Usa el catálogo interactivo o busca por texto para encontrar la tuya.[/]");
                AnsiConsole.WriteLine();
            }

            TripType? tripType = null;
            int? cabinTypeId = null;
            int? originCityId = null;
            int? destinationCityId = null;
            DateOnly? outboundDate = null;
            DateOnly? returnDate = null;
            int adults = 0;
            int seniors = 0;
            int minors = 0;

            var searchStep = 0;
            while (searchStep < 9)
            {
                try
                {
                    switch (searchStep)
                    {
                        case 0:
                        {
                            var tripTypePick = PromptTripType();
                            if (tripTypePick is null)
                                return;
                            tripType = tripTypePick.Value;
                            if (tripType != TripType.RoundTrip)
                                returnDate = null;
                            searchStep++;
                            break;
                        }
                        case 1:
                            cabinTypeId = await PromptCabinTypeIdAsync(cancellationToken);
                            searchStep++;
                            break;
                        case 2:
                            originCityId = await PromptCityAsync("Ciudad origen:", cancellationToken);
                            searchStep++;
                            break;
                        case 3:
                            destinationCityId = await PromptCityAsync("Ciudad destino:", cancellationToken);
                            searchStep++;
                            break;
                        case 4:
                            outboundDate = PromptOptionalDateOnly("Fecha de ida (yyyy-MM-dd) (Enter = no definida):");
                            searchStep++;
                            break;
                        case 5:
                            if (tripType == TripType.RoundTrip)
                                returnDate = PromptOptionalDateOnly("Fecha de regreso (yyyy-MM-dd) (Enter = no definida):");
                            else
                                returnDate = null;
                            searchStep++;
                            break;
                        case 6:
                            adults = PromptMinInt("Número de pasajeros adultos:", 0);
                            searchStep++;
                            break;
                        case 7:
                            seniors = PromptMinInt("Número de pasajeros adulto mayor:", 0);
                            searchStep++;
                            break;
                        case 8:
                        {
                            minors = PromptMinInt("Número de pasajeros menores:", 0);
                            var previewTotal = adults + seniors + minors;
                            if (previewTotal <= 0)
                                throw new InvalidOperationException("Debes reservar al menos 1 pasajero.");
                            if (adults + seniors <= 0)
                                throw new InvalidOperationException("Debe haber al menos 1 Adulto o 1 Adulto mayor en la compra.");
                            searchStep++;
                            break;
                        }
                    }
                }
                catch (BackNavigationException)
                {
                    if (searchStep == 0)
                        return;
                    searchStep--;
                }
            }

            var totalPassengers = adults + seniors + minors;
            var req = new FlightSearchRequest(tripType!.Value, originCityId!.Value, destinationCityId!.Value, outboundDate, returnDate, adults + seniors, minors, cabinTypeId!.Value);

            var outbound = await SearchWithFallbackAsync(originCityId.Value, destinationCityId.Value, outboundDate, cabinTypeId.Value, cancellationToken);
            if (outbound.Count == 0)
            {
                Pause();
                return;
            }

            var outboundPick = PickFlight(outbound, "Selecciona tu vuelo de ida:");
            if (outboundPick is null)
                throw new BackNavigationException();
            int? returnFlightId = null;
            FlightSearchResult? returnPick = null;

            if (tripType == TripType.RoundTrip)
            {
                var ret = await SearchWithFallbackAsync(destinationCityId.Value, originCityId.Value, returnDate, cabinTypeId.Value, cancellationToken);
                if (ret.Count == 0)
                {
                    AnsiConsole.MarkupLine(
                        "\n[yellow]No hay vuelos de regreso (destino → origen) con los criterios actuales, ni en fechas alternas en esta ruta.[/]");

                    var opt = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("\n¿Cómo deseas continuar?")
                            .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                            .AddChoices(
                                "Comprar solo ida (usar solo el vuelo de ida elegido)",
                                "Volver (cambiar ciudades, fechas o elegir «Solo ida»)"));

                    if (!opt.StartsWith("Comprar solo ida", StringComparison.Ordinal))
                    {
                        Pause();
                        return;
                    }

                    tripType = TripType.OneWay;
                    req = new FlightSearchRequest(
                        TripType.OneWay,
                        originCityId.Value,
                        destinationCityId.Value,
                        outboundDate,
                        ReturnDate: null,
                        adults + seniors,
                        minors,
                        cabinTypeId.Value);
                    returnPick = null;
                    returnFlightId = null;
                    AnsiConsole.MarkupLine("\n[green]Se continúa como solo ida: la compra incluirá únicamente el vuelo de ida.[/]");
                }
                else
                {
                    returnPick = PickFlight(ret, "Selecciona tu vuelo de regreso:");
                    if (returnPick is null)
                        throw new BackNavigationException();
                    returnFlightId = returnPick.Flight.Id;
                }
            }

            var passengers = new List<PassengerInput>();
            for (var i = 0; i < totalPassengers; i++)
            {
                string? firstName = null;
                string? lastName = null;
                int? docTypeId = null;
                string? docNumber = null;
                var passengerStep = 0;

                while (passengerStep < 4)
                {
                    try
                    {
                        AnsiConsole.Clear();
                        AnsiConsole.Write(new Rule($"[bold deepskyblue1] Tiquete {i + 1}/{totalPassengers} — datos del pasajero [/]").LeftJustified());

                        switch (passengerStep)
                        {
                            case 0:
                                firstName = PromptNonEmpty("Nombre(s):");
                                passengerStep++;
                                break;
                            case 1:
                                lastName = PromptNonEmpty("Apellido(s):");
                                passengerStep++;
                                break;
                            case 2:
                                docTypeId = await PromptDocumentTypeIdAsync(cancellationToken);
                                passengerStep++;
                                break;
                            case 3:
                                docNumber = PromptDigitsOnly("Número documento:", 1, 30);
                                passengerStep++;
                                break;
                        }
                    }
                    catch (BackNavigationException)
                    {
                        if (passengerStep == 0)
                        {
                            if (i == 0)
                                throw;
                            i -= 2;
                            passengers.RemoveAt(passengers.Count - 1);
                            break;
                        }

                        passengerStep--;
                    }
                }

                if (passengerStep < 4)
                    continue;

                var category = i < adults
                    ? "Adult"
                    : i < adults + seniors
                        ? "Senior"
                        : "Child";
                passengers.Add(new PassengerInput(firstName!, lastName!, docNumber!, docTypeId!.Value, category));
            }

            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Equipaje [/]").LeftJustified());
            AnsiConsole.MarkupLine("[grey]La maleta extra se cobra por pieza y por cada tramo del viaje (ida y, si aplica, regreso).[/]");
            var extraBaggagePieces = PromptMinInt("¿Cuántas maletas extra documentadas? (0 = ninguna):", 0);

            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Datos de contacto [/]").LeftJustified());
            var email = PromptEmail("Email:");
            var email2 = PromptEmail("Confirmar email:");
            if (!string.Equals(email, email2, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Los emails no coinciden.");

            var countryCode = PromptCountryCode("Código país (solo números, ej. 57):");
            var phone = PromptPhoneNumber("Número celular:");
            var contact = new ContactInput(email, countryCode, phone);

            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Método de pago (simulado) [/]").LeftJustified());
            var payment = PromptPaymentMethod();
            if (payment is null)
                return;
            SimulatedCardInput? card = null;
            if (payment is SimulatedPaymentMethod.CreditCard or SimulatedPaymentMethod.DebitCard)
            {
                var number = PromptDigitsOnly("Número tarjeta:", 12, 19);
                var holder = PromptNonEmpty("Titular:");
                var exp = PromptCardExpiration("Vencimiento (MM/AA):");
                var sec = PromptDigitsOnly("Código seguridad:", 3, 4);
                card = new SimulatedCardInput(number, holder, exp, sec);
            }

            var purchaseDraft = new PurchaseRequest(
                req,
                outboundPick.Flight.Id,
                returnFlightId,
                passengers,
                contact,
                payment.Value,
                card,
                TotalAmount: 0m,
                ExtraBaggagePieces: extraBaggagePieces,
                PrioritySeatSelectionAtPurchase: false);

            var preview = await _service.PreviewPurchaseAsync(purchaseDraft, cancellationToken);
            if (preview.TotalAmount <= 0m)
                throw new InvalidOperationException("No se pudo calcular el precio (tarifas en 0). Verifica tarifas y fechas.");

            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Confirmar compra [/]").LeftJustified());

            AnsiConsole.MarkupLine($"\n[white]{Markup.Escape(outboundPick.OriginCity.Name)}[/] → [white]{Markup.Escape(outboundPick.DestinationCity.Name)}[/]  [dim]·[/]  {(tripType == TripType.RoundTrip ? "Ida y vuelta" : "Solo ida")}");
            AnsiConsole.MarkupLine($"[grey]Pasajeros / tiquetes:[/] [white]{totalPassengers}[/]  [dim]·[/]  [grey]Clase:[/] [white]{Markup.Escape(await ResolveCabinTypeNameAsync(cabinTypeId.Value, cancellationToken))}[/]");

            var linesTable = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Grey)
                .AddColumn("[bold grey]Concepto[/]")
                .AddColumn("[bold grey]Monto[/]");
            foreach (var line in preview.Lines)
                linesTable.AddRow(Markup.Escape(line.Leg), line.Amount.ToString("0.00"));
            AnsiConsole.Write(linesTable);
            AnsiConsole.MarkupLine($"\n[bold]Precio total:[/] [green]{preview.TotalAmount:0.00}[/]");

            var confirmOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\nCuando hayas revisado los datos de cada tiquete y el total:")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices("Ver detalle de vuelos", "Confirmar compra", "Cancelar"));

            if (confirmOption == "Ver detalle de vuelos")
            {
                RenderFlightDetail("Vuelo de ida", outboundPick, cabinTypeId.Value);
                if (returnPick is not null)
                    RenderFlightDetail("Vuelo de regreso", returnPick, cabinTypeId.Value);
                Pause();
                confirmOption = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("\n¿Confirmas la compra con el total mostrado?")
                        .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                        .AddChoices("Confirmar compra", "Cancelar"));
            }

            if (confirmOption != "Confirmar compra")
                return;

            var purchase = purchaseDraft with { TotalAmount = preview.TotalAmount };

            var result = await _service.PurchaseAsync(purchase, cancellationToken);

            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold green] Compra exitosa [/]").LeftJustified());
            AnsiConsole.MarkupLine($"\n[green]¡Compra exitosa! Guarda tu código de reserva:[/] [bold]{Markup.Escape(result.ReservationReference)}[/]");
            AnsiConsole.MarkupLine($"[grey]Referencia interna (soporte):[/] [white]{result.BookingId}[/]");

            RenderTicketsTable(result.Tickets);
        }
        catch (BackNavigationException)
        {
            return;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(FormatError(ex))}[/]");
        }

        Pause();
    }

    private async Task<List<FlightSearchResult>> SearchWithFallbackAsync(
        int originCityId,
        int destinationCityId,
        DateOnly? date,
        int cabinTypeId,
        CancellationToken cancellationToken)
    {
        var results = (await _service.SearchFlightsAsync(originCityId, destinationCityId, date, cabinTypeId, cancellationToken)).ToList();
        if (results.Count > 0)
            return results;

        if (date.HasValue)
        {
            AnsiConsole.MarkupLine("\n[yellow]No hay vuelos para esa fecha. Buscando fechas cercanas (±14 días)…[/]");

            for (var delta = -14; delta <= 14; delta++)
            {
                if (delta == 0)
                    continue;
                var d = date.Value.AddDays(delta);
                results = (await _service.SearchFlightsAsync(originCityId, destinationCityId, d, cabinTypeId, cancellationToken)).ToList();
                if (results.Count > 0)
                    return results;
            }

            AnsiConsole.MarkupLine("\n[yellow]Sin resultados en ±14 días. Mostrando todos los vuelos disponibles en esta ruta (cualquier fecha)…[/]");
            results = (await _service.SearchFlightsAsync(originCityId, destinationCityId, null, cabinTypeId, cancellationToken)).ToList();
        }

        return results;
    }

    private async Task MyBookingsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var ctx = await _service.EnsureClientContextAsync(cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new Rule("[bold deepskyblue1] Mis reservas [/]").LeftJustified());

                var bookings = (await _service.GetMyBookingsAsync(ctx.ClientId, cancellationToken)).ToList();
                if (bookings.Count == 0)
                {
                    AnsiConsole.MarkupLine("\n[yellow]No tienes reservas.[/]");
                    Pause();
                    return;
                }

                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Grey)
                    .AddColumn("[bold grey]Código reserva[/]")
                    .AddColumn("[bold grey]Fecha reserva[/]")
                    .AddColumn("[bold grey]Estado[/]")
                    .AddColumn("[bold grey]# vuelos[/]")
                    .AddColumn("[bold grey]# tiquetes[/]")
                    .AddColumn("[bold grey]Monto total[/]");

                foreach (var b in bookings)
                    table.AddRow(
                        Markup.Escape(b.ReservationReference),
                        b.BookedAt.ToString("yyyy-MM-dd HH:mm"),
                        Markup.Escape(b.Status),
                        b.FlightsCount.ToString(),
                        b.TicketsCount.ToString(),
                        b.TotalAmount.ToString("0.00"));

                AnsiConsole.Write(table);

                var pick = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("\nSelecciona una reserva para ver detalle:")
                        .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                        .AddChoices(bookings.Select(b => $"{b.BookingId}|{b.ReservationReference}").Append("Volver")));

                if (pick == "Volver")
                    return;

                var id = int.Parse(pick.Split('|')[0]);
                var details = await _service.GetBookingDetailsAsync(id, ctx.ClientId, cancellationToken);

                AnsiConsole.Clear();
                AnsiConsole.Write(new Rule($"[bold deepskyblue1] Reserva {Markup.Escape(bookings.First(b => b.BookingId == id).ReservationReference)} [/]").LeftJustified());
                AnsiConsole.MarkupLine($"[grey]Id interno:[/] [white]{details.BookingId}[/]");
                AnsiConsole.MarkupLine($"\n[grey]Estado:[/] [white]{Markup.Escape(details.Status)}[/]   [grey]Monto:[/] [white]{details.TotalAmount:0.00}[/]");
                AnsiConsole.MarkupLine($"[grey]Pago:[/] [white]{Markup.Escape(details.PaymentStatus)}[/]");
                AnsiConsole.MarkupLine($"[grey]Tiquetes en esta reserva:[/] [white]{details.Tickets.Count}[/]");

                var flightsTable = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Grey)
                    .AddColumn("[bold grey]Vuelo[/]")
                    .AddColumn("[bold grey]Ruta[/]")
                    .AddColumn("[bold grey]Salida[/]")
                    .AddColumn("[bold grey]Llegada[/]")
                    .AddColumn("[bold grey]Disponibles[/]");
                foreach (var f in details.Flights)
                    flightsTable.AddRow(Markup.Escape(f.FlightCode), Markup.Escape(f.RouteLabel), f.DepartureAt.ToString("yyyy-MM-dd HH:mm"), f.ArrivalAt.ToString("yyyy-MM-dd HH:mm"), f.AvailableSeats.ToString());
                AnsiConsole.Write(new Rule("[grey]Vuelos[/]").LeftJustified());
                AnsiConsole.Write(flightsTable);

                var paxTable = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Grey)
                    .AddColumn("[bold grey]Pasajero[/]")
                    .AddColumn("[bold grey]Documento[/]");
                foreach (var p in details.Passengers)
                    paxTable.AddRow(Markup.Escape(p.FullName), Markup.Escape(p.Document));
                AnsiConsole.Write(new Rule("[grey]Pasajeros[/]").LeftJustified());
                AnsiConsole.Write(paxTable);

                // Tabla de tiquetes de la compra
                var tTable = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Grey)
                    .AddColumn("[bold grey]Tiquete[/]")
                    .AddColumn("[bold grey]Pasajero[/]")
                    .AddColumn("[bold grey]Documento[/]")
                    .AddColumn("[bold grey]Vuelo[/]")
                    .AddColumn("[bold grey]Salida[/]")
                    .AddColumn("[bold grey]Asiento[/]")
                    .AddColumn("[bold grey]Escalas[/]")
                    .AddColumn("[bold grey]Equipaje extra[/]");

                foreach (var t in details.Tickets)
                    tTable.AddRow(
                        Markup.Escape(t.TicketCode),
                        Markup.Escape(t.PassengerFullName),
                        Markup.Escape(t.PassengerDocument),
                        Markup.Escape(t.FlightCode),
                        t.DepartureAt.ToString("yyyy-MM-dd HH:mm"),
                        string.IsNullOrEmpty(t.SeatCode) ? "[grey]—[/]" : $"[white]{Markup.Escape(t.SeatCode)}[/]",
                        t.StopoversCount.ToString(),
                        t.HasExtraBaggage ? "[green]Sí[/]" : "[grey]No[/]");

                AnsiConsole.Write(new Rule("[grey]Tiquetes de esta compra[/]").LeftJustified());
                AnsiConsole.Write(tTable);

                var actions = new List<string> { "Volver" };
                if (details.Status is "Pending" or "Confirmed" && !details.HasAnyCheckin)
                    actions.Insert(0, "Cancelar reserva");

                var action = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("\nAcción:")
                        .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                        .AddChoices(actions));

                if (action == "Cancelar reserva")
                {
                    if (!AnsiConsole.Confirm("¿Confirmas la cancelación?", false))
                        continue;

                    await _service.CancelBookingAsync(id, ctx.ClientId, cancellationToken);
                    AnsiConsole.MarkupLine("\n[green]Reserva cancelada. Los tiquetes asociados quedaron en estado Voided.[/]");
                    Pause();
                }
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(FormatError(ex))}[/]");
            Pause();
        }
    }

    private async Task OnlineCheckinAsync(CancellationToken cancellationToken)
    {
        try
        {
            var ctx = await _service.EnsureClientContextAsync(cancellationToken);
            var utcNow = DateTime.UtcNow;

            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Check-in en línea [/]").LeftJustified());

            var mode = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n¿Cómo deseas buscar tu reserva?")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices("Código de reserva", "Buscar por apellido", "Volver"));

            if (mode == "Volver")
                return;

            string? bookingLookup = null;
            string? lastName = null;
            if (mode == "Código de reserva")
                bookingLookup = PromptNonEmpty("Código de reserva (el que te mostramos al comprar):");
            else
                lastName = PromptNonEmpty("Apellido del pasajero:");

            var matches = (await _service.FindBookingsForCheckinAsync(ctx.ClientId, bookingLookup, lastName, utcNow, cancellationToken)).ToList();
            if (matches.Count == 0)
            {
                AnsiConsole.MarkupLine("\n[yellow]No se encontraron reservas con ese criterio.[/]");
                Pause();
                return;
            }

            int bookingPick;
            if (matches.Count == 1)
            {
                bookingPick = matches[0].BookingId;
            }
            else
            {
                var pick = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("\nSelecciona la reserva:")
                        .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                        .AddChoices(matches.Select(m => $"{m.BookingId}|{ReservationReferenceCodec.Encode(m.BookingId)}").Append("Cancelar")));

                if (pick == "Cancelar")
                    return;

                bookingPick = int.Parse(pick.Split('|')[0]);
            }

            var bookingMatch = matches.First(m => m.BookingId == bookingPick);

            var flightPickLabel = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\nVuelos disponibles en esa reserva:")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(
                        bookingMatch.Flights.Select(f =>
                            $"{f.FlightId}|{f.FlightCode}|{f.RouteLabel}|{f.DepartureAt:yyyy-MM-dd HH:mm}|{f.ArrivalAt:yyyy-MM-dd HH:mm}")
                        .Append("Volver")));

            if (flightPickLabel == "Volver")
                return;

            var flightId = int.Parse(flightPickLabel.Split('|')[0]);
            while (!cancellationToken.IsCancellationRequested)
            {
                var candidates = await _service.GetCheckinCandidatesAsync(ctx.ClientId, bookingPick, flightId, DateTime.UtcNow, cancellationToken);

                AnsiConsole.Clear();
                AnsiConsole.Write(new Rule("[bold deepskyblue1] Check-in en línea [/]").LeftJustified());
                AnsiConsole.MarkupLine($"[grey]Vuelo:[/] [white]{Markup.Escape(candidates.FlightCode)}[/]  [grey]Ruta:[/] [white]{Markup.Escape(candidates.OriginIata)} → {Markup.Escape(candidates.DestinationIata)}[/]");
                AnsiConsole.MarkupLine($"[grey]Salida:[/] [white]{candidates.DepartureAt:yyyy-MM-dd HH:mm}[/]  [grey]Llegada:[/] [white]{candidates.ArrivalAt:yyyy-MM-dd HH:mm}[/]\n");

                RenderCheckinPassengerTable("Pendientes por check-in", candidates.PendingPassengers, includeBoardingPass: false);
                RenderCheckinPassengerTable("Check-in listo", candidates.CheckedInPassengers, includeBoardingPass: true);

                if (candidates.PendingPassengers.Count == 0)
                {
                    AnsiConsole.MarkupLine("\n[green]Todos los pasajeros de este vuelo ya tienen check-in listo.[/]");
                    Pause();
                    return;
                }

                var action = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("\n¿Qué deseas hacer?")
                        .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                        .AddChoices(
                            "Hacer check-in a un pasajero",
                            "Hacer check-in a todos",
                            "Volver"));

                if (action == "Volver")
                    return;

                if (action == "Hacer check-in a todos")
                {
                    if (!CaptureCheckinRequirements())
                        continue;

                    if (!AnsiConsole.Confirm("Se asignará asiento aleatorio a cada pasajero pendiente. ¿Deseas continuar?", true))
                        continue;

                    var results = new List<(string PassengerName, string SeatCode, string BoardingPassNumber)>();
                    foreach (var passenger in candidates.PendingPassengers)
                    {
                        var checkIn = await _service.PerformOnlineCheckinAsync(
                            ctx.ClientId,
                            passenger.PassengerReservationId,
                            flightId,
                            desiredSeatCode: null,
                            allowRandomSeat: true,
                            utcNow: DateTime.UtcNow,
                            cancellationToken);

                        results.Add((checkIn.PassengerFullName, checkIn.SeatCode, checkIn.BoardingPassNumber));
                    }

                    AnsiConsole.Clear();
                    AnsiConsole.Write(new Rule("[bold deepskyblue1] Check-in completado [/]").LeftJustified());
                    var completedTable = new Table()
                        .Border(TableBorder.Rounded)
                        .BorderColor(Color.Grey)
                        .AddColumn("[bold grey]Pasajero[/]")
                        .AddColumn("[bold grey]Asiento[/]")
                        .AddColumn("[bold grey]Pase de abordar[/]");

                    foreach (var item in results)
                        completedTable.AddRow(
                            Markup.Escape(item.PassengerName),
                            Markup.Escape(item.SeatCode),
                            Markup.Escape(item.BoardingPassNumber));

                    AnsiConsole.Write(completedTable);
                    Pause();
                    continue;
                }

                var passengerPickLabel = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("\nSelecciona el pasajero pendiente:")
                        .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                        .AddChoices(candidates.PendingPassengers.Select(t =>
                            $"{t.PassengerReservationId}|{t.PassengerName}|{t.PassengerDocument}|{t.TicketCode}").Append("Volver")));

                if (passengerPickLabel == "Volver")
                    continue;

                if (!CaptureCheckinRequirements())
                    continue;

                var passengerReservationId = int.Parse(passengerPickLabel.Split('|')[0]);
                var (desiredSeat, allowRandom) = await PromptSeatSelectionAsync(flightId, cancellationToken);
                if (desiredSeat is null && !allowRandom)
                    continue;

                if (!AnsiConsole.Confirm("¿Confirmar asiento y completar check-in?", true))
                    continue;

                var singleCheckin = await _service.PerformOnlineCheckinAsync(
                    ctx.ClientId,
                    passengerReservationId,
                    flightId,
                    desiredSeat,
                    allowRandom,
                    DateTime.UtcNow,
                    cancellationToken);

                if (singleCheckin.AdditionalSeatChoiceCharge > 0m)
                    AnsiConsole.MarkupLine(
                        $"\n[yellow]Cargo por elegir asiento concreto:[/] [white]{singleCheckin.AdditionalSeatChoiceCharge:0.00}[/] [grey](registrado en tu reserva)[/]");
                else
                    AnsiConsole.MarkupLine(
                        $"\n[green]Asiento asignado automáticamente:[/] [white]{Markup.Escape(singleCheckin.SeatCode)}[/]");

                RenderBoardingPass(
                    singleCheckin.PassengerFullName,
                    singleCheckin.FlightCode,
                    $"{singleCheckin.OriginIata} → {singleCheckin.DestinationIata}",
                    singleCheckin.DepartureAt,
                    singleCheckin.ArrivalAt,
                    singleCheckin.SeatCode,
                    singleCheckin.CabinTypeName,
                    singleCheckin.BoardingPassNumber,
                    singleCheckin.AdditionalSeatChoiceCharge);

                Pause();
            }
        }
        catch (BackNavigationException)
        {
            return;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(FormatError(ex))}[/]");
        }

        Pause();
    }

    private async Task MyTicketsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var ctx = await _service.EnsureClientContextAsync(cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new Rule("[bold deepskyblue1] Mis tiquetes [/]").LeftJustified());

                var tickets = (await _service.GetMyTicketsAsync(ctx.ClientId, cancellationToken)).ToList();
                if (tickets.Count == 0)
                {
                    AnsiConsole.MarkupLine("\n[yellow]No tienes tiquetes.[/]");
                    Pause();
                    return;
                }

                var table = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Grey)
                    .AddColumn("[bold grey]Código tiquete[/]")
                    .AddColumn("[bold grey]Vuelo[/]")
                    .AddColumn("[bold grey]Ruta[/]")
                    .AddColumn("[bold grey]Fecha vuelo[/]")
                    .AddColumn("[bold grey]Asiento[/]")
                    .AddColumn("[bold grey]Estado[/]");

                foreach (var t in tickets.OrderByDescending(x => x.DepartureAt))
                    table.AddRow(
                        Markup.Escape(t.TicketCode),
                        Markup.Escape(t.FlightCode),
                        $"{Markup.Escape(t.OriginIata)} → {Markup.Escape(t.DestinationIata)}",
                        t.DepartureAt.ToString("yyyy-MM-dd HH:mm"),
                        string.IsNullOrEmpty(t.SeatCode) ? "[grey]—[/]" : $"[white]{Markup.Escape(t.SeatCode)}[/]",
                        Markup.Escape(t.TicketState));

                AnsiConsole.Write(table);

                var pick = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("\nSelecciona un tiquete:")
                        .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                        .AddChoices(tickets.Select(t => $"{t.TicketId}|{t.TicketCode}|{t.PassengerName}").Append("Volver")));

                if (pick == "Volver")
                    return;

                var ticketId = int.Parse(pick.Split('|')[0]);
                var details = await _service.GetTicketDetailsAsync(ticketId, ctx.ClientId, cancellationToken);

                AnsiConsole.Clear();
                AnsiConsole.Write(new Rule("[bold deepskyblue1] Detalle de tiquete [/]").LeftJustified());

                if (!details.IsCheckedIn)
                {
                    AnsiConsole.MarkupLine("\n[yellow]Check-in pendiente.[/]");
                    AnsiConsole.MarkupLine($"[grey]Tiquete:[/] [white]{Markup.Escape(details.Ticket.TicketCode)}[/]");
                    AnsiConsole.MarkupLine($"[grey]Asiento:[/] [grey]— (se asigna al hacer check-in)[/]");
                    Pause();
                    continue;
                }

                RenderBoardingPass(
                    details.Ticket.PassengerName,
                    details.Ticket.FlightCode,
                    $"{details.Ticket.OriginIata} → {details.Ticket.DestinationIata}",
                    details.Ticket.DepartureAt,
                    details.Ticket.EstimatedArrivalAt,
                    details.SeatCode ?? "—",
                    details.CabinTypeName ?? "—",
                    details.BoardingPassNumber ?? "—",
                    0m);

                Pause();
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"\n[red]{Markup.Escape(FormatError(ex))}[/]");
            Pause();
        }
    }

    private static TripType? PromptTripType()
    {
        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\nTipo de viaje:")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices("Ida y vuelta", "Solo ida", "Regresar al menú portal"));
        return option switch
        {
            "Ida y vuelta" => TripType.RoundTrip,
            "Solo ida" => TripType.OneWay,
            _ => null
        };
    }

    private async Task<int> PromptCityAsync(string label, CancellationToken cancellationToken)
    {
        while (true)
        {
            AnsiConsole.MarkupLine($"\n[bold]{Markup.Escape(label)}[/]");

            var mode = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[grey]¿Cómo deseas elegir la ciudad?[/]")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .PageSize(10)
                    .AddChoices("Ver catálogo completo", "Buscar por nombre", "Volver"));

            if (mode == "Volver")
                throw new BackNavigationException();

            if (mode == "Ver catálogo completo")
            {
                var all = (await _service.ListAllCitiesAsync(cancellationToken))
                    .OrderBy(x => x.City, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(x => x.Region, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(x => x.Country, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                var choices = all
                    .Select(c => new { c.Id, Label = $"{c.City} · {c.Region} · {c.Country}" })
                    .ToList();

                var choiceLabels = choices
                    .Select(c => c.Label)
                    .Append("Volver")
                    .ToList();

                var pick = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("\nSelecciona una ciudad del catálogo:")
                        .PageSize(15)
                        .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                        .AddChoices(choiceLabels));

                if (pick == "Volver")
                    continue;

                return choices.First(c => c.Label == pick).Id;
            }

            var query = PromptNonEmpty("Escribe parte del nombre de la ciudad:");
            var matches = (await _service.SearchCitiesAsync(query, cancellationToken)).ToList();
            if (matches.Count == 0)
            {
                AnsiConsole.MarkupLine("\n[yellow]No se encontraron ciudades. Intenta con otro texto.[/]");
                continue;
            }

            var pick2 = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\nSelecciona una ciudad:")
                    .PageSize(10)
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices(matches.Select(m => m.Label).Append("Volver")));

            if (pick2 == "Volver")
                continue;

            return matches.First(m => m.Label == pick2).Id;
        }
    }

    private async Task<int> PromptCabinTypeIdAsync(CancellationToken cancellationToken)
    {
        var cabinTypes = (await _service.GetCabinTypesAsync(cancellationToken)).ToList();
        if (cabinTypes.Count == 0)
        {
            // fallback: ids conocidos (si no hay catálogo)
            var pickId = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\nClase:")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices("1", "2", "3", "Volver"));
            if (pickId == "Volver")
                throw new BackNavigationException();
            return int.Parse(pickId);
        }

        // Mapeo a etiquetas “bonitas” si los nombres coinciden
        string MapLabel((int Id, string Name) c)
        {
            var n = c.Name.Trim();
            if (n.Equals("Economy", StringComparison.OrdinalIgnoreCase) || n.Equals("Económica", StringComparison.OrdinalIgnoreCase))
                return $"{c.Id}|Económica";
            if (n.Equals("Business", StringComparison.OrdinalIgnoreCase) || n.Equals("Ejecutiva", StringComparison.OrdinalIgnoreCase))
                return $"{c.Id}|Ejecutiva";
            if (n.Equals("First", StringComparison.OrdinalIgnoreCase) || n.Equals("Primera", StringComparison.OrdinalIgnoreCase) || n.Equals("Primera Clase", StringComparison.OrdinalIgnoreCase))
                return $"{c.Id}|Primera Clase";
            return $"{c.Id}|{n}";
        }

        var options = cabinTypes.Select(MapLabel).ToList();
        var pick = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\nClase:")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices(options.Append("Volver")));

        if (pick == "Volver")
            throw new BackNavigationException();

        return int.Parse(pick.Split('|')[0]);
    }

    private async Task<string> ResolveCabinTypeNameAsync(int cabinTypeId, CancellationToken cancellationToken)
    {
        var cabinTypes = await _service.GetCabinTypesAsync(cancellationToken);
        var name = cabinTypes.FirstOrDefault(c => c.Id == cabinTypeId).Name;
        if (!string.IsNullOrWhiteSpace(name))
            return name;

        return $"Cabina {cabinTypeId}";
    }

    private async Task<int> PromptDocumentTypeIdAsync(CancellationToken cancellationToken)
    {
        var docs = (await _service.GetDocumentTypesAsync(cancellationToken)).ToList();
        if (docs.Count == 0)
            return PromptPositiveInt("Tipo documento (ID):");

        var options = docs.Select(d => $"{d.Id}|{d.Name}").ToList();
        var pick = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\nTipo documento:")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices(options.Append("Volver")));

        if (pick == "Volver")
            throw new BackNavigationException();
        return int.Parse(pick.Split('|')[0]);
    }

    private static DateOnly? PromptOptionalDateOnly(string label)
    {
        var raw = PromptFieldValue(
            label,
            text => string.IsNullOrWhiteSpace(text) || DateOnly.TryParse(text, out _)
                ? ValidationResult.Success()
                : ValidationResult.Error("[red]Fecha inválida.[/]"),
            allowEmpty: true);
        if (string.IsNullOrWhiteSpace(raw))
            return null;
        if (!DateOnly.TryParse(raw.Trim(), out var d))
            throw new InvalidOperationException("Fecha inválida. Formato esperado: yyyy-MM-dd");
        return d;
    }

    private static DateOnly PromptDateOnly(string label)
    {
        var raw = PromptFieldValue(
            label,
            text => DateOnly.TryParse(text, out _)
                ? ValidationResult.Success()
                : ValidationResult.Error("[red]Fecha inválida.[/]"));
        return DateOnly.Parse(raw.Trim());
    }

    private static int PromptMinInt(string label, int min)
    {
        var raw = PromptFieldValue(
            label,
            v =>
            {
                if (!int.TryParse(v, out var parsed))
                    return ValidationResult.Error("[red]Ingresa un número válido.[/]");
                return parsed >= min ? ValidationResult.Success() : ValidationResult.Error($"[red]Debe ser >= {min}.[/]");
            },
            initialValue: min.ToString());
        return int.Parse(raw);
    }

    private static int PromptPositiveInt(string label)
    {
        var raw = PromptFieldValue(
            label,
            v =>
            {
                if (!int.TryParse(v, out var parsed))
                    return ValidationResult.Error("[red]Ingresa un número válido.[/]");
                return parsed > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]Debe ser mayor que cero.[/]");
            });
        return int.Parse(raw);
    }

    private static string PromptNonEmpty(string label)
        => PromptFieldValue(
            label,
            s => !string.IsNullOrWhiteSpace(s)
                ? ValidationResult.Success()
                : ValidationResult.Error("[red]Campo obligatorio.[/]"));

    private static string PromptDigitsOnly(string label, int minLength, int maxLength)
        => PromptFieldValue(
            label,
            value =>
            {
                var text = value?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(text))
                    return ValidationResult.Error("[red]Campo obligatorio.[/]");
                if (!text.All(char.IsDigit))
                    return ValidationResult.Error("[red]Solo se permiten números.[/]");
                if (text.Length < minLength || text.Length > maxLength)
                    return ValidationResult.Error($"[red]Debe tener entre {minLength} y {maxLength} dígitos.[/]");
                return ValidationResult.Success();
            });

    private static string PromptCardExpiration(string label)
        => PromptFieldValue(
            label,
            value =>
            {
                var text = value?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(text))
                    return ValidationResult.Error("[red]Campo obligatorio.[/]");
                if (text.Length != 5 || text[2] != '/')
                    return ValidationResult.Error("[red]Usa el formato MM/AA.[/]");

                var monthText = text[..2];
                var yearText = text[3..];
                if (!int.TryParse(monthText, out var month) || !int.TryParse(yearText, out var shortYear))
                    return ValidationResult.Error("[red]Usa solo números en el formato MM/AA.[/]");
                if (month < 1 || month > 12)
                    return ValidationResult.Error("[red]El mes debe estar entre 01 y 12.[/]");

                var now = DateTime.Now;
                var currentShortYear = now.Year % 100;
                if (shortYear < currentShortYear || (shortYear == currentShortYear && month < now.Month))
                    return ValidationResult.Error($"[red]La fecha no puede ser anterior a {now:MM/yy}.[/]");

                return ValidationResult.Success();
            });

    private static string PromptCountryCode(string label)
        => PromptFieldValue(
            label,
            value =>
            {
                var text = value?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(text))
                    return ValidationResult.Error("[red]Campo obligatorio.[/]");
                if (!text.All(char.IsDigit))
                    return ValidationResult.Error("[red]El código del país solo puede contener números.[/]");
                if (text.Length is < 1 or > 4)
                    return ValidationResult.Error("[red]El código del país debe tener entre 1 y 4 dígitos.[/]");
                return ValidationResult.Success();
            });

    private static string PromptPhoneNumber(string label)
        => PromptFieldValue(
            label,
            value =>
            {
                var text = value?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(text))
                    return ValidationResult.Error("[red]Campo obligatorio.[/]");
                if (!text.All(char.IsDigit))
                    return ValidationResult.Error("[red]El teléfono solo puede contener números.[/]");
                if (text.Length is < 7 or > 15)
                    return ValidationResult.Error("[red]El teléfono debe tener entre 7 y 15 dígitos.[/]");
                return ValidationResult.Success();
            });

    private static string PromptCountryName(string label)
        => PromptFieldValue(
            label,
            value =>
            {
                var text = value?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(text))
                    return ValidationResult.Error("[red]Campo obligatorio.[/]");
                if (text.All(char.IsDigit))
                    return ValidationResult.Error("[red]El país de residencia no puede ser solo números.[/]");
                return ValidationResult.Success();
            });

    private static string PromptSeatCode(string label)
        => PromptFieldValue(
            label,
            value =>
            {
                var text = value?.Trim().ToUpperInvariant() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(text))
                    return ValidationResult.Error("[red]Campo obligatorio.[/]");
                if (!System.Text.RegularExpressions.Regex.IsMatch(text, @"^\d{1,3}[A-Z]$"))
                    return ValidationResult.Error("[red]Usa el formato del asiento, por ejemplo 12A.[/]");
                return ValidationResult.Success();
            }).ToUpperInvariant();

    private static char PromptGender(string label)
    {
        var raw = AnsiConsole.Prompt(new TextPrompt<string>($"[deepskyblue1]{label}[/]")
            .Validate(s =>
            {
                var t = (s ?? "").Trim().ToUpperInvariant();
                return t is "M" or "F" ? ValidationResult.Success() : ValidationResult.Error("[red]Debe ser M o F.[/]");
            }));
        return raw.Trim().ToUpperInvariant()[0];
    }

    private static string PromptEmail(string label)
        => PromptFieldValue(
            label,
            s => (s?.Contains('@') == true && s.Contains('.'))
                ? ValidationResult.Success()
                : ValidationResult.Error("[red]Email inválido.[/]"));

    private static SimulatedPaymentMethod? PromptPaymentMethod()
    {
        var pick = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\nSelecciona método:")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices("Tarjeta crédito", "PSE", "Nequi", "Tarjeta débito", "Volver"));

        return pick switch
        {
            "Tarjeta crédito" => SimulatedPaymentMethod.CreditCard,
            "PSE" => SimulatedPaymentMethod.Pse,
            "Nequi" => SimulatedPaymentMethod.Nequi,
            "Tarjeta débito" => SimulatedPaymentMethod.DebitCard,
            "Volver" => null,
            _ => SimulatedPaymentMethod.Pse
        };
    }

    private static FlightSearchResult? PickFlight(List<FlightSearchResult> flights, string title)
    {
        var options = flights
            .Select(f =>
            {
                var duration = f.Flight.EstimatedArrivalAt - f.Flight.DepartureAt;
                var durLabel = $"{(int)duration.TotalHours} h {duration.Minutes}m";
                var label =
                    $"Vuelo {f.Flight.FlightCode} | {f.OriginAirport.IataCode} → {f.DestinationAirport.IataCode} | " +
                    $"Salida: {f.Flight.DepartureAt:yyyy-MM-dd HH:mm} | Llegada: {f.Flight.EstimatedArrivalAt:yyyy-MM-dd HH:mm} | " +
                    $"Duración: {durLabel} | Asientos disponibles: {f.Flight.AvailableSeats} | Precio base: {f.BasePrice:0.00}";
                return (Label: label, FlightId: f.Flight.Id);
            })
            .ToList();

        var pick = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"\n{title}")
                .PageSize(10)
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                .AddChoices(options.Select(o => o.Label).Append("Volver")));

        if (pick == "Volver")
            return null;

        var id = options.First(o => o.Label == pick).FlightId;
        return flights.First(f => f.Flight.Id == id);
    }

    private static void RenderFlightDetail(string header, FlightSearchResult f, int cabinTypeId)
    {
        var duration = f.Flight.EstimatedArrivalAt - f.Flight.DepartureAt;
        var durLabel = $"{(int)duration.TotalHours} h {duration.Minutes}m";

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold grey]Campo[/]")
            .AddColumn("[bold grey]Valor[/]");

        table.AddRow("Vuelo", Markup.Escape(f.Flight.FlightCode));
        table.AddRow("Fecha", f.Flight.DepartureAt.ToString("yyyy-MM-dd"));
        table.AddRow("Hora salida", f.Flight.DepartureAt.ToString("HH:mm"));
        table.AddRow("Hora llegada", f.Flight.EstimatedArrivalAt.ToString("HH:mm"));
        table.AddRow("Aeropuerto origen", Markup.Escape($"{f.OriginAirport.Name} ({f.OriginAirport.IataCode})"));
        table.AddRow("Aeropuerto destino", Markup.Escape($"{f.DestinationAirport.Name} ({f.DestinationAirport.IataCode})"));
        table.AddRow("Duración", durLabel);
        table.AddRow("Clase", cabinTypeId.ToString());

        AnsiConsole.Write(new Rule($"[grey]{Markup.Escape(header)}[/]").LeftJustified());
        AnsiConsole.Write(table);
    }

    private static void RenderTicketsTable(IReadOnlyList<TicketSummary> tickets)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold grey]Tiquete[/]")
            .AddColumn("[bold grey]Pasajero[/]")
            .AddColumn("[bold grey]Vuelo[/]")
            .AddColumn("[bold grey]Ruta[/]")
            .AddColumn("[bold grey]Salida[/]")
            .AddColumn("[bold grey]Asiento[/]");

        foreach (var t in tickets.OrderBy(x => x.DepartureAt))
            table.AddRow(
                Markup.Escape(t.TicketCode),
                Markup.Escape(t.PassengerName),
                Markup.Escape(t.FlightCode),
                $"{Markup.Escape(t.OriginIata)} → {Markup.Escape(t.DestinationIata)}",
                t.DepartureAt.ToString("yyyy-MM-dd HH:mm"),
                string.IsNullOrEmpty(t.SeatCode) ? "[grey]— (check-in)[/]" : $"[white]{Markup.Escape(t.SeatCode)}[/]");

        AnsiConsole.Write(new Rule("[grey]Tiquetes generados[/]").LeftJustified());
        AnsiConsole.Write(table);
    }

    private static bool CaptureCheckinRequirements()
    {
        try
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Seguridad [/]").LeftJustified());
            AnsiConsole.MarkupLine("\n[bold]ARTÍCULOS PERMITIDOS EN CABINA (con restricciones):[/]");
            AnsiConsole.MarkupLine("- Cargadores externos, baterías de litio, cigarrillos electrónicos");
            AnsiConsole.MarkupLine("\n[bold]ARTÍCULOS PROHIBIDOS:[/]");
            AnsiConsole.MarkupLine("- Fuegos artificiales, líquidos inflamables, sólidos inflamables, gases inflamables, productos radioactivos");
            AnsiConsole.WriteLine();

            var securityAction = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("¿Qué deseas hacer?")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices("Aceptar y continuar", "Volver"));

            if (securityAction == "Volver")
                return false;

            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Contacto de emergencia [/]").LeftJustified());
            _ = PromptNonEmpty("Nombre del contacto:");
            _ = PromptCountryCode("Código del país (solo números, ej. 57):");
            _ = PromptPhoneNumber("Teléfono:");
            _ = PromptCountryName("País de residencia:");

            return true;
        }
        catch (BackNavigationException)
        {
            return false;
        }
    }

    private async Task<(string? DesiredSeat, bool AllowRandom)> PromptSeatSelectionAsync(int flightId, CancellationToken cancellationToken)
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold deepskyblue1] Selección de asiento [/]").LeftJustified());

            var availableSeats = (await _service.GetAvailableSeatCodesAsync(flightId, cancellationToken)).ToList();
            if (availableSeats.Count == 0)
                throw new InvalidOperationException("No hay asientos disponibles.");

            RenderAvailableSeats(availableSeats);

            var seatMode = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n¿Cómo deseas seleccionar asiento?")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices("Asignar aleatorio (sin pagar extra)", "Elegir por código", "Volver"));

            if (seatMode == "Volver")
                return (null, false);

            if (seatMode == "Asignar aleatorio (sin pagar extra)")
                return (null, true);

            var desiredSeat = PromptSeatCode("Escribe el código del asiento (ej. 12A):");
            if (availableSeats.Contains(desiredSeat, StringComparer.OrdinalIgnoreCase))
                return (desiredSeat, false);

            AnsiConsole.MarkupLine("\n[red]Ese asiento no aparece entre los disponibles para este vuelo.[/]");
            Pause();
        }
    }

    private static void RenderAvailableSeats(IReadOnlyList<string> availableSeats)
    {
        AnsiConsole.MarkupLine("[grey]Asientos disponibles en este momento:[/]\n");

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey);

        for (var i = 0; i < 6; i++)
            table.AddColumn($"[bold grey]{i + 1}[/]");

        for (var i = 0; i < availableSeats.Count; i += 6)
        {
            var row = availableSeats
                .Skip(i)
                .Take(6)
                .Select(code => $"[white]{Markup.Escape(code)}[/]")
                .ToList();

            while (row.Count < 6)
                row.Add("[grey]—[/]");

            table.AddRow(row.ToArray());
        }

        AnsiConsole.Write(table);
    }

    private static void RenderCheckinPassengerTable(string title, IReadOnlyList<CheckinPassengerRow> passengers, bool includeBoardingPass)
    {
        AnsiConsole.Write(new Rule($"[grey]{Markup.Escape(title)}[/]").LeftJustified());

        if (passengers.Count == 0)
        {
            AnsiConsole.MarkupLine("[grey]No hay registros en esta sección.[/]\n");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn("[bold grey]Pasajero[/]")
            .AddColumn("[bold grey]Documento[/]")
            .AddColumn("[bold grey]Tiquete[/]")
            .AddColumn("[bold grey]Estado[/]")
            .AddColumn("[bold grey]Asiento[/]");

        if (includeBoardingPass)
            table.AddColumn("[bold grey]Pase de abordar[/]");

        foreach (var passenger in passengers)
        {
            var cells = new List<string>
            {
                Markup.Escape(passenger.PassengerName),
                Markup.Escape(passenger.PassengerDocument),
                Markup.Escape(passenger.TicketCode),
                includeBoardingPass ? "[green]Check-in listo[/]" : Markup.Escape(passenger.TicketState),
                string.IsNullOrWhiteSpace(passenger.SeatCode) ? "[grey]—[/]" : Markup.Escape(passenger.SeatCode)
            };

            if (includeBoardingPass)
                cells.Add(string.IsNullOrWhiteSpace(passenger.BoardingPassNumber) ? "[grey]—[/]" : Markup.Escape(passenger.BoardingPassNumber));

            table.AddRow(cells.ToArray());
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    private static void RenderBoardingPass(
        string passenger,
        string flightCode,
        string route,
        DateTime departureAt,
        DateTime arrivalAt,
        string seatCode,
        string cabinType,
        string boardingPass,
        decimal additionalSeatCharge = 0m)
    {
        var boardingAt = departureAt.AddMinutes(-60);
        var feeLine = additionalSeatCharge > 0m
            ? $"\nCargo elección asiento: {additionalSeatCharge:0.00}"
            : "";
        var content =
$@"══════════════════════════════════════
       PASE DE ABORDAR
══════════════════════════════════════
Pasajero: {passenger}
Vuelo:    {flightCode}
Ruta:     {route}
Fecha:    {departureAt:yyyy-MM-dd}
Salida:   {departureAt:HH:mm}
Llegada:  {arrivalAt:HH:mm}
Asiento:  {seatCode}  (único en este vuelo)
Clase:    {cabinType}
Grupo:    F
Abordaje: {boardingAt:HH:mm}
Código:   {boardingPass}{feeLine}
══════════════════════════════════════";

        AnsiConsole.Write(new Panel(new Markup(Markup.Escape(content))).Border(BoxBorder.Rounded).BorderColor(Color.DeepSkyBlue1));
    }

    private static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Prompt(new TextPrompt<string>("[grey]Presiona Enter para continuar...[/]").AllowEmpty());
    }

    private static string PromptFieldValue(
        string label,
        Func<string, ValidationResult> validator,
        string? initialValue = null,
        bool allowEmpty = false)
    {
        var currentValue = initialValue ?? string.Empty;

        while (true)
        {
            var edited = ReadFieldInput(label, currentValue, allowEmpty);
            if (edited is null)
                throw new BackNavigationException();

            currentValue = edited;

            var validation = validator(currentValue);
            if (validation.Successful)
            {
                var postEntryAction = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"\nValor ingresado: [white]{Markup.Escape(currentValue)}[/]\n¿Qué deseas hacer?")
                        .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                        .AddChoices("Aceptar", "Editar", "Volver"));

                if (postEntryAction == "Aceptar")
                    return currentValue.Trim();

                if (postEntryAction == "Volver")
                    throw new BackNavigationException();

                continue;
            }

            AnsiConsole.MarkupLine(validation.Message ?? "[red]Valor inválido.[/]");
            var nextAction = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("¿Qué deseas hacer?")
                    .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1))
                    .AddChoices("Reescribir campo", "Volver"));

            if (nextAction == "Volver")
                throw new BackNavigationException();
        }
    }

    private static string? ReadFieldInput(string label, string currentValue, bool allowEmpty)
    {
        AnsiConsole.MarkupLine($"\n[deepskyblue1]{Markup.Escape(label)}[/] [grey](Esc para volver)[/]");
        Console.Write("> ");
        Console.Write(currentValue);

        var buffer = new System.Text.StringBuilder(currentValue);

        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Escape)
            {
                Console.WriteLine();
                return null;
            }

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                var value = buffer.ToString();
                if (!allowEmpty && string.IsNullOrWhiteSpace(value))
                    continue;
                return value;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (buffer.Length == 0)
                    continue;

                buffer.Length--;
                Console.Write("\b \b");
                continue;
            }

            if (char.IsControl(key.KeyChar))
                continue;

            buffer.Append(key.KeyChar);
            Console.Write(key.KeyChar);
        }
    }
}
