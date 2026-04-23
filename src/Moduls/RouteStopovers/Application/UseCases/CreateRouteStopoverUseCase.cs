using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Application.UseCases;

public sealed class CreateRouteStopoverUseCase
{
    private readonly IRouteStopoversRepository _repository;
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRouteStopoverUseCase(
        IRouteStopoversRepository repository,
        AppDbContext context,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int routeId, int stopoverAirportId, int stopOrder, int layoverMin, CancellationToken cancellationToken = default)
    {
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

        if (duplicate is not null)
            throw new InvalidOperationException($"Ya existe una escala con stop_order {stopOrder} para la ruta {routeId}.");

        var aggregate = RouteStopover.Create(routeId, stopoverAirportId, stopOrder, layoverMin);
        await _repository.SaveAsync(aggregate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
