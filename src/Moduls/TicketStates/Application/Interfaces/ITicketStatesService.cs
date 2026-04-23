using System;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.Aggregate;
namespace GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Application.Interfaces;

public interface ITicketStatesService
{
    Task<TicketState> CreateAsync(int id, string name, CancellationToken cancellationToken = default);

    Task<TicketState?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TicketState>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TicketState> UpdateAsync(int id, string name, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
