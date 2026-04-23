using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Application.UseCases;

public sealed class UpdateRouteStopoverUseCase
{
    private readonly IRouteStopoversRepository _repository;
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRouteStopoverUseCase(
        IRouteStopoversRepository repository,
        AppDbContext context,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, int routeId, int stopoverAirportId, int stopOrder, int layoverMin, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(RouteStopOversId.Create(id), cancellationToken);
        if (existing is null)
            throw new InvalidOperationException($"No se encontro la escala con ID {id}.");

        var routeExists = await _context.Routes.AsNoTracking()
            .AnyAsync(x => x.Id == routeId, cancellationToken);
        if (!routeExists)
            throw new InvalidOperationException($"No se encontro la ruta con ID {routeId}.");

        var airportExists = await _context.Airports.AsNoTracking()
            .AnyAsync(x => x.Id == stopoverAirportId, cancellationToken);
        if (!airportExists)
            throw new InvalidOperationException($"No se encontro el aeropuerto de escala con ID {stopoverAirportId}.");

        var duplicate = await _repository.GetByRouteIdAndStopOrderAsync(
            RouteId.Create(routeId),
            RouteStopOrder.Create(stopOrder),
            cancellationToken);

        if (duplicate is not null && duplicate.Id?.Value != id)
            throw new InvalidOperationException($"Ya existe otra escala con stop_order {stopOrder} para la ruta {routeId}.");

        existing.Update(routeId, stopoverAirportId, stopOrder, layoverMin);
        await _repository.UpdateAsync(existing, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
