using DomainTransition = GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.Aggregate.FlightStatusTransition;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Application.Interfaces;

public interface IFlightStatusTransitionsService
{
    Task<IEnumerable<DomainTransition>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DomainTransition?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainTransition>> GetByFromStatusIdAsync(int fromStatusId, CancellationToken cancellationToken = default);
    Task<bool> ValidateTransitionAsync(int fromStatusId, int toStatusId, CancellationToken cancellationToken = default);
    Task CreateAsync(int fromStatusId, int toStatusId, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, int fromStatusId, int toStatusId, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
}
