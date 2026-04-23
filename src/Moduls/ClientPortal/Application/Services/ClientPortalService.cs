using GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.People.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Infrastructure.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.CabinTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.ClientPortal.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.ClientPortal.Application.Models;
using GestorDeVuelosProyectoFinal.src.Moduls.ClientPortal.Application.Support;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightReservations.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Session;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.ClientPortal.Application.Services;

public sealed class ClientPortalService : IClientPortalService
{
    private readonly AppDbContext _db;

    /// <summary>Tarifas demo (misma moneda que rates).</summary>
    private const decimal ExtraBaggagePerPiecePerFlightLeg = 45m;
    private const decimal PrioritySeatChoicePerPassengerPerLeg = 25m;
    private const decimal CheckInSpecificSeatChoiceFee = 15m;

    public ClientPortalService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ClientContext> EnsureClientContextAsync(CancellationToken cancellationToken)
    {
        var session = UserSession.Current;
        if (session is null)
            throw new InvalidOperationException("No hay una sesión autenticada.");

        var roleName = (session.RoleName ?? "").Trim();
        if (string.IsNullOrWhiteSpace(roleName))
            throw new InvalidOperationException("La sesión no tiene rol.");

        await EnsureLookupsAsync(cancellationToken);

        var role = await _db.SystemRoles.FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);
        if (role is null)
        {
            role = new SystemRolesEntity { Name = roleName, Description = $"Rol '{roleName}' creado automáticamente" };
            _db.SystemRoles.Add(role);
            await _db.SaveChangesAsync(cancellationToken);
        }

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == session.UserId, cancellationToken);
        if (user is null)
            throw new InvalidOperationException("No se encontró el usuario autenticado en la tabla users.");

        if (user.Role_Id != role.Id)
        {
            user.Role_Id = role.Id;
            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);
        }

        if (user.Person_Id is null)
        {
            var docTypeId = await GetAnyDocumentTypeIdAsync(cancellationToken);
            var docNumber = $"USR-{user.Id}";
            var person = await GetOrCreatePersonAsync(docTypeId, docNumber, "Cliente", user.Username, cancellationToken);

            user.Person_Id = person.Id;
            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);
        }

        var client = await _db.Customers.FirstOrDefaultAsync(c => c.PersonId == user.Person_Id.Value, cancellationToken);
        if (client is null)
        {
            client = new CustomerEntity
            {
                PersonId = user.Person_Id.Value,
                CreatedAt = DateTime.UtcNow
            };
            _db.Customers.Add(client);
            await _db.SaveChangesAsync(cancellationToken);
        }

        return new ClientContext(user.Id, user.Person_Id.Value, client.Id, user.Username, roleName);
    }

    public async Task<IReadOnlyList<(int Id, string Label)>> SearchCitiesAsync(string query, CancellationToken cancellationToken)
    {
        var q = (query ?? "").Trim();
        if (q.Length == 0)
            return Array.Empty<(int, string)>();

        var cities = await _db.Cities.AsNoTracking().ToListAsync(cancellationToken);
        var regions = await _db.Regions.AsNoTracking().ToListAsync(cancellationToken);
        var countries = await _db.Countries.AsNoTracking().ToListAsync(cancellationToken);

        var filtered = cities
            .Where(c => c.Name.Contains(q, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .ToList();

        var joined =
            from c in filtered
            join r in regions on c.RegionId equals r.Id
            join co in countries on r.CountryId equals co.Id
            select (c.Id, $"{c.Name}  [grey]·[/]  {r.Name}  [grey]·[/]  {co.Name}");

        return joined.ToList();
    }

    public async Task<IReadOnlyList<(int Id, string City, string Region, string Country)>> ListAllCitiesAsync(CancellationToken cancellationToken)
    {
        var cities = await _db.Cities.AsNoTracking().ToListAsync(cancellationToken);
        var regions = await _db.Regions.AsNoTracking().ToListAsync(cancellationToken);
        var countries = await _db.Countries.AsNoTracking().ToListAsync(cancellationToken);

        var joined =
            from c in cities
            join r in regions on c.RegionId equals r.Id
            join co in countries on r.CountryId equals co.Id
            orderby c.Name, r.Name, co.Name
            select (c.Id, c.Name, r.Name, co.Name);

        return joined.ToList();
    }

    public async Task<IReadOnlyList<(int Id, string Name)>> GetCabinTypesAsync(CancellationToken cancellationToken)
    {
        var list = await _db.CabinTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);
        return list.Select(x => (x.Id, x.Name)).ToList();
    }

    public async Task<IReadOnlyList<(int Id, string Name)>> GetDocumentTypesAsync(CancellationToken cancellationToken)
    {
        var list = await _db.DocumentTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);
        return list.Select(x => (x.Id, x.Name)).ToList();
    }

    public async Task<IReadOnlyList<(string FlightCode, string Origin, string Destination, DateTime DepartureAt, DateTime ArrivalAt, int AvailableSeats)>> ListAvailableFlightsAsync(
        CancellationToken cancellationToken)
    {
        var flights = await _db.Flights.AsNoTracking().OrderBy(f => f.DepartureAt).ToListAsync(cancellationToken);
        var routes = await _db.Routes.AsNoTracking().ToListAsync(cancellationToken);
        var airports = await _db.Airports.AsNoTracking().ToListAsync(cancellationToken);

        var joined =
            from f in flights
            join r in routes on f.RouteId equals r.Id
            join ao in airports on r.OriginAirportId equals ao.Id
            join ad in airports on r.DestinationAirportId equals ad.Id
            select (f.FlightCode, ao.IataCode, ad.IataCode, f.DepartureAt, f.EstimatedArrivalAt, f.AvailableSeats);

        return joined.ToList();
    }

    public async Task<IReadOnlyList<FlightSearchResult>> SearchFlightsAsync(
        int originCityId,
        int destinationCityId,
        DateOnly? date,
        int cabinTypeId,
        CancellationToken cancellationToken)
    {
        var flights = await _db.Flights.AsNoTracking().ToListAsync(cancellationToken);
        var routes = await _db.Routes.AsNoTracking().ToListAsync(cancellationToken);
        var airports = await _db.Airports.AsNoTracking().ToListAsync(cancellationToken);
        var cities = await _db.Cities.AsNoTracking().ToListAsync(cancellationToken);
        var regions = await _db.Regions.AsNoTracking().ToListAsync(cancellationToken);
        var countries = await _db.Countries.AsNoTracking().ToListAsync(cancellationToken);
        var rates = await _db.Rates.AsNoTracking().ToListAsync(cancellationToken);

        IEnumerable<FlightEntity> flightQuery = flights;

        if (date.HasValue)
        {
            var d = date.Value;
            flightQuery = flightQuery.Where(f => DateOnly.FromDateTime(f.DepartureAt) == d);
        }

        var joined =
            from f in flightQuery
            join r in routes on f.RouteId equals r.Id
            join ao in airports on r.OriginAirportId equals ao.Id
            join ad in airports on r.DestinationAirportId equals ad.Id
            join co in cities on ao.CityId equals co.Id
            join cd in cities on ad.CityId equals cd.Id
            join ro in regions on co.RegionId equals ro.Id
            join rd in regions on cd.RegionId equals rd.Id
            join ctryO in countries on ro.CountryId equals ctryO.Id
            join ctryD in countries on rd.CountryId equals ctryD.Id
            where co.Id == originCityId && cd.Id == destinationCityId
            select new { f, r, ao, ad, co, cd, ro, rd, ctryO, ctryD };

        var adultPassengerTypeId = await GetPassengerTypeIdAsync("Adult", cancellationToken);

        var results = joined
            .Select(x =>
            {
                var basePrice = ResolveBasePrice(rates, x.r.Id, cabinTypeId, adultPassengerTypeId, x.f.DepartureAt);
                return new FlightSearchResult(x.f, x.r, x.ao, x.ad, x.co, x.cd, x.ro, x.rd, x.ctryO, x.ctryD, basePrice);
            })
            .OrderBy(x => x.Flight.DepartureAt)
            .ToList();

        return results;
    }

    public async Task<(decimal TotalAmount, IReadOnlyList<(string Leg, decimal Amount)> Lines)> PreviewPurchaseAsync(
        PurchaseRequest request,
        CancellationToken cancellationToken)
    {
        await EnsureLookupsAsync(cancellationToken);
        ValidatePassengerComposition(request.Passengers);

        var outbound = await _db.Flights.AsNoTracking().FirstOrDefaultAsync(f => f.Id == request.OutboundFlightId, cancellationToken)
            ?? throw new InvalidOperationException("Vuelo de ida no encontrado.");
        FlightEntity? inbound = null;
        if (request.ReturnFlightId is not null)
        {
            inbound = await _db.Flights.AsNoTracking().FirstOrDefaultAsync(f => f.Id == request.ReturnFlightId.Value, cancellationToken)
                ?? throw new InvalidOperationException("Vuelo de regreso no encontrado.");
        }

        var routes = await _db.Routes.AsNoTracking().ToListAsync(cancellationToken);
        var rates = await _db.Rates.AsNoTracking().ToListAsync(cancellationToken);

        var outboundRoute = routes.FirstOrDefault(r => r.Id == outbound.RouteId)
            ?? throw new InvalidOperationException("Ruta de ida no encontrada.");
        RouteEntity? inboundRoute = null;
        if (inbound is not null)
            inboundRoute = routes.FirstOrDefault(r => r.Id == inbound.RouteId)
                ?? throw new InvalidOperationException("Ruta de regreso no encontrada.");

        var lineAmounts = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

        foreach (var p in request.Passengers)
        {
            var passengerTypeName = p.PassengerCategory switch
            {
                "Senior" => "Senior",
                "Child" => "Child",
                _ => "Adult"
            };
            var passengerTypeId = await GetPassengerTypeIdAsync(passengerTypeName, cancellationToken);

            var priceOut = ResolveBasePrice(rates, outboundRoute.Id, request.Search.CabinTypeId, passengerTypeId, outbound.DepartureAt);
            AddLine(lineAmounts, $"Ida · {PassengerCategoryLabel(passengerTypeName)}", priceOut);

            if (inbound is not null && inboundRoute is not null)
            {
                var priceIn = ResolveBasePrice(rates, inboundRoute.Id, request.Search.CabinTypeId, passengerTypeId, inbound.DepartureAt);
                AddLine(lineAmounts, $"Regreso · {PassengerCategoryLabel(passengerTypeName)}", priceIn);
            }
        }

        var legCount = request.ReturnFlightId is not null ? 2 : 1;

        if (request.ExtraBaggagePieces > 0)
        {
            var extraBagTotal = request.ExtraBaggagePieces * ExtraBaggagePerPiecePerFlightLeg * legCount;
            AddLine(
                lineAmounts,
                $"Equipaje extra ({request.ExtraBaggagePieces} pieza(s), {legCount} tramo(s))",
                extraBagTotal);
        }

        var lines = lineAmounts
            .OrderBy(x => x.Key, StringComparer.OrdinalIgnoreCase)
            .Select(x => (x.Key, x.Value))
            .ToList();

        var total = lines.Sum(x => x.Value);
        return (total, lines);

        static void AddLine(Dictionary<string, decimal> dict, string key, decimal amount)
        {
            if (amount == 0m)
                return;
            if (!dict.TryAdd(key, amount))
                dict[key] += amount;
        }

        static string PassengerCategoryLabel(string name) => name switch
        {
            "Senior" => "Adulto mayor",
            "Child" => "Menor",
            _ => "Adulto"
        };
    }

    public async Task<PurchaseResult> PurchaseAsync(PurchaseRequest request, CancellationToken cancellationToken)
    {
        await EnsureLookupsAsync(cancellationToken);
        ValidatePassengerComposition(request.Passengers);

        var ctx = await EnsureClientContextAsync(cancellationToken);

        var (expectedTotal, _) = await PreviewPurchaseAsync(request, cancellationToken);
        if (Math.Abs(expectedTotal - request.TotalAmount) > 0.05m)
            throw new InvalidOperationException($"El total ({request.TotalAmount:0.00}) no coincide con el desglose ({expectedTotal:0.00}).");

        var pendingStatusId = await GetBookingStatusIdAsync("Pending", cancellationToken);
        var confirmedStatusId = await GetBookingStatusIdAsync("Confirmed", cancellationToken);
        var paidStatusId = await GetPaymentStatusIdAsync("Paid", cancellationToken);
        var issuedTicketStateId = await GetTicketStateIdAsync("Issued", cancellationToken);

        var paymentMethodId = await EnsurePaymentMethodAsync(request.PaymentMethod, cancellationToken);

        var utcNow = DateTime.UtcNow;

        var booking = new BookingEntity
        {
            ClientId = ctx.ClientId,
            BookedAt = utcNow,
            BookingStatusId = pendingStatusId,
            TotalAmount = request.TotalAmount,
            ExpiresAt = utcNow.AddMinutes(25),
            CreatedAt = utcNow,
            UpdatedAt = utcNow
        };
        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync(cancellationToken);

        var flights = await _db.Flights.AsNoTracking()
            .Where(f => f.Id == request.OutboundFlightId || (request.ReturnFlightId.HasValue && f.Id == request.ReturnFlightId.Value))
            .ToListAsync(cancellationToken);
        var routes = await _db.Routes.AsNoTracking().ToListAsync(cancellationToken);
        var airports = await _db.Airports.AsNoTracking().ToListAsync(cancellationToken);
        var cities = await _db.Cities.AsNoTracking().ToListAsync(cancellationToken);
        var cabinType = await _db.CabinTypes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.Search.CabinTypeId, cancellationToken);
        var ticketStates = await _db.TicketStates.AsNoTracking().ToListAsync(cancellationToken);

        var bookingFlights = new List<BookingFlightsEntity>();
        bookingFlights.Add(new BookingFlightsEntity
        {
            BookingId = booking.Id,
            FlightId = request.OutboundFlightId,
            PartialAmount = request.TotalAmount / (request.ReturnFlightId.HasValue ? 2m : 1m)
        });
        if (request.ReturnFlightId.HasValue)
        {
            bookingFlights.Add(new BookingFlightsEntity
            {
                BookingId = booking.Id,
                FlightId = request.ReturnFlightId.Value,
                PartialAmount = request.TotalAmount / 2m
            });
        }

        _db.BookingFlights.AddRange(bookingFlights);
        await _db.SaveChangesAsync(cancellationToken);

        var flightReservations = new List<FlightReservationsEntity>();
        foreach (var bf in bookingFlights)
        {
            var fr = new FlightReservationsEntity { BookingFlightId = bf.Id };
            flightReservations.Add(fr);
        }
        _db.FlightReservations.AddRange(flightReservations);
        await _db.SaveChangesAsync(cancellationToken);

        var passengerSummaries = new List<(int PersonId, int PassengerId, string FullName, string Document)>();
        foreach (var p in request.Passengers)
        {
            var person = await GetOrCreatePersonAsync(p.DocumentTypeId, p.DocumentNumber.Trim(), p.FirstName.Trim(), p.LastName.Trim(), cancellationToken);

            var passengerTypeName = p.PassengerCategory switch
            {
                "Senior" => "Senior",
                "Child" => "Child",
                _ => "Adult"
            };
            var passengerTypeId = await GetPassengerTypeIdAsync(passengerTypeName, cancellationToken);
            var passenger = await GetOrCreatePassengerAsync(person.Id, passengerTypeId, cancellationToken);

            passengerSummaries.Add((person.Id, passenger.Id, $"{person.FirstName} {person.LastName}", $"{person.DocumentTypeId}-{person.DocumentNumber}"));
        }

        await UpsertContactAsync(ctx.PersonId, request.Contact, cancellationToken);

        var passengerReservations = new List<PassengerReservationsEntity>();
        foreach (var fr in flightReservations)
        {
            foreach (var passenger in passengerSummaries)
            {
                passengerReservations.Add(new PassengerReservationsEntity
                {
                    Flight_Reservation_Id = fr.Id,
                    Passenger_Id = passenger.PassengerId
                });
            }
        }
        _db.PassengerReservations.AddRange(passengerReservations);
        await _db.SaveChangesAsync(cancellationToken);

        var tickets = new List<TicketEntity>();
        foreach (var pr in passengerReservations)
        {
            tickets.Add(new TicketEntity
            {
                Code = $"TKT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4].ToUpper()}",
                IssueDate = utcNow,
                CreatedAt = utcNow,
                UpdatedAt = utcNow,
                PassengerReservation_Id = pr.Id,
                TicketState_Id = issuedTicketStateId
            });
        }
        _db.Tickets.AddRange(tickets);
        await _db.SaveChangesAsync(cancellationToken);

        await DeductSoldSeatsFromFlightsAsync(request.OutboundFlightId, request.ReturnFlightId, request.Passengers.Count, utcNow, cancellationToken);

        var payment = new PaymentsEntity
        {
            BookingId = booking.Id,
            Amount = request.TotalAmount,
            PaidAt = utcNow,
            PaymentStatusId = paidStatusId,
            PaymentMethodId = paymentMethodId,
            CreatedAt = utcNow,
            UpdatedAt = utcNow
        };
        _db.Payments.Add(payment);

        booking.BookingStatusId = confirmedStatusId;
        booking.UpdatedAt = utcNow;
        await _db.SaveChangesAsync(cancellationToken);

        var summary = await BuildTicketSummariesAsync(tickets.Select(t => t.Id).ToList(), ticketStates, flights, routes, airports, cities, cabinType?.Name, cancellationToken);

        var reference = ReservationReferenceCodec.Encode(booking.Id);
        return new PurchaseResult(booking.Id, reference, summary);
    }

    private static void ValidatePassengerComposition(IReadOnlyList<PassengerInput> passengers)
    {
        if (passengers is null || passengers.Count == 0)
            throw new InvalidOperationException("Debes reservar al menos 1 pasajero.");

        var adultOrSeniorCount = passengers.Count(p =>
            string.Equals(p.PassengerCategory, "Adult", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(p.PassengerCategory, "Senior", StringComparison.OrdinalIgnoreCase));

        if (adultOrSeniorCount <= 0)
            throw new InvalidOperationException("Debe viajar al menos 1 adulto o 1 adulto mayor en la reserva.");
    }

    public async Task<IReadOnlyList<(int BookingId, string ReservationReference, DateTime BookedAt, string Status, int FlightsCount, int TicketsCount, decimal TotalAmount)>> GetMyBookingsAsync(
        int clientId,
        CancellationToken cancellationToken)
    {
        await EnsureLookupsAsync(cancellationToken);
        var voidedTicketStateId = await GetTicketStateIdAsync("Voided", cancellationToken);
        var cancelledBookingStatusId = await GetBookingStatusIdAsync("Cancelled", cancellationToken);

        var bookings = await _db.Bookings.AsNoTracking()
            .Where(b => b.ClientId == clientId && b.BookingStatusId != cancelledBookingStatusId)
            .OrderByDescending(b => b.BookedAt)
            .ToListAsync(cancellationToken);

        var bookingStatuses = await _db.BookingStatuses.AsNoTracking().ToListAsync(cancellationToken);
        var bookingFlights = await _db.BookingFlights.AsNoTracking().ToListAsync(cancellationToken);
        var bookingIds = bookings.Select(b => b.Id).ToList();

        var relevantBookingFlights = bookingFlights.Where(bf => bookingIds.Contains(bf.BookingId)).ToList();
        var frs = await _db.FlightReservations.AsNoTracking()
            .Where(fr => relevantBookingFlights.Select(bf => bf.Id).Contains(fr.BookingFlightId))
            .ToListAsync(cancellationToken);
        var prs = await _db.PassengerReservations.AsNoTracking()
            .Where(pr => frs.Select(fr => fr.Id).Contains(pr.Flight_Reservation_Id))
            .ToListAsync(cancellationToken);
        var tickets = await _db.Tickets.AsNoTracking()
            .Where(t => prs.Select(pr => pr.Id).Contains(t.PassengerReservation_Id) && t.TicketState_Id != voidedTicketStateId)
            .ToListAsync(cancellationToken);

        var frToBooking =
            (from fr in frs
                join bf in relevantBookingFlights on fr.BookingFlightId equals bf.Id
                select (fr.Id, bf.BookingId)).ToList();

        var prBookingMap =
            (from pr in prs
                join link in frToBooking on pr.Flight_Reservation_Id equals link.Id
                select (pr.Id, link.BookingId)).ToDictionary(x => x.Id, x => x.BookingId);

        var ticketsPerBooking = new Dictionary<int, int>();
        foreach (var t in tickets)
        {
            if (!prBookingMap.TryGetValue(t.PassengerReservation_Id, out var bookingId))
                continue;
            ticketsPerBooking[bookingId] = ticketsPerBooking.TryGetValue(bookingId, out var c) ? c + 1 : 1;
        }

        var result =
            from b in bookings
            join s in bookingStatuses on b.BookingStatusId equals s.Id
            let flightsCount = bookingFlights.Count(x => x.BookingId == b.Id)
            let ticketCount = ticketsPerBooking.TryGetValue(b.Id, out var tc) ? tc : 0
            select (
                b.Id,
                ReservationReferenceCodec.Encode(b.Id),
                b.BookedAt,
                s.Name,
                flightsCount,
                ticketCount,
                b.TotalAmount
            );

        return result.ToList();
    }

    public async Task<(int BookingId, string Status, decimal TotalAmount,
        IReadOnlyList<(string FlightCode, string RouteLabel, DateTime DepartureAt, DateTime ArrivalAt, int AvailableSeats)> Flights,
        IReadOnlyList<(string FullName, string Document)> Passengers,
        string PaymentStatus,
        bool HasAnyCheckin,
        IReadOnlyList<int> TicketIds,
        IReadOnlyList<BookingTicketRow> Tickets)> GetBookingDetailsAsync(int bookingId, int clientId, CancellationToken cancellationToken)
    {
        var booking = await _db.Bookings.AsNoTracking().FirstOrDefaultAsync(b => b.Id == bookingId && b.ClientId == clientId, cancellationToken);
        if (booking is null)
            throw new InvalidOperationException("Reserva no encontrada.");

        await EnsureLookupsAsync(cancellationToken);
        var cancelledBookingStatusId = await GetBookingStatusIdAsync("Cancelled", cancellationToken);
        if (booking.BookingStatusId == cancelledBookingStatusId)
            throw new InvalidOperationException("Esta reserva fue cancelada y ya no está disponible.");

        var bookingStatuses = await _db.BookingStatuses.AsNoTracking().ToListAsync(cancellationToken);
        var statusName = bookingStatuses.FirstOrDefault(s => s.Id == booking.BookingStatusId)?.Name ?? booking.BookingStatusId.ToString();

        var bookingFlights = await _db.BookingFlights.AsNoTracking().Where(bf => bf.BookingId == bookingId).ToListAsync(cancellationToken);
        var flights = await _db.Flights.AsNoTracking().Where(f => bookingFlights.Select(x => x.FlightId).Contains(f.Id)).ToListAsync(cancellationToken);
        var routes = await _db.Routes.AsNoTracking().ToListAsync(cancellationToken);
        var airports = await _db.Airports.AsNoTracking().ToListAsync(cancellationToken);

        var flightRows =
            from bf in bookingFlights
            join f in flights on bf.FlightId equals f.Id
            join r in routes on f.RouteId equals r.Id
            join ao in airports on r.OriginAirportId equals ao.Id
            join ad in airports on r.DestinationAirportId equals ad.Id
            select (f.FlightCode, $"{ao.IataCode} → {ad.IataCode}", f.DepartureAt, f.EstimatedArrivalAt, f.AvailableSeats);

        var flightReservationIds = await _db.FlightReservations.AsNoTracking()
            .Where(fr => bookingFlights.Select(bf => bf.Id).Contains(fr.BookingFlightId))
            .Select(fr => fr.Id)
            .ToListAsync(cancellationToken);

        var prs = await _db.PassengerReservations.AsNoTracking()
            .Where(pr => flightReservationIds.Contains(pr.Flight_Reservation_Id))
            .ToListAsync(cancellationToken);

        var passengerIds = prs.Select(p => p.Passenger_Id).Distinct().ToList();
        var passengers = await _db.Passengers.AsNoTracking().Where(p => passengerIds.Contains(p.Id)).ToListAsync(cancellationToken);
        var personIds = passengers.Select(p => p.PersonId).Distinct().ToList();
        var persons = await _db.Persons.AsNoTracking().Where(p => personIds.Contains(p.Id)).ToListAsync(cancellationToken);

        var passengerRows =
            from p in passengers
            join pe in persons on p.PersonId equals pe.Id
            select ($"{pe.FirstName} {pe.LastName}", pe.DocumentNumber);

        var payments = await _db.Payments.AsNoTracking().Where(p => p.BookingId == bookingId).ToListAsync(cancellationToken);
        var paymentStatuses = await _db.PaymentStatuses.AsNoTracking().ToListAsync(cancellationToken);
        var paymentStatus = payments.Count == 0
            ? "Sin pago"
            : paymentStatuses.FirstOrDefault(s => s.Id == payments[0].PaymentStatusId)?.Name ?? payments[0].PaymentStatusId.ToString();

        var ticketIds = await _db.Tickets.AsNoTracking()
            .Where(t => prs.Select(x => x.Id).Contains(t.PassengerReservation_Id))
            .Select(t => t.Id)
            .ToListAsync(cancellationToken);
        var hasAnyCheckin = ticketIds.Count != 0 && await _db.Checkins.AsNoTracking()
            .AnyAsync(c => ticketIds.Contains(c.TicketId), cancellationToken);

        var tickets = await BuildBookingTicketRowsAsync(bookingId, cancellationToken);

        return (booking.Id, statusName, booking.TotalAmount, flightRows.ToList(), passengerRows.Distinct().ToList(), paymentStatus, hasAnyCheckin, ticketIds, tickets);
    }

    private async Task<IReadOnlyList<BookingTicketRow>> BuildBookingTicketRowsAsync(int bookingId, CancellationToken cancellationToken)
    {
        var bfs = await _db.BookingFlights.AsNoTracking().Where(x => x.BookingId == bookingId).ToListAsync(cancellationToken);
        var flights = await _db.Flights.AsNoTracking().Where(f => bfs.Select(b => b.FlightId).Contains(f.Id)).ToListAsync(cancellationToken);
        var routes = await _db.Routes.AsNoTracking().ToListAsync(cancellationToken);
        var airports = await _db.Airports.AsNoTracking().ToListAsync(cancellationToken);
        var stopovers = await _db.RouteStopovers.AsNoTracking().ToListAsync(cancellationToken);

        var frs = await _db.FlightReservations.AsNoTracking().Where(fr => bfs.Select(b => b.Id).Contains(fr.BookingFlightId)).ToListAsync(cancellationToken);
        var prs = await _db.PassengerReservations.AsNoTracking().Where(pr => frs.Select(fr => fr.Id).Contains(pr.Flight_Reservation_Id)).ToListAsync(cancellationToken);
        var tickets = await _db.Tickets.AsNoTracking().Where(t => prs.Select(pr => pr.Id).Contains(t.PassengerReservation_Id)).ToListAsync(cancellationToken);

        var passengers = await _db.Passengers.AsNoTracking().Where(p => prs.Select(x => x.Passenger_Id).Contains(p.Id)).ToListAsync(cancellationToken);
        var persons = await _db.Persons.AsNoTracking().Where(p => passengers.Select(x => x.PersonId).Contains(p.Id)).ToListAsync(cancellationToken);

        var checkins = await _db.Checkins.AsNoTracking().Where(c => tickets.Select(t => t.Id).Contains(c.TicketId)).ToListAsync(cancellationToken);
        var baggage = await _db.Baggages.AsNoTracking().Where(b => checkins.Select(c => c.Id).Contains(b.CheckinId)).ToListAsync(cancellationToken);

        var checkinByTicketId = checkins.ToDictionary(c => c.TicketId);
        var flightSeatIdsForRows = checkins.Select(c => c.FlightSeatId).Distinct().ToList();
        var seatCodesBySeatId = flightSeatIdsForRows.Count == 0
            ? new Dictionary<int, string>()
            : await _db.FlightSeats.AsNoTracking()
                .Where(s => flightSeatIdsForRows.Contains(s.Id))
                .ToDictionaryAsync(s => s.Id, s => s.Code, cancellationToken);

        var mapFlightForPr =
            from pr in prs
            join fr in frs on pr.Flight_Reservation_Id equals fr.Id
            join bf in bfs on fr.BookingFlightId equals bf.Id
            select new { pr.Id, bf.FlightId, pr.Passenger_Id };

        var rows =
            from t in tickets
            join link in mapFlightForPr on t.PassengerReservation_Id equals link.Id
            join f in flights on link.FlightId equals f.Id
            join r in routes on f.RouteId equals r.Id
            join ao in airports on r.OriginAirportId equals ao.Id
            join ad in airports on r.DestinationAirportId equals ad.Id
            join pa in passengers on link.Passenger_Id equals pa.Id
            join pe in persons on pa.PersonId equals pe.Id
            select new
            {
                t.Id,
                t.Code,
                PassengerName = $"{pe.FirstName} {pe.LastName}",
                PassengerDoc = pe.DocumentNumber,
                f.FlightCode,
                RouteLabel = $"{ao.IataCode} → {ad.IataCode}",
                f.DepartureAt,
                ArrivalAt = f.EstimatedArrivalAt,
                StopoversCount = stopovers.Count(s => s.RouteId == r.Id),
                CheckinId = checkins.FirstOrDefault(c => c.TicketId == t.Id)?.Id
            };

        var result = rows
            .Select(x =>
            {
                string? seatCode = null;
                if (checkinByTicketId.TryGetValue(x.Id, out var ck))
                    seatCodesBySeatId.TryGetValue(ck.FlightSeatId, out seatCode);

                return new BookingTicketRow(
                    x.Id,
                    x.Code,
                    x.PassengerName,
                    x.PassengerDoc,
                    x.FlightCode,
                    x.RouteLabel,
                    x.DepartureAt,
                    x.ArrivalAt,
                    x.StopoversCount,
                    x.CheckinId is not null && baggage.Any(b => b.CheckinId == x.CheckinId.Value && b.ChargedPrice > 0),
                    seatCode);
            })
            .OrderBy(x => x.DepartureAt)
            .ThenBy(x => x.PassengerFullName)
            .ToList();

        return result;
    }

    public async Task CancelBookingAsync(int bookingId, int clientId, CancellationToken cancellationToken)
    {
        await EnsureLookupsAsync(cancellationToken);

        var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId && b.ClientId == clientId, cancellationToken);
        if (booking is null)
            throw new InvalidOperationException("Reserva no encontrada.");

        var pendingId = await GetBookingStatusIdAsync("Pending", cancellationToken);
        var confirmedId = await GetBookingStatusIdAsync("Confirmed", cancellationToken);
        if (booking.BookingStatusId != pendingId && booking.BookingStatusId != confirmedId)
            throw new InvalidOperationException("Solo se pueden cancelar reservas en estado Pending o Confirmed.");

        var cancelledId = await GetBookingStatusIdAsync("Cancelled", cancellationToken);
        booking.BookingStatusId = cancelledId;
        booking.UpdatedAt = DateTime.UtcNow;

        var bookingFlights = await _db.BookingFlights.AsNoTracking().Where(bf => bf.BookingId == bookingId).ToListAsync(cancellationToken);
        var frIds = await _db.FlightReservations.AsNoTracking()
            .Where(fr => bookingFlights.Select(bf => bf.Id).Contains(fr.BookingFlightId))
            .Select(fr => fr.Id)
            .ToListAsync(cancellationToken);
        var prIds = await _db.PassengerReservations.AsNoTracking()
            .Where(pr => frIds.Contains(pr.Flight_Reservation_Id))
            .Select(pr => pr.Id)
            .ToListAsync(cancellationToken);
        var ticketIds = await _db.Tickets.AsNoTracking()
            .Where(t => prIds.Contains(t.PassengerReservation_Id))
            .Select(t => t.Id)
            .ToListAsync(cancellationToken);
        var hasAnyCheckin = ticketIds.Count != 0 && await _db.Checkins.AsNoTracking()
            .AnyAsync(c => ticketIds.Contains(c.TicketId), cancellationToken);

        if (hasAnyCheckin)
            throw new InvalidOperationException("No puedes cancelar una reserva que ya tiene check-in registrado.");

        var voidedId = await GetTicketStateIdAsync("Voided", cancellationToken);
        var tickets = await _db.Tickets.Where(t => prIds.Contains(t.PassengerReservation_Id)).ToListAsync(cancellationToken);
        foreach (var t in tickets)
        {
            t.TicketState_Id = voidedId;
            t.UpdatedAt = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TicketSummary>> GetMyTicketsAsync(int clientId, CancellationToken cancellationToken)
    {
        await EnsureLookupsAsync(cancellationToken);
        var voidedTicketStateId = await GetTicketStateIdAsync("Voided", cancellationToken);

        var bookingIds = await _db.Bookings.AsNoTracking().Where(b => b.ClientId == clientId).Select(b => b.Id).ToListAsync(cancellationToken);
        var bookingFlights = await _db.BookingFlights.AsNoTracking().Where(bf => bookingIds.Contains(bf.BookingId)).ToListAsync(cancellationToken);
        var frs = await _db.FlightReservations.AsNoTracking().Where(fr => bookingFlights.Select(bf => bf.Id).Contains(fr.BookingFlightId)).ToListAsync(cancellationToken);
        var prs = await _db.PassengerReservations.AsNoTracking().Where(pr => frs.Select(fr => fr.Id).Contains(pr.Flight_Reservation_Id)).ToListAsync(cancellationToken);
        var tickets = await _db.Tickets.AsNoTracking()
            .Where(t => prs.Select(pr => pr.Id).Contains(t.PassengerReservation_Id) && t.TicketState_Id != voidedTicketStateId)
            .ToListAsync(cancellationToken);

        var ticketIds = tickets.Select(t => t.Id).ToList();
        var checkinsForTickets = await _db.Checkins.AsNoTracking()
            .Where(c => ticketIds.Contains(c.TicketId))
            .ToListAsync(cancellationToken);
        var seatIds = checkinsForTickets.Select(c => c.FlightSeatId).Distinct().ToList();
        var seatCodesById = seatIds.Count == 0
            ? new Dictionary<int, string>()
            : await _db.FlightSeats.AsNoTracking()
                .Where(s => seatIds.Contains(s.Id))
                .ToDictionaryAsync(s => s.Id, s => s.Code, cancellationToken);
        var checkinByTicketId = checkinsForTickets.ToDictionary(c => c.TicketId);

        var passengers = await _db.Passengers.AsNoTracking().Where(p => prs.Select(pr => pr.Passenger_Id).Contains(p.Id)).ToListAsync(cancellationToken);
        var persons = await _db.Persons.AsNoTracking().Where(p => passengers.Select(x => x.PersonId).Contains(p.Id)).ToListAsync(cancellationToken);

        var flights = await _db.Flights.AsNoTracking().Where(f => bookingFlights.Select(bf => bf.FlightId).Contains(f.Id)).ToListAsync(cancellationToken);
        var routes = await _db.Routes.AsNoTracking().ToListAsync(cancellationToken);
        var airports = await _db.Airports.AsNoTracking().ToListAsync(cancellationToken);
        var ticketStates = await _db.TicketStates.AsNoTracking().ToListAsync(cancellationToken);

        var mapFlightForPr =
            from pr in prs
            join fr in frs on pr.Flight_Reservation_Id equals fr.Id
            join bf in bookingFlights on fr.BookingFlightId equals bf.Id
            select new { pr.Id, bf.FlightId, pr.Passenger_Id };

        var result =
            from t in tickets
            join link in mapFlightForPr on t.PassengerReservation_Id equals link.Id
            join f in flights on link.FlightId equals f.Id
            join r in routes on f.RouteId equals r.Id
            join ao in airports on r.OriginAirportId equals ao.Id
            join ad in airports on r.DestinationAirportId equals ad.Id
            join p in passengers on link.Passenger_Id equals p.Id
            join pe in persons on p.PersonId equals pe.Id
            select new TicketSummary(
                t.Id,
                t.Code,
                t.PassengerReservation_Id,
                f.Id,
                f.FlightCode,
                ao.IataCode,
                ad.IataCode,
                f.DepartureAt,
                f.EstimatedArrivalAt,
                $"{pe.FirstName} {pe.LastName}",
                checkinByTicketId.ContainsKey(t.Id)
                    ? "Check-in listo"
                    : ticketStates.FirstOrDefault(s => s.Id == t.TicketState_Id)?.Name ?? t.TicketState_Id.ToString(),
                checkinByTicketId.TryGetValue(t.Id, out var ck)
                    ? seatCodesById.GetValueOrDefault(ck.FlightSeatId)
                    : null
            );

        return result
            .OrderByDescending(x => x.DepartureAt)
            .ToList();
    }

    public async Task<(TicketSummary Ticket, string? BoardingPassNumber, string? SeatCode, string? CabinTypeName, bool IsCheckedIn)> GetTicketDetailsAsync(
        int ticketId,
        int clientId,
        CancellationToken cancellationToken)
    {
        var tickets = await GetMyTicketsAsync(clientId, cancellationToken);
        var ticket = tickets.FirstOrDefault(t => t.TicketId == ticketId);
        if (ticket is null)
            throw new InvalidOperationException("Tiquete no encontrado.");

        var checkin = await _db.Checkins.AsNoTracking().FirstOrDefaultAsync(c => c.TicketId == ticketId, cancellationToken);
        if (checkin is null)
            return (ticket, null, null, null, false);

        var seat = await _db.FlightSeats.AsNoTracking().FirstOrDefaultAsync(s => s.Id == checkin.FlightSeatId, cancellationToken);
        var cabin = seat is null
            ? null
            : await _db.CabinTypes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == seat.CabinTypeId, cancellationToken);

        return (ticket, checkin.BoardingPassNumber, seat?.Code, cabin?.Name, true);
    }

    public async Task<IReadOnlyList<(int BookingId, IReadOnlyList<(int FlightId, string FlightCode, string RouteLabel, DateTime DepartureAt, DateTime ArrivalAt)> Flights)>> FindBookingsForCheckinAsync(
        int clientId,
        string? bookingLookup,
        string? passengerLastName,
        DateTime utcNow,
        CancellationToken cancellationToken)
    {
        await EnsureLookupsAsync(cancellationToken);
        var cancelledBookingStatusId = await GetBookingStatusIdAsync("Cancelled", cancellationToken);

        List<BookingEntity> bookings;

        if (!string.IsNullOrWhiteSpace(bookingLookup))
        {
            int? targetBookingId = null;

            if (ReservationReferenceCodec.TryParseReservationCode(bookingLookup, out var parsedFromCode))
                targetBookingId = parsedFromCode;

            if (targetBookingId is null)
            {
                var candidateIds = await _db.Bookings.AsNoTracking()
                    .Where(b => b.ClientId == clientId && b.BookingStatusId != cancelledBookingStatusId)
                    .Select(b => b.Id)
                    .ToListAsync(cancellationToken);

                foreach (var bid in candidateIds)
                {
                    if (ReservationReferenceCodec.MatchesBookingCode(bookingLookup, bid))
                    {
                        targetBookingId = bid;
                        break;
                    }
                }
            }

            if (targetBookingId is null)
                return Array.Empty<(int BookingId, IReadOnlyList<(int FlightId, string FlightCode, string RouteLabel, DateTime DepartureAt, DateTime ArrivalAt)> Flights)>();

            bookings = await _db.Bookings.AsNoTracking()
                .Where(b =>
                    b.Id == targetBookingId.Value &&
                    b.ClientId == clientId &&
                    b.BookingStatusId != cancelledBookingStatusId)
                .ToListAsync(cancellationToken);

            if (bookings.Count == 0)
                return Array.Empty<(int BookingId, IReadOnlyList<(int FlightId, string FlightCode, string RouteLabel, DateTime DepartureAt, DateTime ArrivalAt)> Flights)>();
        }
        else
        {
            bookings = await _db.Bookings.AsNoTracking()
                .Where(b => b.ClientId == clientId && b.BookingStatusId != cancelledBookingStatusId)
                .ToListAsync(cancellationToken);
        }

        var bookingIds = bookings.Select(b => b.Id).ToList();
        var bookingFlights = await _db.BookingFlights.AsNoTracking().Where(bf => bookingIds.Contains(bf.BookingId)).ToListAsync(cancellationToken);
        var flights = await _db.Flights.AsNoTracking().Where(f => bookingFlights.Select(bf => bf.FlightId).Contains(f.Id)).ToListAsync(cancellationToken);
        var routes = await _db.Routes.AsNoTracking().ToListAsync(cancellationToken);
        var airports = await _db.Airports.AsNoTracking().ToListAsync(cancellationToken);

        var frs = await _db.FlightReservations.AsNoTracking().Where(fr => bookingFlights.Select(bf => bf.Id).Contains(fr.BookingFlightId)).ToListAsync(cancellationToken);
        var prs = await _db.PassengerReservations.AsNoTracking().Where(pr => frs.Select(fr => fr.Id).Contains(pr.Flight_Reservation_Id)).ToListAsync(cancellationToken);
        var passengers = await _db.Passengers.AsNoTracking().Where(p => prs.Select(pr => pr.Passenger_Id).Contains(p.Id)).ToListAsync(cancellationToken);
        var persons = await _db.Persons.AsNoTracking().Where(p => passengers.Select(x => x.PersonId).Contains(p.Id)).ToListAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(passengerLastName))
        {
            var ln = passengerLastName.Trim();
            var passengerIdsForLn =
                (from pa in passengers
                    join pe in persons on pa.PersonId equals pe.Id
                    where pe.LastName.Contains(ln, StringComparison.OrdinalIgnoreCase)
                    select pa.Id).ToHashSet();

            var prIdsForLn = prs.Where(pr => passengerIdsForLn.Contains(pr.Passenger_Id)).Select(pr => pr.Flight_Reservation_Id).ToHashSet();
            var bfIdsForLn = frs.Where(fr => prIdsForLn.Contains(fr.Id)).Select(fr => fr.BookingFlightId).ToHashSet();
            var bookingIdsForLn = bookingFlights.Where(bf => bfIdsForLn.Contains(bf.Id)).Select(bf => bf.BookingId).Distinct().ToHashSet();
            bookings = bookings.Where(b => bookingIdsForLn.Contains(b.Id)).ToList();
        }

        var flightRows =
            (from bf in bookingFlights
                join f in flights on bf.FlightId equals f.Id
                join r in routes on f.RouteId equals r.Id
                join ao in airports on r.OriginAirportId equals ao.Id
                join ad in airports on r.DestinationAirportId equals ad.Id
                select new { bf.BookingId, FlightId = f.Id, f.FlightCode, RouteLabel = $"{ao.IataCode} → {ad.IataCode}", f.DepartureAt, ArrivalAt = f.EstimatedArrivalAt })
            .ToList();

        // Solo vuelos que aún no han salido (evita elegir la pata de ida ya pasada en ida y vuelta).
        var eligibleFlightRows = flightRows.Where(x => x.DepartureAt > utcNow).ToList();

        var result = bookings
            .Where(b => eligibleFlightRows.Exists(x => x.BookingId == b.Id))
            .Select(b => (
                b.Id,
                (IReadOnlyList<(int FlightId, string FlightCode, string RouteLabel, DateTime DepartureAt, DateTime ArrivalAt)>)eligibleFlightRows
                    .Where(x => x.BookingId == b.Id)
                    .OrderBy(x => x.DepartureAt)
                    .Select(x => (x.FlightId, x.FlightCode, x.RouteLabel, x.DepartureAt, x.ArrivalAt))
                    .ToList()
            ))
            .ToList();

        return result;
    }

    public async Task<CheckinCandidatesResult> GetCheckinCandidatesAsync(
        int clientId,
        int bookingId,
        int flightId,
        DateTime utcNow,
        CancellationToken cancellationToken)
    {
        var booking = await _db.Bookings.AsNoTracking().FirstOrDefaultAsync(b => b.Id == bookingId && b.ClientId == clientId, cancellationToken);
        if (booking is null)
            throw new InvalidOperationException("Reserva no encontrada.");

        await EnsureLookupsAsync(cancellationToken);
        var cancelledBookingStatusId = await GetBookingStatusIdAsync("Cancelled", cancellationToken);
        if (booking.BookingStatusId == cancelledBookingStatusId)
            throw new InvalidOperationException("Esta reserva está cancelada; no se puede hacer check-in.");

        var bf = await _db.BookingFlights.AsNoTracking().FirstOrDefaultAsync(x => x.BookingId == bookingId && x.FlightId == flightId, cancellationToken);
        if (bf is null)
            throw new InvalidOperationException("Vuelo no pertenece a la reserva.");

        var flight = await _db.Flights.AsNoTracking().FirstOrDefaultAsync(f => f.Id == flightId, cancellationToken);
        if (flight is null)
            throw new InvalidOperationException("Vuelo no encontrado.");

        if (flight.DepartureAt <= utcNow)
            throw new InvalidOperationException("El vuelo ya salió.");

        if (flight.DepartureAt - utcNow > TimeSpan.FromHours(24))
            throw new InvalidOperationException("El check-in solo está disponible dentro de las 24 horas previas al vuelo.");

        var route = await _db.Routes.AsNoTracking().FirstOrDefaultAsync(r => r.Id == flight.RouteId, cancellationToken);
        if (route is null) throw new InvalidOperationException("Ruta no encontrada.");
        var ao = await _db.Airports.AsNoTracking().FirstOrDefaultAsync(a => a.Id == route.OriginAirportId, cancellationToken);
        var ad = await _db.Airports.AsNoTracking().FirstOrDefaultAsync(a => a.Id == route.DestinationAirportId, cancellationToken);
        if (ao is null || ad is null) throw new InvalidOperationException("Aeropuertos no encontrados.");

        var fr = await _db.FlightReservations.AsNoTracking().FirstOrDefaultAsync(x => x.BookingFlightId == bf.Id, cancellationToken);
        if (fr is null) throw new InvalidOperationException("No se encontró la reserva de vuelo.");

        var prs = await _db.PassengerReservations.AsNoTracking().Where(pr => pr.Flight_Reservation_Id == fr.Id).ToListAsync(cancellationToken);
        var tickets = await _db.Tickets.AsNoTracking().Where(t => prs.Select(x => x.Id).Contains(t.PassengerReservation_Id)).ToListAsync(cancellationToken);
        var checkins = await _db.Checkins.AsNoTracking().Where(c => tickets.Select(t => t.Id).Contains(c.TicketId)).ToListAsync(cancellationToken);
        var passengers = await _db.Passengers.AsNoTracking().Where(p => prs.Select(x => x.Passenger_Id).Contains(p.Id)).ToListAsync(cancellationToken);
        var persons = await _db.Persons.AsNoTracking().Where(p => passengers.Select(x => x.PersonId).Contains(p.Id)).ToListAsync(cancellationToken);
        var ticketStates = await _db.TicketStates.AsNoTracking().ToListAsync(cancellationToken);
        var seatCodesById = checkins.Count == 0
            ? new Dictionary<int, string>()
            : await _db.FlightSeats.AsNoTracking()
                .Where(s => checkins.Select(c => c.FlightSeatId).Contains(s.Id))
                .ToDictionaryAsync(s => s.Id, s => s.Code, cancellationToken);
        var checkinByTicketId = checkins.ToDictionary(c => c.TicketId);

        var candidateRows =
            from t in tickets
            join pr in prs on t.PassengerReservation_Id equals pr.Id
            join pa in passengers on pr.Passenger_Id equals pa.Id
            join pe in persons on pa.PersonId equals pe.Id
            let foundCheckin = checkinByTicketId.GetValueOrDefault(t.Id)
            select new CheckinPassengerRow(
                pr.Id,
                pa.Id,
                $"{pe.FirstName} {pe.LastName}",
                pe.DocumentNumber,
                t.Code,
                ticketStates.FirstOrDefault(s => s.Id == t.TicketState_Id)?.Name ?? t.TicketState_Id.ToString(),
                foundCheckin is null ? null : seatCodesById.GetValueOrDefault(foundCheckin.FlightSeatId),
                foundCheckin?.BoardingPassNumber
            );

        var orderedRows = candidateRows
            .OrderBy(x => x.PassengerName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(x => x.PassengerDocument, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return new CheckinCandidatesResult(
            bookingId,
            flightId,
            flight.FlightCode,
            ao.IataCode,
            ad.IataCode,
            flight.DepartureAt,
            flight.EstimatedArrivalAt,
            orderedRows.Where(x => x.BoardingPassNumber is null).ToList(),
            orderedRows.Where(x => x.BoardingPassNumber is not null).ToList());
    }

    public async Task<IReadOnlyList<string>> GetAvailableSeatCodesAsync(
        int flightId,
        CancellationToken cancellationToken)
    {
        await EnsurePhysicalSeatRowsExistForFlightAsync(flightId, cancellationToken);

        return await _db.FlightSeats.AsNoTracking()
            .Where(s => s.FlightId == flightId && !s.IsOccupied)
            .OrderBy(s => s.Code.Length)
            .ThenBy(s => s.Code)
            .Select(s => s.Code)
            .ToListAsync(cancellationToken);
    }

    public async Task<(string BoardingPassNumber, string SeatCode, string CabinTypeName, string PassengerFullName, string FlightCode,
        string OriginIata, string DestinationIata, DateTime DepartureAt, DateTime ArrivalAt,
        decimal AdditionalSeatChoiceCharge)> PerformOnlineCheckinAsync(
        int clientId,
        int passengerReservationId,
        int flightId,
        string? desiredSeatCode,
        bool allowRandomSeat,
        DateTime utcNow,
        CancellationToken cancellationToken)
    {
        await EnsureLookupsAsync(cancellationToken);

        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.PassengerReservation_Id == passengerReservationId, cancellationToken);
        if (ticket is null)
            throw new InvalidOperationException("No se encontró el tiquete del pasajero seleccionado.");

        var ticketId = ticket.Id;

        var already = await _db.Checkins.AsNoTracking().FirstOrDefaultAsync(c => c.TicketId == ticketId, cancellationToken);
        if (already is not null)
            throw new InvalidOperationException("Este pasajero ya tiene check-in para ese vuelo.");

        var candidates = await GetCheckinCandidatesAsync(clientId, bookingId: await ResolveBookingIdForTicketAsync(clientId, ticketId, cancellationToken), flightId, utcNow, cancellationToken);
        var isCandidate = candidates.PendingPassengers.Any(t => t.PassengerReservationId == passengerReservationId);
        if (!isCandidate)
            throw new InvalidOperationException("Ese pasajero no pertenece a esa reserva o vuelo.");

        await EnsurePhysicalSeatRowsExistForFlightAsync(flightId, cancellationToken);

        var seats = await _db.FlightSeats.Where(s => s.FlightId == flightId).ToListAsync(cancellationToken);
        var free = seats.Where(s => !s.IsOccupied).ToList();
        if (free.Count == 0)
            throw new InvalidOperationException("No hay asientos disponibles.");

        FlightSeatEntity chosen;
        if (!string.IsNullOrWhiteSpace(desiredSeatCode))
        {
            var code = desiredSeatCode.Trim().ToUpperInvariant();
            chosen = free.FirstOrDefault(s => string.Equals(s.Code, code, StringComparison.OrdinalIgnoreCase))
                     ?? throw new InvalidOperationException("El asiento solicitado no está disponible.");
        }
        else if (allowRandomSeat)
        {
            chosen = free[Random.Shared.Next(0, free.Count)];
        }
        else
        {
            chosen = free[0];
        }

        chosen.IsOccupied = true;

        var usedTicketStateId = await GetTicketStateIdAsync("Used", cancellationToken);
        ticket.TicketState_Id = usedTicketStateId;
        ticket.UpdatedAt = DateTime.UtcNow;

        var checkinCompletedId = await GetCheckinStateIdAsync("Completed", cancellationToken);
        var bp = $"BP-{Guid.NewGuid().ToString()[..6].ToUpper()}";

        var checkin = new CheckinEntity
        {
            TicketId = ticketId,
            StaffId = await ResolveAutomaticStaffIdAsync(cancellationToken),
            FlightSeatId = chosen.Id,
            CheckedInAt = utcNow,
            CheckinStatusId = checkinCompletedId,
            BoardingPassNumber = bp
        };
        _db.Checkins.Add(checkin);

        var additionalSeatChoiceCharge = 0m;
        if (!string.IsNullOrWhiteSpace(desiredSeatCode))
        {
            additionalSeatChoiceCharge = CheckInSpecificSeatChoiceFee;
            var bookingIdForFee = await ResolveBookingIdForTicketAsync(clientId, ticketId, cancellationToken);
            var bookingForFee = await _db.Bookings.FirstOrDefaultAsync(
                b => b.Id == bookingIdForFee && b.ClientId == clientId,
                cancellationToken);
            if (bookingForFee is null)
                throw new InvalidOperationException("No se pudo registrar el cargo adicional de asiento.");

            bookingForFee.TotalAmount += additionalSeatChoiceCharge;
            bookingForFee.UpdatedAt = utcNow;

            var paidStatusId = await GetPaymentStatusIdAsync("Paid", cancellationToken);
            var existingPaymentMethodId = await _db.Payments.AsNoTracking()
                .Where(p => p.BookingId == bookingForFee.Id)
                .OrderBy(p => p.Id)
                .Select(p => p.PaymentMethodId)
                .FirstOrDefaultAsync(cancellationToken);
            var paymentMethodIdForFee = existingPaymentMethodId != 0
                ? existingPaymentMethodId
                : await EnsurePaymentMethodAsync(SimulatedPaymentMethod.Nequi, cancellationToken);

            _db.Payments.Add(new PaymentsEntity
            {
                BookingId = bookingForFee.Id,
                Amount = additionalSeatChoiceCharge,
                PaidAt = utcNow,
                PaymentStatusId = paidStatusId,
                PaymentMethodId = paymentMethodIdForFee,
                CreatedAt = utcNow,
                UpdatedAt = utcNow
            });
        }

        await _db.SaveChangesAsync(cancellationToken);

        var flight = await _db.Flights.AsNoTracking().FirstAsync(f => f.Id == flightId, cancellationToken);
        var route = await _db.Routes.AsNoTracking().FirstAsync(r => r.Id == flight.RouteId, cancellationToken);
        var ao = await _db.Airports.AsNoTracking().FirstAsync(a => a.Id == route.OriginAirportId, cancellationToken);
        var ad = await _db.Airports.AsNoTracking().FirstAsync(a => a.Id == route.DestinationAirportId, cancellationToken);
        var cabin = await _db.CabinTypes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == chosen.CabinTypeId, cancellationToken);

        var passengerName = await ResolvePassengerNameForTicketAsync(ticketId, cancellationToken);

        return (bp, chosen.Code, cabin?.Name ?? chosen.CabinTypeId.ToString(), passengerName, flight.FlightCode, ao.IataCode, ad.IataCode, flight.DepartureAt, flight.EstimatedArrivalAt, additionalSeatChoiceCharge);
    }

    private async Task DeductSoldSeatsFromFlightsAsync(
        int outboundFlightId,
        int? returnFlightId,
        int passengerCount,
        DateTime utcNow,
        CancellationToken cancellationToken)
    {
        var outbound = await _db.Flights.FirstAsync(f => f.Id == outboundFlightId, cancellationToken);
        if (outbound.AvailableSeats < passengerCount)
            throw new InvalidOperationException(
                $"No hay suficientes cupos en el vuelo de ida (disponibles: {outbound.AvailableSeats}, solicitados: {passengerCount}).");

        outbound.AvailableSeats -= passengerCount;
        outbound.UpdatedAt = utcNow;

        if (returnFlightId is int rid)
        {
            var inbound = await _db.Flights.FirstAsync(f => f.Id == rid, cancellationToken);
            if (inbound.AvailableSeats < passengerCount)
                throw new InvalidOperationException(
                    $"No hay suficientes cupos en el vuelo de regreso (disponibles: {inbound.AvailableSeats}, solicitados: {passengerCount}).");

            inbound.AvailableSeats -= passengerCount;
            inbound.UpdatedAt = utcNow;
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureLookupsAsync(CancellationToken cancellationToken)
    {
        await EnsureBookingStatusAsync("Pending", cancellationToken);
        await EnsureBookingStatusAsync("Confirmed", cancellationToken);
        await EnsureBookingStatusAsync("Cancelled", cancellationToken);

        await EnsurePaymentStatusAsync("Paid", cancellationToken);

        await EnsureTicketStateAsync("Issued", cancellationToken);
        await EnsureTicketStateAsync("Used", cancellationToken);
        await EnsureTicketStateAsync("Voided", cancellationToken);

        await EnsureCheckinStateAsync("Completed", cancellationToken);

        // PassengerTypes en este proyecto se siembran como Adult/Senior/Child/Infant.
        await EnsurePassengerTypeAsync("Adult", minAge: 12, maxAge: null, cancellationToken);
        await EnsurePassengerTypeAsync("Senior", minAge: 60, maxAge: null, cancellationToken);
        await EnsurePassengerTypeAsync("Child", minAge: 0, maxAge: 17, cancellationToken);

        await EnsureSystemRoleAsync("Client", cancellationToken);
        await EnsureSystemRoleAsync("User", cancellationToken);
    }

    private async Task EnsureSystemRoleAsync(string name, CancellationToken cancellationToken)
    {
        var exists = await _db.SystemRoles.AsNoTracking().AnyAsync(r => r.Name == name, cancellationToken);
        if (exists) return;
        _db.SystemRoles.Add(new SystemRolesEntity { Name = name, Description = $"Rol '{name}'" });
        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureBookingStatusAsync(string name, CancellationToken cancellationToken)
    {
        var exists = await _db.BookingStatuses.AsNoTracking().AnyAsync(s => s.Name == name, cancellationToken);
        if (exists) return;
        _db.BookingStatuses.Add(new BookingStatusesEntity { Name = name });
        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task<int> GetBookingStatusIdAsync(string name, CancellationToken cancellationToken)
        => (await _db.BookingStatuses.AsNoTracking().FirstAsync(s => s.Name == name, cancellationToken)).Id;

    private async Task EnsurePaymentStatusAsync(string name, CancellationToken cancellationToken)
    {
        var exists = await _db.PaymentStatuses.AsNoTracking().AnyAsync(s => s.Name == name, cancellationToken);
        if (exists) return;
        _db.PaymentStatuses.Add(new PaymentStatusesEntity { Name = name });
        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task<int> GetPaymentStatusIdAsync(string name, CancellationToken cancellationToken)
        => (await _db.PaymentStatuses.AsNoTracking().FirstAsync(s => s.Name == name, cancellationToken)).Id;

    private async Task EnsureTicketStateAsync(string name, CancellationToken cancellationToken)
    {
        var exists = await _db.TicketStates.AsNoTracking().AnyAsync(s => s.Name == name, cancellationToken);
        if (exists) return;
        _db.TicketStates.Add(new TicketStatesEntity { Name = name });
        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task<int> GetTicketStateIdAsync(string name, CancellationToken cancellationToken)
        => (await _db.TicketStates.AsNoTracking().FirstAsync(s => s.Name == name, cancellationToken)).Id;

    private async Task EnsureCheckinStateAsync(string name, CancellationToken cancellationToken)
    {
        var exists = await _db.CheckinStates.AsNoTracking().AnyAsync(s => s.Name == name, cancellationToken);
        if (exists) return;
        _db.CheckinStates.Add(new CheckinStatesEntity { Name = name });
        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task<int> GetCheckinStateIdAsync(string name, CancellationToken cancellationToken)
        => (await _db.CheckinStates.AsNoTracking().FirstAsync(s => s.Name == name, cancellationToken)).Id;

    private async Task EnsurePassengerTypeAsync(string name, int? minAge, int? maxAge, CancellationToken cancellationToken)
    {
        var exists = await _db.PassengerTypes.AsNoTracking().AnyAsync(p => p.Name == name, cancellationToken);
        if (exists) return;
        _db.PassengerTypes.Add(new PassengerTypeEntity { Name = name, MinAge = minAge, MaxAge = maxAge });
        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task<int> GetPassengerTypeIdAsync(string name, CancellationToken cancellationToken)
        => (await _db.PassengerTypes.AsNoTracking().FirstAsync(p => p.Name == name, cancellationToken)).Id;

    private static decimal ResolveBasePrice(List<RateEntity> rates, int routeId, int cabinTypeId, int passengerTypeId, DateTime departureAt)
    {
        var d = DateOnly.FromDateTime(departureAt);
        var match = rates.FirstOrDefault(r =>
            r.RouteId == routeId &&
            r.CabinTypeId == cabinTypeId &&
            r.PassengerTypeId == passengerTypeId &&
            (r.ValidFrom is null || r.ValidFrom.Value <= d) &&
            (r.ValidUntil is null || r.ValidUntil.Value >= d));

        return match?.BasePrice ?? 0m;
    }

    private async Task<int> EnsurePaymentMethodAsync(SimulatedPaymentMethod method, CancellationToken cancellationToken)
    {
        var (typeName, displayName) = method switch
        {
            SimulatedPaymentMethod.CreditCard => ("Card", "Tarjeta crédito"),
            SimulatedPaymentMethod.DebitCard => ("Card", "Tarjeta débito"),
            SimulatedPaymentMethod.Pse => ("Transfer", "PSE"),
            SimulatedPaymentMethod.Nequi => ("Wallet", "Nequi"),
            _ => ("Other", method.ToString())
        };

        var type = await _db.PaymentMediumTypes.FirstOrDefaultAsync(t => t.Name == typeName, cancellationToken);
        if (type is null)
        {
            type = new PaymentMediumTypesEntity { Name = typeName };
            _db.PaymentMediumTypes.Add(type);
            await _db.SaveChangesAsync(cancellationToken);
        }

        var pm = await _db.PaymentMethods.FirstOrDefaultAsync(p => p.DisplayName == displayName, cancellationToken);
        if (pm is null)
        {
            pm = new PaymentMethodsEntity
            {
                PaymentMethodTypeId = type.Id,
                CardTypeId = null,
                CardIssuerId = null,
                DisplayName = displayName
            };
            _db.PaymentMethods.Add(pm);
            await _db.SaveChangesAsync(cancellationToken);
        }

        return pm.Id;
    }

    private async Task<int> GetAnyDocumentTypeIdAsync(CancellationToken cancellationToken)
    {
        var dt = await _db.DocumentTypes.AsNoTracking().OrderBy(x => x.Id).FirstOrDefaultAsync(cancellationToken);
        if (dt is not null) return dt.Id;

        // fallback ultra simple: crear uno
        var created = new GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Infrastructure.Persistence.Entities.DocumentTypeEntity
        {
            Name = "N/A",
            Code = "NA"
        };
        _db.DocumentTypes.Add(created);
        await _db.SaveChangesAsync(cancellationToken);
        return created.Id;
    }

    private async Task<PersonEntity> GetOrCreatePersonAsync(
        int documentTypeId,
        string documentNumber,
        string firstName,
        string lastName,
        CancellationToken cancellationToken)
    {
        var doc = documentNumber.Trim();
        var existing = await _db.Persons.FirstOrDefaultAsync(
            x => x.DocumentTypeId == documentTypeId && x.DocumentNumber == doc,
            cancellationToken);

        if (existing is not null)
            return existing;

        var now = DateTime.UtcNow;
        var created = new PersonEntity
        {
            DocumentTypeId = documentTypeId,
            DocumentNumber = doc,
            FirstName = firstName,
            LastName = lastName,
            BirthDate = null,
            Gender = null,
            AddressId = null,
            CreatedAt = now,
            UpdatedAt = now
        };
        _db.Persons.Add(created);
        await _db.SaveChangesAsync(cancellationToken);
        return created;
    }

    private async Task<PassengersEntity> GetOrCreatePassengerAsync(
        int personId,
        int passengerTypeId,
        CancellationToken cancellationToken)
    {
        var existing = await _db.Passengers.FirstOrDefaultAsync(p => p.PersonId == personId, cancellationToken);
        if (existing is not null)
        {
            // Si ya existe, solo alineamos tipo si cambia (Adult/Senior/Child)
            if (existing.PassengerTypeId != passengerTypeId)
            {
                existing.PassengerTypeId = passengerTypeId;
                await _db.SaveChangesAsync(cancellationToken);
            }
            return existing;
        }

        var created = new PassengersEntity
        {
            PersonId = personId,
            PassengerTypeId = passengerTypeId
        };
        _db.Passengers.Add(created);
        await _db.SaveChangesAsync(cancellationToken);
        return created;
    }

    private async Task UpsertContactAsync(int personId, ContactInput contact, CancellationToken cancellationToken)
    {
        // EmailDomains y PhoneCodes existen como catálogos, pero el portal solo necesita persistir algo útil.
        // Guardamos email como PersonEmail si existe dominio; si no, no fallamos.
        try
        {
            var email = contact.Email.Trim();
            var at = email.LastIndexOf('@');
            if (at > 0 && at < email.Length - 1)
            {
                var user = email[..at];
                var domain = email[(at + 1)..].ToLowerInvariant();

                var domains = await _db.EmailDomains.AsNoTracking().ToListAsync(cancellationToken);
                var domainEntity = domains.FirstOrDefault(d => d.Domain.Equals(domain, StringComparison.OrdinalIgnoreCase));
                if (domainEntity is null)
                {
                    domainEntity = new EmailDomainsEntity { Domain = domain };
                    _db.EmailDomains.Add(domainEntity);
                    await _db.SaveChangesAsync(cancellationToken);
                }

                var existing = await _db.PersonEmails.FirstOrDefaultAsync(e => e.PersonId == personId && e.IsPrimary, cancellationToken);
                if (existing is null)
                {
                    _db.PersonEmails.Add(new src.Moduls.PeopleEmails.Infrastructure.Entity.PersonEmailEntity
                    {
                        PersonId = personId,
                        EmailUser = user,
                        EmailDomainId = domainEntity.Id,
                        IsPrimary = true
                    });
                    await _db.SaveChangesAsync(cancellationToken);
                }
            }
        }
        catch
        {
            // no-op: no queremos bloquear la compra por contacto.
        }
    }

    private async Task<IReadOnlyList<TicketSummary>> BuildTicketSummariesAsync(
        List<int> ticketIds,
        List<TicketStatesEntity> ticketStates,
        List<FlightEntity> flights,
        List<RouteEntity> routes,
        List<AirportEntity> airports,
        List<CityEntity> cities,
        string? cabinTypeName,
        CancellationToken cancellationToken)
    {
        var tickets = await _db.Tickets.AsNoTracking().Where(t => ticketIds.Contains(t.Id)).ToListAsync(cancellationToken);
        var prs = await _db.PassengerReservations.AsNoTracking().Where(pr => tickets.Select(t => t.PassengerReservation_Id).Contains(pr.Id)).ToListAsync(cancellationToken);
        var passengers = await _db.Passengers.AsNoTracking().Where(p => prs.Select(pr => pr.Passenger_Id).Contains(p.Id)).ToListAsync(cancellationToken);
        var persons = await _db.Persons.AsNoTracking().Where(p => passengers.Select(x => x.PersonId).Contains(p.Id)).ToListAsync(cancellationToken);
        var frs = await _db.FlightReservations.AsNoTracking().Where(fr => prs.Select(pr => pr.Flight_Reservation_Id).Contains(fr.Id)).ToListAsync(cancellationToken);
        var bfs = await _db.BookingFlights.AsNoTracking().Where(bf => frs.Select(fr => fr.BookingFlightId).Contains(bf.Id)).ToListAsync(cancellationToken);

        var mapFlightForPr =
            from pr in prs
            join fr in frs on pr.Flight_Reservation_Id equals fr.Id
            join bf in bfs on fr.BookingFlightId equals bf.Id
            select new { pr.Id, bf.FlightId, pr.Passenger_Id };

        var result =
            from t in tickets
            join link in mapFlightForPr on t.PassengerReservation_Id equals link.Id
            join f in flights on link.FlightId equals f.Id
            join r in routes on f.RouteId equals r.Id
            join ao in airports on r.OriginAirportId equals ao.Id
            join ad in airports on r.DestinationAirportId equals ad.Id
            join pa in passengers on link.Passenger_Id equals pa.Id
            join pe in persons on pa.PersonId equals pe.Id
            select new TicketSummary(
                t.Id,
                t.Code,
                t.PassengerReservation_Id,
                f.Id,
                f.FlightCode,
                ao.IataCode,
                ad.IataCode,
                f.DepartureAt,
                f.EstimatedArrivalAt,
                $"{pe.FirstName} {pe.LastName}",
                ticketStates.FirstOrDefault(s => s.Id == t.TicketState_Id)?.Name ?? t.TicketState_Id.ToString(),
                null
            );

        return result.ToList();
    }

    /// <summary>
    /// Genera filas en <c>flight_seats</c> hasta alcanzar la capacidad del vuelo cuando aún no existen (p. ej. solo se sembraron 2 vuelos en demo).
    /// Así el check-in puede asignar códigos únicos sin depender de datos manuales por cada vuelo.
    /// </summary>
    private async Task EnsurePhysicalSeatRowsExistForFlightAsync(int flightId, CancellationToken cancellationToken)
    {
        var flight = await _db.Flights.AsNoTracking().FirstOrDefaultAsync(f => f.Id == flightId, cancellationToken)
            ?? throw new InvalidOperationException("Vuelo no encontrado.");

        var target = Math.Max(flight.TotalCapacity, 1);

        var currentCount = await _db.FlightSeats.CountAsync(s => s.FlightId == flightId, cancellationToken);
        if (currentCount >= target)
            return;

        var cabinTypeId = await _db.CabinTypes.OrderBy(c => c.Id).Select(c => c.Id).FirstAsync(cancellationToken);
        var seatLocationTypeId = await _db.SeatLocationTypes.OrderBy(s => s.Id).Select(s => s.Id).FirstAsync(cancellationToken);

        var existingCodes = await _db.FlightSeats
            .AsNoTracking()
            .Where(s => s.FlightId == flightId)
            .Select(s => s.Code)
            .ToListAsync(cancellationToken);
        var reservedCodes = existingCodes.ToHashSet(StringComparer.OrdinalIgnoreCase);

        var missing = target - currentCount;
        var batch = new List<FlightSeatEntity>(missing);
        foreach (var code in EnumerateDistinctSeatCodes(missing, reservedCodes))
        {
            batch.Add(new FlightSeatEntity
            {
                FlightId = flightId,
                CabinTypeId = cabinTypeId,
                SeatLocationTypeId = seatLocationTypeId,
                Code = code,
                IsOccupied = false
            });
        }

        if (batch.Count == 0)
            return;

        await _db.FlightSeats.AddRangeAsync(batch, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    private static IEnumerable<string> EnumerateDistinctSeatCodes(int count, HashSet<string> reservedCodes)
    {
        if (count <= 0)
            yield break;

        for (var row = 1; ; row++)
        {
            foreach (var letter in "ABCDEFGHJKLMNOPQRSTUVWXY")
            {
                var code = $"{row}{letter}";
                if (!reservedCodes.Add(code))
                    continue;
                yield return code;
                count--;
                if (count <= 0)
                    yield break;
            }

            if (count <= 0)
                yield break;
        }
    }

    private async Task<int> ResolveBookingIdForTicketAsync(int clientId, int ticketId, CancellationToken cancellationToken)
    {
        var bookingIds = await _db.Bookings.AsNoTracking().Where(b => b.ClientId == clientId).Select(b => b.Id).ToListAsync(cancellationToken);
        var bookingFlights = await _db.BookingFlights.AsNoTracking().Where(bf => bookingIds.Contains(bf.BookingId)).ToListAsync(cancellationToken);
        var frs = await _db.FlightReservations.AsNoTracking().Where(fr => bookingFlights.Select(bf => bf.Id).Contains(fr.BookingFlightId)).ToListAsync(cancellationToken);
        var prs = await _db.PassengerReservations.AsNoTracking().Where(pr => frs.Select(fr => fr.Id).Contains(pr.Flight_Reservation_Id)).ToListAsync(cancellationToken);
        var ticket = await _db.Tickets.AsNoTracking().FirstOrDefaultAsync(t => t.Id == ticketId, cancellationToken);
        if (ticket is null) throw new InvalidOperationException("Tiquete no encontrado.");

        var pr = prs.FirstOrDefault(x => x.Id == ticket.PassengerReservation_Id);
        if (pr is null) throw new InvalidOperationException("Reserva de pasajero no encontrada.");

        var fr = frs.FirstOrDefault(x => x.Id == pr.Flight_Reservation_Id);
        if (fr is null) throw new InvalidOperationException("Reserva de vuelo no encontrada.");

        var bf = bookingFlights.FirstOrDefault(x => x.Id == fr.BookingFlightId);
        if (bf is null) throw new InvalidOperationException("Reserva-vuelo no encontrada.");

        return bf.BookingId;
    }

    private async Task<int> ResolveAutomaticStaffIdAsync(CancellationToken cancellationToken)
    {
        var staffId = await _db.Staffs.AsNoTracking()
            .Where(s => s.IsActive)
            .OrderBy(s => s.Id)
            .Select(s => s.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (staffId > 0)
            return staffId;

        var positionId = await EnsureAutomaticCheckinPositionIdAsync(cancellationToken);
        var documentTypeId = await GetAnyDocumentTypeIdAsync(cancellationToken);
        var person = await GetOrCreatePersonAsync(
            documentTypeId,
            "9000000000",
            "Agente",
            "Checkin",
            cancellationToken);

        var existingStaff = await _db.Staffs.FirstOrDefaultAsync(s => s.PersonId == person.Id, cancellationToken);
        if (existingStaff is not null)
        {
            if (!existingStaff.IsActive)
            {
                existingStaff.IsActive = true;
                existingStaff.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync(cancellationToken);
            }

            return existingStaff.Id;
        }

        var now = DateTime.UtcNow;
        var created = new StaffEntity
        {
            PersonId = person.Id,
            PositionId = positionId,
            AirlineId = null,
            AirportId = null,
            HireDate = now.Date,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };

        _db.Staffs.Add(created);
        await _db.SaveChangesAsync(cancellationToken);
        return created.Id;
    }

    private async Task<int> EnsureAutomaticCheckinPositionIdAsync(CancellationToken cancellationToken)
    {
        var existingId = await _db.PersonalPositions.AsNoTracking()
            .Where(p => p.Name == "Check-In Agent")
            .Select(p => p.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingId > 0)
            return existingId;

        var fallbackId = await _db.PersonalPositions.AsNoTracking()
            .OrderBy(p => p.Id)
            .Select(p => p.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (fallbackId > 0)
            return fallbackId;

        var created = new GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Infrastructure.Persistence.Entities.PersonalPositionEntity
        {
            Name = "Check-In Agent"
        };

        _db.PersonalPositions.Add(created);
        await _db.SaveChangesAsync(cancellationToken);
        return created.Id;
    }

    private async Task<string> ResolvePassengerNameForTicketAsync(int ticketId, CancellationToken cancellationToken)
    {
        var ticket = await _db.Tickets.AsNoTracking().FirstAsync(t => t.Id == ticketId, cancellationToken);
        var pr = await _db.PassengerReservations.AsNoTracking().FirstAsync(p => p.Id == ticket.PassengerReservation_Id, cancellationToken);
        var passenger = await _db.Passengers.AsNoTracking().FirstAsync(p => p.Id == pr.Passenger_Id, cancellationToken);
        var person = await _db.Persons.AsNoTracking().FirstAsync(p => p.Id == passenger.PersonId, cancellationToken);
        return $"{person.FirstName} {person.LastName}";
    }
}
