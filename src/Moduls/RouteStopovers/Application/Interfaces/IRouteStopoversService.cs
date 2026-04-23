using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Application.Interfaces;

public interface IRouteStopoversService
{
    Task<IReadOnlyCollection<RouteStopover>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<RouteStopover?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Busqueda por ruta (todas las escalas de esa ruta).</summary>
    Task<IReadOnlyCollection<RouteStopover>> GetByRouteIdAsync(int routeId, CancellationToken cancellationToken = default);

    Task CreateAsync(int routeId, int stopoverAirportId, int stopOrder, int layoverMin, CancellationToken cancellationToken = default);

    Task UpdateAsync(int id, int routeId, int stopoverAirportId, int stopOrder, int layoverMin, CancellationToken cancellationToken = default);

    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<int> DeleteByRouteIdAsync(int routeId, CancellationToken cancellationToken = default);
}
