using System.Linq.Expressions;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;
using DomainFlights = GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Aggregate.Flights;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Repository;

public sealed class FlightsRepository : IFlightsRepository
{
    private readonly AppDbContext _context;

    public FlightsRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Elimina asignaciones de tripulacion que apuntan a vuelos que cumplen el predicado, luego elimina esos vuelos.</summary>
    private async Task<int> DeleteFlightsAndStaffAssignmentsAsync(
        Expression<Func<FlightEntity, bool>> flightPredicate,
        CancellationToken cancellationToken)
    {
        var flightIds = await _context.Flights.AsNoTracking()
            .Where(flightPredicate)
            .Select(f => f.Id)
            .ToListAsync(cancellationToken);

        if (flightIds.Count == 0)
            return 0;

        await _context.FlightAssignments
            .Where(a => flightIds.Contains(a.FlightId))
            .ExecuteDeleteAsync(cancellationToken);

        return await _context.Flights
            .Where(f => flightIds.Contains(f.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<DomainFlights?> GetByIdAsync(FlightsId id, CancellationToken cancellationToken = default)
    {
        var e = await _context.Flights.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);
        return e is null ? null : MapToDomain(e);
    }

    public async Task<DomainFlights?> GetByFlightCodeAsync(string flightCode, CancellationToken cancellationToken = default)
    {
        var normalized = FlightCode.Create(flightCode).Value;
        var e = await _context.Flights.AsNoTracking()
            .FirstOrDefaultAsync(x => x.FlightCode.ToLower() == normalized.ToLower(), cancellationToken);
        return e is null ? null : MapToDomain(e);
    }

    public async Task<IReadOnlyList<DomainFlights>> GetByAircraftIdAsync(int aircraftId, CancellationToken cancellationToken = default)
    {
        var list = await _context.Flights.AsNoTracking()
            .Where(x => x.AircraftId == aircraftId)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
        return list.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyList<DomainFlights>> GetByAircraftRegistrationAsync(string registration, CancellationToken cancellationToken = default)
    {
        var reg = registration.Trim();
        var query =
            from f in _context.Flights.AsNoTracking()
            join a in _context.Aircrafts.AsNoTracking() on f.AircraftId equals a.Id
            where a.Registration == reg
            orderby f.Id
            select f;

        var list = await query.ToListAsync(cancellationToken);
        return list.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyList<DomainFlights>> GetByTotalCapacityAsync(int totalCapacity, CancellationToken cancellationToken = default)
    {
        var list = await _context.Flights.AsNoTracking()
            .Where(x => x.TotalCapacity == totalCapacity)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
        return list.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyList<DomainFlights>> GetByRouteIdAsync(int routeId, CancellationToken cancellationToken = default)
    {
        var list = await _context.Flights.AsNoTracking()
            .Where(x => x.RouteId == routeId)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
        return list.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyList<DomainFlights>> GetByFlightStatusIdAsync(int flightStatusId, CancellationToken cancellationToken = default)
    {
        var list = await _context.Flights.AsNoTracking()
            .Where(x => x.FlightStatusId == flightStatusId)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
        return list.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyList<DomainFlights>> GetByAirlineIdAsync(int airlineId, CancellationToken cancellationToken = default)
    {
        var list = await _context.Flights.AsNoTracking()
            .Where(x => x.AirlineId == airlineId)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
        return list.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyList<DomainFlights>> GetByAirportIdAsync(int airportId, CancellationToken cancellationToken = default)
    {
        var query =
            from f in _context.Flights.AsNoTracking()
            join r in _context.Routes.AsNoTracking() on f.RouteId equals r.Id
            where r.OriginAirportId == airportId || r.DestinationAirportId == airportId
            orderby f.Id
            select f;

        var list = await query.ToListAsync(cancellationToken);
        return list.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyList<DomainFlights>> GetByDepartureBetweenAsync(
        DateTime fromUtcInclusive,
        DateTime toUtcInclusive,
        CancellationToken cancellationToken = default)
    {
        var list = await _context.Flights.AsNoTracking()
            .Where(x => x.DepartureAt >= fromUtcInclusive && x.DepartureAt <= toUtcInclusive)
            .OrderBy(x => x.DepartureAt)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken);
        return list.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<DomainFlights>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await _context.Flights.AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
        return list.Select(MapToDomain).ToList();
    }

    public async Task SaveAsync(DomainFlights flight, CancellationToken cancellationToken = default)
    {
        if (flight.Id is not null)
            throw new InvalidOperationException("El vuelo ya tiene id.");

        var now = DateTime.UtcNow;
        await _context.Flights.AddAsync(new FlightEntity
        {
            FlightCode = flight.Code.Value,
            AirlineId = flight.AirlineId.Value,
            RouteId = flight.RouteId.Value,
            AircraftId = flight.AircraftId.Value,
            DepartureAt = flight.DepartureAt.Value,
            EstimatedArrivalAt = flight.EstimatedArrivalAt.Value,
            TotalCapacity = flight.TotalCapacity.Value,
            AvailableSeats = flight.AvailableSeats.Value,
            FlightStatusId = flight.FlightStatusId.Value,
            RescheduledAt = flight.RescheduledAt.Value,
            CreatedAt = now,
            UpdatedAt = now
        }, cancellationToken);
    }

    public async Task UpdateAsync(DomainFlights flight, CancellationToken cancellationToken = default)
    {
        if (flight.Id is null)
            throw new InvalidOperationException("No se puede actualizar un vuelo sin id.");

        var entity = await _context.Flights
            .FirstOrDefaultAsync(x => x.Id == flight.Id.Value, cancellationToken);

        if (entity is null)
            throw new InvalidOperationException($"No se encontro el vuelo con id {flight.Id.Value}.");

        entity.FlightCode = flight.Code.Value;
        entity.AirlineId = flight.AirlineId.Value;
        entity.RouteId = flight.RouteId.Value;
        entity.AircraftId = flight.AircraftId.Value;
        entity.DepartureAt = flight.DepartureAt.Value;
        entity.EstimatedArrivalAt = flight.EstimatedArrivalAt.Value;
        entity.TotalCapacity = flight.TotalCapacity.Value;
        entity.AvailableSeats = flight.AvailableSeats.Value;
        entity.FlightStatusId = flight.FlightStatusId.Value;
        entity.RescheduledAt = flight.RescheduledAt.Value;
        entity.UpdatedAt = flight.UpdatedAt.Value;
    }

    public async Task DeleteAsync(FlightsId id, CancellationToken cancellationToken = default)
    {
        await _context.FlightAssignments
            .Where(a => a.FlightId == id.Value)
            .ExecuteDeleteAsync(cancellationToken);

        await _context.Flights
            .Where(x => x.Id == id.Value)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public Task<int> DeleteByFlightCodeAsync(string flightCode, CancellationToken cancellationToken = default)
    {
        var normalized = FlightCode.Create(flightCode).Value;
        return DeleteFlightsAndStaffAssignmentsAsync(
            x => x.FlightCode.ToLower() == normalized.ToLower(),
            cancellationToken);
    }

    public Task<int> DeleteByAircraftIdAsync(int aircraftId, CancellationToken cancellationToken = default)
        => DeleteFlightsAndStaffAssignmentsAsync(x => x.AircraftId == aircraftId, cancellationToken);

    public Task<int> DeleteByAircraftRegistrationAsync(string registration, CancellationToken cancellationToken = default)
    {
        var reg = registration.Trim();
        return DeleteFlightsAndStaffAssignmentsAsync(
            f => _context.Aircrafts.Any(a => a.Id == f.AircraftId && a.Registration == reg),
            cancellationToken);
    }

    public Task<int> DeleteByTotalCapacityAsync(int totalCapacity, CancellationToken cancellationToken = default)
        => DeleteFlightsAndStaffAssignmentsAsync(x => x.TotalCapacity == totalCapacity, cancellationToken);

    public Task<int> DeleteByRouteIdAsync(int routeId, CancellationToken cancellationToken = default)
        => DeleteFlightsAndStaffAssignmentsAsync(x => x.RouteId == routeId, cancellationToken);

    public Task<int> DeleteByFlightStatusIdAsync(int flightStatusId, CancellationToken cancellationToken = default)
        => DeleteFlightsAndStaffAssignmentsAsync(x => x.FlightStatusId == flightStatusId, cancellationToken);

    public Task<int> DeleteByAirlineIdAsync(int airlineId, CancellationToken cancellationToken = default)
        => DeleteFlightsAndStaffAssignmentsAsync(x => x.AirlineId == airlineId, cancellationToken);

    public Task<int> DeleteByAirportIdAsync(int airportId, CancellationToken cancellationToken = default)
        => DeleteFlightsAndStaffAssignmentsAsync(
            f => _context.Routes.Any(r =>
                r.Id == f.RouteId && (r.OriginAirportId == airportId || r.DestinationAirportId == airportId)),
            cancellationToken);

    public Task<int> DeleteByDepartureBetweenAsync(
        DateTime fromUtcInclusive,
        DateTime toUtcInclusive,
        CancellationToken cancellationToken = default)
        => DeleteFlightsAndStaffAssignmentsAsync(
            x => x.DepartureAt >= fromUtcInclusive && x.DepartureAt <= toUtcInclusive,
            cancellationToken);

    private static DomainFlights MapToDomain(FlightEntity e)
        => DomainFlights.FromPersistence(
            e.Id,
            e.FlightCode,
            e.AirlineId,
            e.RouteId,
            e.AircraftId,
            e.DepartureAt,
            e.EstimatedArrivalAt,
            e.TotalCapacity,
            e.AvailableSeats,
            e.FlightStatusId,
            e.RescheduledAt,
            e.CreatedAt,
            e.UpdatedAt);
}
