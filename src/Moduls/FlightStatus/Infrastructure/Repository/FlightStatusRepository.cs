using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using DomainFlightStatus = GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.Aggregate.FlightStatus;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Infrastructure.Repository;

public sealed class FlightStatusRepository : IFlightStatusRepository
{
    private readonly AppDbContext _context;

    public FlightStatusRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DomainFlightStatus?> GetByIdAsync(FlightStatusId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.FlightStatuses.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<DomainFlightStatus?> GetByNameAsync(FlightStatusName name, CancellationToken cancellationToken = default)
    {
        var normalized = name.Value;
        var entity = await _context.FlightStatuses.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == normalized.ToLower(), cancellationToken);
        return entity is null ? null : MapToDomain(entity);
    }

    public Task<DomainFlightStatus?> GetByNameStringAsync(string name, CancellationToken cancellationToken = default)
        => GetByNameAsync(FlightStatusName.Create(name), cancellationToken);

    public async Task<IEnumerable<DomainFlightStatus>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.FlightStatuses.AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(DomainFlightStatus flightStatus, CancellationToken cancellationToken = default)
    {
        await _context.FlightStatuses.AddAsync(new FlightStatusEntity
        {
            Name = flightStatus.Name.Value
        }, cancellationToken);
    }

    public async Task UpdateAsync(DomainFlightStatus flightStatus, CancellationToken cancellationToken = default)
    {
        if (flightStatus.Id is null)
            throw new InvalidOperationException("No se puede actualizar un estado de vuelo sin id.");

        var entity = await _context.FlightStatuses
            .FirstOrDefaultAsync(x => x.Id == flightStatus.Id.Value, cancellationToken);

        if (entity is null)
            throw new InvalidOperationException($"No se encontro el estado de vuelo con id {flightStatus.Id.Value}.");

        entity.Name = flightStatus.Name.Value;
    }

    public async Task DeleteAsync(FlightStatusId id, CancellationToken cancellationToken = default)
    {
        await _context.FlightStatuses
            .Where(x => x.Id == id.Value)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<bool> DeleteByNameAsync(FlightStatusName name, CancellationToken cancellationToken = default)
    {
        var n = await _context.FlightStatuses
            .Where(x => x.Name.ToLower() == name.Value.ToLower())
            .ExecuteDeleteAsync(cancellationToken);
        return n > 0;
    }

    private static DomainFlightStatus MapToDomain(FlightStatusEntity e)
        => DomainFlightStatus.FromPrimitives(e.Id, e.Name);
}
