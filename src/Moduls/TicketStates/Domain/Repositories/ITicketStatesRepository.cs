using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.Repositories;

public interface ITicketStatesRepository
{
    Task<TicketState?> GetByIdAsync(TicketStatesId id,CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TicketState>> GetAllAsync(CancellationToken cancellationToken = default);

    Task SaveAsync(TicketState ticketStates,CancellationToken cancellationToken = default);

    Task DeleteAsync(TicketStatesId id,CancellationToken cancellationToken = default);
}