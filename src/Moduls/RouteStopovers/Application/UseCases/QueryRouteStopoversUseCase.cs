using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Application.UseCases;

public sealed class QueryRouteStopoversUseCase
{
    private readonly IRouteStopoversRepository _repository;

    public QueryRouteStopoversUseCase(IRouteStopoversRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyCollection<RouteStopover>> GetAllAsync(CancellationToken cancellationToken = default)
        => _repository.GetAllAsync(cancellationToken);

    public async Task<RouteStopover?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _repository.GetByIdAsync(RouteStopOversId.Create(id), cancellationToken);

    public Task<IReadOnlyCollection<RouteStopover>> GetByRouteIdAsync(int routeId, CancellationToken cancellationToken = default)
        => _repository.GetByRouteIdAsync(RouteId.Create(routeId), cancellationToken);
}
