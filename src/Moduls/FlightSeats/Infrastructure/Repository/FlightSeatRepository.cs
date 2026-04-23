using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Infrastructure.
Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Infrastructure.Repository;

public sealed class FlightSeatRepository : IFlightSeatsRepository
{
    private readonly AppDbContext _context;

    public FlightSeatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(FlightSeat flightSeat, CancellationToken cancellationToken = default)
    {
        await _context.FlightSeats.AddAsync(MapToEntity(flightSeat), cancellationToken);
    }

    public async Task<FlightSeat?> GetByIdAsync(FlightSeatsId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.FlightSeats
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<FlightSeat?> GetByCodeAsync(FlightSeatsCode code, CancellationToken cancellationToken = default)
    {
        var entity = await _context.FlightSeats
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == code.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<FlightSeat>> GetByFlightIdAsync(FlightsId flightId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.FlightSeats
            .AsNoTracking()
            .Where(x => x.FlightId == flightId.Value)
            .OrderBy(x => x.Code)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<FlightSeat>> GetByCabinTypeIdAsync(CabinTypesId cabinTypeId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.FlightSeats
            .AsNoTracking()
            .Where(x => x.CabinTypeId == cabinTypeId.Value)
            .OrderBy(x => x.Code)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<FlightSeat>> GetBySeatLocationTypeIdAsync (SeatLocationTypesId seatLocationTypeId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.FlightSeats
            .AsNoTracking()
            .Where(x => x.SeatLocationTypeId == seatLocationTypeId.Value)
            .OrderBy(x => x.Code)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain).ToList();
    }
    public async Task<IReadOnlyCollection<FlightSeat>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.FlightSeats
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain).ToList();
    }

    public Task<bool> ExistsByCodeAsync(FlightSeatsCode code, CancellationToken cancellationToken = default)
        => _context.FlightSeats.AnyAsync(x => x.Code == code.Value, cancellationToken);

    public Task<bool> HasFutureFlightsAsync(FlightSeatsId id, CancellationToken cancellationToken = default)
        => _context.Flights.AnyAsync(
            x => x.FlightStatusId == id.Value && x.DepartureAt > DateTime.UtcNow,
            cancellationToken);
    public async Task UpdateAsync(FlightSeat flightSeat, CancellationToken cancellationToken = default)
    {
        var entity = await _context.FlightSeats.FirstOrDefaultAsync(x => x.Id == flightSeat.Id.Value, cancellationToken);

        if (entity is null)
            throw new InvalidOperationException($"No se encontro el asiento con id {flightSeat.Id.Value}.");

        entity.FlightId = flightSeat.FlightId.Value;
        entity.CabinTypeId = flightSeat.CabinTypeId.Value;
        entity.SeatLocationTypeId = flightSeat.SeatLocationTypeId.Value;
        entity.IsOccupied = flightSeat.IsOccupied.Value;
        entity.Code = flightSeat.Code.Value;
    }
    public async Task<bool> DeleteByIdAsync(FlightSeatsId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.FlightSeats.FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);
        if (entity is null)
            return false;

        _context.FlightSeats.Remove(entity);
        return true;
    }

    private static FlightSeat MapToDomain(FlightSeatEntity entity)
        => FlightSeat.FromPrimitives(
            entity.Id,
            entity.FlightId,
            entity.CabinTypeId,
            entity.SeatLocationTypeId,
            entity.IsOccupied,
            entity.Code);

    private static FlightSeatEntity MapToEntity(FlightSeat aggregate)
        => new()
        {
            FlightId = aggregate.FlightId.Value,
            CabinTypeId = aggregate.CabinTypeId.Value,
            SeatLocationTypeId = aggregate.SeatLocationTypeId.Value,
            IsOccupied = aggregate.IsOccupied.Value,
            Code = aggregate.Code.Value
        };
}
