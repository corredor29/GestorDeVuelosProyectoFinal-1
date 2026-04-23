using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.Repositories;

public interface IRouteStopoversRepository
{
    Task<RouteStopover?> GetByIdAsync(RouteStopOversId id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<RouteStopover>> GetByRouteIdAsync(RouteId routeId, CancellationToken cancellationToken = default);


    Task<RouteStopover?> GetByRouteIdAndStopOrderAsync(
        RouteId routeId,
        RouteStopOrder stopOrder,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<RouteStopover>> GetAllAsync(CancellationToken cancellationToken = default);

    Task SaveAsync(RouteStopover routeStopover, CancellationToken cancellationToken = default);

    Task UpdateAsync(RouteStopover routeStopover, CancellationToken cancellationToken = default);

    Task DeleteAsync(RouteStopOversId id, CancellationToken cancellationToken = default);

    Task<int> DeleteByRouteIdAsync(RouteId routeId, CancellationToken cancellationToken = default);
}
