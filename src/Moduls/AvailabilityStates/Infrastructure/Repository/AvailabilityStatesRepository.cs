using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Infrastructure.Repository;

public sealed class AvailabilityStatesRepository : IAvailabilityStatesRepository
{
    private readonly AppDbContext _context;

    public AvailabilityStatesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AvailabilityState?> GetByIdAsync(AvailabilityStatesId id)
    {
        var entity = await _context.AvailabilityStates.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id.Value);
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<AvailabilityState?> GetByNameAsync(AvailabilityStatesName name)
    {
        var normalized = name.Value.Trim().ToLower();
        var entity = await _context.AvailabilityStates.AsNoTracking().FirstOrDefaultAsync(x => x.Name.ToLower() == normalized);
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<AvailabilityState>> GetByStaffIdAsync(PersonalId staffId)
    {
        var entities = await (
            from state in _context.AvailabilityStates.AsNoTracking()
            join availability in _context.StaffAvailabilities.AsNoTracking() on state.Id equals availability.AvailabilityStatusId
            where availability.StaffId == staffId.Value
            orderby state.Name
            select state
        ).Distinct().ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<AvailabilityState>> GetAllAsync()
    {
        var entities = await _context.AvailabilityStates.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(AvailabilityState state)
    {
        await _context.AvailabilityStates.AddAsync(MapToEntity(state));
    }

    public Task UpdateAsync(AvailabilityState state)
    {
        _context.AvailabilityStates.Update(MapToEntity(state));
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(AvailabilityStatesId id)
    {
        var entity = await _context.AvailabilityStates.FirstOrDefaultAsync(x => x.Id == id.Value);
        if (entity is not null)
            _context.AvailabilityStates.Remove(entity);
    }

    public async Task DeleteByNameAsync(AvailabilityStatesName name)
    {
        var normalized = name.Value.Trim().ToLower();
        var entity = await _context.AvailabilityStates.FirstOrDefaultAsync(x => x.Name.ToLower() == normalized);
        if (entity is not null)
            _context.AvailabilityStates.Remove(entity);
    }

    private static AvailabilityState MapToDomain(AvailabilityStateEntity entity)
    {
        return AvailabilityState.FromPrimitives(entity.Id, entity.Name);
    }

    private static AvailabilityStateEntity MapToEntity(AvailabilityState aggregate)
    {
        return new AvailabilityStateEntity
        {
            Id = aggregate.Id?.Value ?? 0,
            Name = aggregate.Name.Value
        };
    }
}
