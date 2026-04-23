using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Infrastructure.Repository;

public sealed class CheckinStatesRepository : ICheckinStatesRepository
{
    private readonly AppDbContext _context;

    public CheckinStatesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CheckinState?> GetByIdAsync(
        CheckinStatesId id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Set<CheckinStatesEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<CheckinState>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Set<CheckinStatesEntity>()
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(
        CheckinState checkinState,
        CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(checkinState);
        await _context.Set<CheckinStatesEntity>().AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        checkinState.SetId(entity.Id);
    }

    public async Task DeleteAsync(
        CheckinStatesId id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Set<CheckinStatesEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        if (entity is null) return;

        _context.Set<CheckinStatesEntity>().Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }


    private static CheckinState MapToDomain(CheckinStatesEntity entity)
    {
        return CheckinState.Create(entity.Id, entity.Name);
    }

    private static CheckinStatesEntity MapToEntity(CheckinState domain)
    {
        return new CheckinStatesEntity
        {
            Name = domain.Name.Value
        };
    }
}