using System;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.Repositories;

public interface ITicketRepository
{
    Task<TicketAggregate?> GetByIdAsync(TicketId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TicketAggregate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(TicketAggregate ticket, CancellationToken cancellationToken = default);
    Task DeleteAsync(TicketId id, CancellationToken cancellationToken = default);
    Task UpdateAsync(TicketAggregate ticket, CancellationToken cancellationToken = default); 
}
