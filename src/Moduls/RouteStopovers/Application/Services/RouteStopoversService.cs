using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Application.UseCases;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Application.Services;

public sealed class RouteStopoversService : IRouteStopoversService
{
    private readonly QueryRouteStopoversUseCase _query;
    private readonly CreateRouteStopoverUseCase _create;
    private readonly UpdateRouteStopoverUseCase _update;
    private readonly DeleteRouteStopoversUseCase _delete;

    public RouteStopoversService(
        QueryRouteStopoversUseCase query,
        CreateRouteStopoverUseCase create,
        UpdateRouteStopoverUseCase update,
        DeleteRouteStopoversUseCase delete)
    {
        _query = query;
        _create = create;
        _update = update;
        _delete = delete;
    }

    public Task<IReadOnlyCollection<RouteStopover>> GetAllAsync(CancellationToken cancellationToken = default)
        => _query.GetAllAsync(cancellationToken);

    public Task<RouteStopover?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _query.GetByIdAsync(id, cancellationToken);

    public Task<IReadOnlyCollection<RouteStopover>> GetByRouteIdAsync(int routeId, CancellationToken cancellationToken = default)
        => _query.GetByRouteIdAsync(routeId, cancellationToken);

    public Task CreateAsync(int routeId, int stopoverAirportId, int stopOrder, int layoverMin, CancellationToken cancellationToken = default)
        => _create.ExecuteAsync(routeId, stopoverAirportId, stopOrder, layoverMin, cancellationToken);

    public Task UpdateAsync(int id, int routeId, int stopoverAirportId, int stopOrder, int layoverMin, CancellationToken cancellationToken = default)
        => _update.ExecuteAsync(id, routeId, stopoverAirportId, stopOrder, layoverMin, cancellationToken);

    public Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
        => _delete.DeleteByIdAsync(id, cancellationToken);

    public Task<int> DeleteByRouteIdAsync(int routeId, CancellationToken cancellationToken = default)
        => _delete.DeleteByRouteIdAsync(routeId, cancellationToken);
}
