using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.Repositories;

public interface IFlightStatusTransitionsRepository
{
    Task<FlightStatusTransition?> GetByIdAsync(FlightStatusTransitionsId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<FlightStatusTransition>> GetByFromStatusIdAsync(FlightStatusId fromStatusId, CancellationToken cancellationToken = default);
    Task<FlightStatusTransition?> GetByStatusesAsync(FlightStatusId fromStatusId, FlightStatusId toStatusId, CancellationToken cancellationToken = default);
    Task<IEnumerable<FlightStatusTransition>> GetAllAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(FlightStatusTransition transition, CancellationToken cancellationToken = default);
    Task UpdateAsync(FlightStatusTransition transition, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(FlightStatusTransitionsId id, CancellationToken cancellationToken = default);
    Task<bool> ValidateTransitionAsync(FlightStatusId fromStatusId, FlightStatusId toStatusId, CancellationToken cancellationToken = default);
}
