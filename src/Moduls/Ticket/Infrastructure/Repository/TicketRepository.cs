using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Infrastructure.Repository;

public sealed class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TicketAggregate?> GetByIdAsync(
        TicketId id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Set<TicketEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<TicketAggregate>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Set<TicketEntity>()
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(
        TicketAggregate ticket,
        CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(ticket);
        await _context.Set<TicketEntity>().AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        ticket.SetId(entity.Id);
    }

    public async Task UpdateAsync(
        TicketAggregate ticket,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Set<TicketEntity>()
            .FirstOrDefaultAsync(x => x.Id == ticket.Id.Value, cancellationToken);

        if (entity is null) return;

        entity.Code = ticket.Code.Value;
        entity.IssueDate = ticket.IssueDate.Value;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.PassengerReservation_Id = ticket.ReservationPassengerId.Value;
        entity.TicketState_Id = ticket.StatesId.Value;

        _context.Set<TicketEntity>().Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        TicketId id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Set<TicketEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        if (entity is null) return;

        _context.Set<TicketEntity>().Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
    private static TicketAggregate MapToDomain(TicketEntity entity)
    {
        var domain = TicketAggregate.Create(
            entity.Id,
            entity.Code,
            entity.IssueDate,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.PassengerReservation_Id,
            entity.TicketState_Id
        );
        return domain;
    }

    private static TicketEntity MapToEntity(TicketAggregate domain)
    {
        return new TicketEntity
        {
            Code = domain.Code.Value,
            IssueDate = domain.IssueDate.Value,
            CreatedAt = domain.CreatedAt.Value,
            UpdatedAt = domain.UpdateAt.Value,
            PassengerReservation_Id = domain.ReservationPassengerId.Value,
            TicketState_Id = domain.StatesId.Value
        };
    }
}