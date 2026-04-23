namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.Repositories;

public interface IFlightStatusTransitionRepository
{
    /// <summary>Indica si existe una transicion permitida de fromStatusId a toStatusId.</summary>
    Task<bool> IsTransitionAllowedAsync(int fromStatusId, int toStatusId, CancellationToken cancellationToken = default);

    /// <summary>Destinos permitidos desde el estado indicado (para UI o validaciones).</summary>
    Task<IReadOnlyList<int>> GetAllowedToStatusIdsAsync(int fromStatusId, CancellationToken cancellationToken = default);
}
