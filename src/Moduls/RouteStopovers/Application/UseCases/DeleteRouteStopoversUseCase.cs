using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Application.UseCases;

public sealed class DeleteRouteStopoversUseCase
{
    private readonly IRouteStopoversRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRouteStopoversUseCase(IRouteStopoversRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(RouteStopOversId.Create(id), cancellationToken);
        if (existing is null)
            throw new InvalidOperationException($"No se encontro la escala con ID {id}.");

        await _repository.DeleteAsync(RouteStopOversId.Create(id), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> DeleteByRouteIdAsync(int routeId, CancellationToken cancellationToken = default)
    {
        var n = await _repository.DeleteByRouteIdAsync(RouteId.Create(routeId), cancellationToken);
        if (n > 0)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return n;
    }
}
