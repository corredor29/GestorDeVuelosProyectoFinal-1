using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Infrastructure.Repository;

public sealed class SeatLocationTypesRepository : ISeatLocationTypesRepository
{
    private readonly AppDbContext _context;

    public SeatLocationTypesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SeatLocationType?> GetByIdAsync(SeatLocationTypesId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.SeatLocationTypes.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<SeatLocationType?> GetByNameAsync(SeatLocationTypesName name, CancellationToken cancellationToken = default)
    {
        var normalized = name.Value;
        var entity = await _context.SeatLocationTypes.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == normalized.ToLower(), cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public Task<SeatLocationType?> GetByNameStringAsync(string name, CancellationToken cancellationToken = default)
        => GetByNameAsync(SeatLocationTypesName.Create(name), cancellationToken);

    public async Task<IEnumerable<SeatLocationType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.SeatLocationTypes.AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(SeatLocationType seatLocationType, CancellationToken cancellationToken = default)
    {
        await _context.SeatLocationTypes.AddAsync(new SeatLocationTypesEntity
        {
            Name = seatLocationType.Name.Value
        }, cancellationToken);
    }

    public async Task UpdateAsync(SeatLocationType seatLocationType, CancellationToken cancellationToken = default)
    {
        if (seatLocationType.Id is null)
            throw new InvalidOperationException("No se puede actualizar un tipo de asiento sin id.");

        var entity = await _context.SeatLocationTypes
            .FirstOrDefaultAsync(x => x.Id == seatLocationType.Id.Value, cancellationToken);

        if (entity is null)
            throw new InvalidOperationException($"No se encontro el tipo de asiento con id {seatLocationType.Id.Value}.");

        entity.Name = seatLocationType.Name.Value;
    }

    public async Task DeleteAsync(SeatLocationTypesId id, CancellationToken cancellationToken = default)
    {
        await _context.SeatLocationTypes
            .Where(x => x.Id == id.Value)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<bool> DeleteByNameAsync(SeatLocationTypesName name, CancellationToken cancellationToken = default)
    {
        var deleted = await _context.SeatLocationTypes
            .Where(x => x.Name.ToLower() == name.Value.ToLower())
            .ExecuteDeleteAsync(cancellationToken);

        return deleted > 0;
    }

    private static SeatLocationType MapToDomain(SeatLocationTypesEntity entity)
        => SeatLocationType.FromPrimitives(entity.Id, entity.Name);
}
