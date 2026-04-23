using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Infrastructure.Repository;

public sealed class TicketStatesRepository : ITicketStatesRepository
{
    private readonly AppDbContext _context;

    public TicketStatesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TicketState?> GetByIdAsync(
        TicketStatesId id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Set<TicketStatesEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<TicketState>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Set<TicketStatesEntity>()
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task SaveAsync(
        TicketState ticketState,
        CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(ticketState);
        await _context.Set<TicketStatesEntity>().AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        ticketState.SetId(entity.Id);
    }

    public async Task DeleteAsync(
        TicketStatesId id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Set<TicketStatesEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        if (entity is null) return;

        _context.Set<TicketStatesEntity>().Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }


    private static TicketState MapToDomain(TicketStatesEntity entity)
    {
        var domain = TicketState.Create(entity.Id, entity.Name);
        return domain;
    }

    private static TicketStatesEntity MapToEntity(TicketState domain)
    {
        return new TicketStatesEntity
        {
            Name = domain.Name.Value
        };
    }
}