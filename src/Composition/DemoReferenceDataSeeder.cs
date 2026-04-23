using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.People.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Composition;

/// <summary>
/// Datos mínimos que otros seeders o el menú dan por supuestos pero no existían en el arranque.
/// </summary>
internal sealed class DemoReferenceDataSeeder
{
    private readonly AppDbContext _db;

    // Estos estados son los que el sistema necesita ver desde el principio
    // para que los vuelos demo y varios menús funcionen con sentido.
    private static readonly string[] FlightStatusNames =
    {
        "Scheduled", "Boarding", "In Flight", "Completed", "Cancelled", "Rescheduled"
    };

    public DemoReferenceDataSeeder(AppDbContext db)
    {
        _db = db;
    }

    public async Task SeedFlightStatusesPassengerTypesAndDemoPersonsAsync(CancellationToken cancellationToken = default)
    {
        // Primero garantizamos estados de vuelo básicos.
        var existingStatuses = await _db.FlightStatuses
            .Select(x => x.Name)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase, cancellationToken);

        foreach (var name in FlightStatusNames)
        {
            if (existingStatuses.Contains(name))
                continue;
            await _db.FlightStatuses.AddAsync(new FlightStatusEntity { Name = name }, cancellationToken);
        }

        await _db.SaveChangesAsync(cancellationToken);

        // Luego dejamos los tipos de pasajero que otras reglas del sistema esperan.
        var existingPt = await _db.PassengerTypes
            .Select(x => x.Name)
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase, cancellationToken);

        (string Name, int? Min, int? Max)[] types =
        {
            ("Adult", 18, null),
            ("Senior", 60, null),
            ("Child", 2, 17),
            ("Infant", 0, 2)
        };

        foreach (var t in types)
        {
            if (existingPt.Contains(t.Name))
                continue;
            await _db.PassengerTypes.AddAsync(new PassengerTypeEntity
            {
                Name = t.Name,
                MinAge = t.Min,
                MaxAge = t.Max
            }, cancellationToken);
        }

        await _db.SaveChangesAsync(cancellationToken);

        var ccTypeId = await _db.DocumentTypes.AsNoTracking()
            .Where(x => x.Code == "CC")
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (ccTypeId is null)
            return;

        var now = DateTime.UtcNow;
        // Estas personas demo sirven para que el portal y algunos seeders posteriores
        // no arranquen completamente vacíos.
        var demoPeople = new[]
        {
            ("1001", "María", "García", 'F'),
            ("1002", "Carlos", "López", 'M')
        };

        foreach (var (doc, first, last, gender) in demoPeople)
        {
            var exists = await _db.Persons.AsNoTracking()
                .AnyAsync(x => x.DocumentNumber == doc, cancellationToken);
            if (exists)
                continue;

            await _db.Persons.AddAsync(new PersonEntity
            {
                DocumentTypeId = ccTypeId.Value,
                DocumentNumber = doc,
                FirstName = first,
                LastName = last,
                BirthDate = new DateTime(1990, 1, 15),
                Gender = gender,
                AddressId = null,
                CreatedAt = now,
                UpdatedAt = now
            }, cancellationToken);
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task SeedDemoFlightsAsync(CancellationToken cancellationToken = default)
    {
        // Si ya hay algunos vuelos, aún queremos poblar más para pruebas del portal.
        var existingCount = await _db.Flights.AsNoTracking().CountAsync(cancellationToken);
        if (existingCount >= 60)
            return;

        var scheduledId = await _db.FlightStatuses.AsNoTracking()
            .Where(x => x.Name == "Scheduled")
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (scheduledId is null)
            return;

        var airlines = await _db.Airlines.AsNoTracking().OrderBy(x => x.Id).Take(3).ToListAsync(cancellationToken);
        var routes = await _db.Routes.AsNoTracking().OrderBy(x => x.Id).Take(8).ToListAsync(cancellationToken);
        var aircraft = await _db.Aircrafts.AsNoTracking().OrderBy(x => x.Id).Take(6).ToListAsync(cancellationToken);

        if (airlines.Count == 0 || routes.Count == 0 || aircraft.Count == 0)
            return;

        var startDay = DateTime.UtcNow.Date.AddDays(1);
        var now = DateTime.UtcNow;

        // Generamos vuelos variados para que las pruebas no queden todas con los mismos horarios.
        var seeds = new List<(string Code, int AirlineId, int RouteId, int AircraftId, DateTime Dep, DateTime Arr)>();
        var codeSeq = 200 + existingCount;
        var baseSlots = new[]
        {
            (Hour: 5, Minute: 40),
            (Hour: 7, Minute: 15),
            (Hour: 9, Minute: 50),
            (Hour: 12, Minute: 25),
            (Hour: 15, Minute: 10),
            (Hour: 17, Minute: 45),
            (Hour: 20, Minute: 5),
            (Hour: 22, Minute: 20)
        };

        for (var day = 0; day < 10; day++)
        {
            foreach (var slot in baseSlots)
            {
                foreach (var route in routes)
                {
                    var offsetSeed = route.Id + day + slot.Hour + slot.Minute;
                    var al = airlines[offsetSeed % airlines.Count];
                    var ac = aircraft[offsetSeed % aircraft.Count];

                    var departureMinuteOffset = ((route.Id * 13) + (day * 7) + slot.Minute) % 35;
                    var dep = startDay
                        .AddDays(day)
                        .AddHours(slot.Hour)
                        .AddMinutes(slot.Minute + departureMinuteOffset);

                    var baseMinutes = route.EstimatedDurationMin ?? 120;
                    var durationVariation = ((route.Id * 11) + (day * 5) + slot.Hour) % 51 - 25;
                    var minutes = Math.Max(45, baseMinutes + durationVariation);
                    var arr = dep.AddMinutes(minutes);
                    var code = $"AV{codeSeq++}";
                    seeds.Add((code, al.Id, route.Id, ac.Id, dep, arr));
                    if (seeds.Count + existingCount >= 80)
                        break;
                }
                if (seeds.Count + existingCount >= 80)
                    break;
            }
            if (seeds.Count + existingCount >= 80)
                break;
        }

        foreach (var s in seeds)
        {
            var codeExists = await _db.Flights.AsNoTracking().AnyAsync(x => x.FlightCode == s.Code, cancellationToken);
            if (codeExists)
                continue;

            await _db.Flights.AddAsync(new FlightEntity
            {
                FlightCode = s.Code,
                AirlineId = s.AirlineId,
                RouteId = s.RouteId,
                AircraftId = s.AircraftId,
                DepartureAt = s.Dep,
                EstimatedArrivalAt = s.Arr,
                TotalCapacity = 180,
                AvailableSeats = 180,
                FlightStatusId = scheduledId.Value,
                RescheduledAt = null,
                CreatedAt = now,
                UpdatedAt = now
            }, cancellationToken);
        }

        await _db.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Por cada vuelo existente A→B, crea un vuelo de regreso B→A al día siguiente (misma hora aproximada),
    /// si existe en catálogo una ruta inversa entre las mismas ciudades.
    /// </summary>
    public async Task SeedPairedReturnFlightsAsync(CancellationToken cancellationToken = default)
    {
        // Si ya existen vuelos de retorno generados por este seeder, no los duplicamos.
        if (await _db.Flights.AsNoTracking().AnyAsync(f => f.FlightCode.StartsWith("RTP"), cancellationToken))
            return;

        var scheduledId = await _db.FlightStatuses.AsNoTracking()
            .Where(x => x.Name == "Scheduled")
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (scheduledId is null)
            return;

        var flights = await _db.Flights.AsNoTracking().ToListAsync(cancellationToken);
        var routes = await _db.Routes.AsNoTracking().ToListAsync(cancellationToken);
        var airports = await _db.Airports.AsNoTracking().ToListAsync(cancellationToken);

        if (routes.Count == 0 || airports.Count == 0 || flights.Count == 0)
            return;

        var cityByAirportId = airports.ToDictionary(a => a.Id, a => a.CityId);

        int? FindReverseRouteId(int routeId)
        {
            var r = routes.FirstOrDefault(x => x.Id == routeId);
            if (r is null)
                return null;
            if (!cityByAirportId.TryGetValue(r.OriginAirportId, out var oCity) ||
                !cityByAirportId.TryGetValue(r.DestinationAirportId, out var dCity))
                return null;

            var rev = routes.FirstOrDefault(rr =>
                cityByAirportId.TryGetValue(rr.OriginAirportId, out var oc) &&
                cityByAirportId.TryGetValue(rr.DestinationAirportId, out var dc) &&
                oc == dCity && dc == oCity);

            return rev?.Id;
        }

        var maxCodeNum = 0;
        foreach (var f in flights)
        {
            foreach (var prefix in new[] { "AV", "RT", "RTP" })
            {
                if (f.FlightCode.Length <= prefix.Length ||
                    !f.FlightCode.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    continue;
                if (int.TryParse(f.FlightCode.AsSpan(prefix.Length), out var n))
                    maxCodeNum = Math.Max(maxCodeNum, n);
            }
        }

        var codeSeq = maxCodeNum + 1;
        var existingDepartureKeys = new HashSet<(int RouteId, long DepTicks)>(
            flights.Select(f => (f.RouteId, f.DepartureAt.Ticks)));

        var now = DateTime.UtcNow;
        var toAdd = new List<FlightEntity>();

        foreach (var f in flights.OrderBy(x => x.DepartureAt).ThenBy(x => x.RouteId))
        {
            if (f.FlightCode.StartsWith("RTP", StringComparison.OrdinalIgnoreCase))
                continue;

            var revRouteId = FindReverseRouteId(f.RouteId);
            if (revRouteId is null)
                continue;

            var revRoute = routes.First(rr => rr.Id == revRouteId.Value);
            var returnHourOffset = ((f.RouteId * 3) + (f.AirlineId * 2)) % 9 + 2;
            var returnMinuteOffset = ((f.RouteId * 17) + f.AirlineId) % 50;
            var depReturn = f.DepartureAt
                .AddDays(1)
                .AddHours(returnHourOffset)
                .AddMinutes(returnMinuteOffset);
            var legMinutes = revRoute.EstimatedDurationMin is > 0
                ? revRoute.EstimatedDurationMin.Value
                : (int)Math.Clamp((f.EstimatedArrivalAt - f.DepartureAt).TotalMinutes, 45, 600);
            var variedLegMinutes = Math.Max(45, legMinutes + (((f.RouteId * 7) + f.AirlineId) % 41 - 20));
            var arrReturn = depReturn.AddMinutes(variedLegMinutes);

            var key = (revRouteId.Value, depReturn.Ticks);
            if (existingDepartureKeys.Contains(key))
                continue;

            var code = $"RTP{codeSeq++}";
            while (flights.Exists(x => x.FlightCode == code) || toAdd.Exists(x => x.FlightCode == code))
                code = $"RTP{codeSeq++}";

            toAdd.Add(new FlightEntity
            {
                FlightCode = code,
                AirlineId = f.AirlineId,
                RouteId = revRouteId.Value,
                AircraftId = f.AircraftId,
                DepartureAt = depReturn,
                EstimatedArrivalAt = arrReturn,
                TotalCapacity = f.TotalCapacity,
                AvailableSeats = f.AvailableSeats,
                FlightStatusId = scheduledId.Value,
                RescheduledAt = null,
                CreatedAt = now,
                UpdatedAt = now
            });

            existingDepartureKeys.Add(key);
        }

        if (toAdd.Count == 0)
            return;

        await _db.Flights.AddRangeAsync(toAdd, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
