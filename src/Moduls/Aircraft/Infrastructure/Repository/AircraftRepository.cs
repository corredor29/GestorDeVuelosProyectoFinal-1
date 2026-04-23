using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using AircraftAggregate = GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Aggregate.Aircraft;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Infrastructure.Repository;

public sealed class AircraftRepository : IAircraftRepository
{
    private readonly AppDbContext _context;

    public AircraftRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AircraftAggregate aircraft, CancellationToken cancellationToken = default)
    {
        await _context.Aircrafts.AddAsync(MapToEntity(aircraft), cancellationToken);
    }

    public async Task<AircraftAggregate?> GetByIdAsync(AircraftId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Aircrafts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<AircraftAggregate?> GetByRegistrationAsync(AircraftRegistration registration, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Aircrafts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Registration == registration.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<AircraftAggregate>> GetByAirlineIdAsync(AirlinesId airlineId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.Aircrafts
            .AsNoTracking()
            .Where(x => x.AirlinesId == airlineId.Value)
            .OrderBy(x => x.Registration)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<AircraftAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.Aircrafts
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain).ToList();
    }

    public Task<bool> ExistsByRegistrationAsync(AircraftRegistration registration, CancellationToken cancellationToken = default)
        => _context.Aircrafts.AnyAsync(x => x.Registration == registration.Value, cancellationToken);

    public Task<bool> HasFutureFlightsAsync(AircraftId id, CancellationToken cancellationToken = default)
        => _context.Flights.AnyAsync(
            x => x.AircraftId == id.Value && x.DepartureAt > DateTime.UtcNow,
            cancellationToken);

    public async Task UpdateAsync(AircraftAggregate aircraft, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Aircrafts
            .FirstOrDefaultAsync(x => x.Id == aircraft.Id.Value, cancellationToken);

        if (entity is null)
            throw new InvalidOperationException($"Aircraft with id '{aircraft.Id.Value}' not found.");

        entity.AircraftModelId = aircraft.ModelId.Value;
        entity.AirlinesId = aircraft.AirlineId.Value;
        entity.Registration = aircraft.Registration.Value;
        entity.DateManufactured = aircraft.ManufacturedDate.Value;
        entity.IsActive = aircraft.IsActive.Value;
    }

    public async Task<bool> DeleteByIdAsync(AircraftId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Aircrafts
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        if (entity is null)
            return false;

        _context.Aircrafts.Remove(entity);
        return true;
    }

    private static AircraftAggregate MapToDomain(AircraftEntity entity)
        => AircraftAggregate.FromPrimitives(
            entity.Id,
            entity.AircraftModelId,
            entity.AirlinesId,
            entity.Registration,
            entity.DateManufactured,
            entity.IsActive);

    private static AircraftEntity MapToEntity(AircraftAggregate aircraft)
        => new()
        {
            AircraftModelId = aircraft.ModelId.Value,
            AirlinesId = aircraft.AirlineId.Value,
            Registration = aircraft.Registration.Value,
            DateManufactured = aircraft.ManufacturedDate.Value,
            IsActive = aircraft.IsActive.Value
        };
}
