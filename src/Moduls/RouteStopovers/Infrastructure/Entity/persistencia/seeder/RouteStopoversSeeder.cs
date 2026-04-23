using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Infrastructure.Entity.persistencia.seeder;

public sealed class RouteStopoversSeeder
{
    private readonly AppDbContext _context;
    private readonly IRouteStopoversRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RouteStopoversSeeder(
        AppDbContext context,
        IRouteStopoversRepository repository,
        IUnitOfWork unitOfWork)
    {
        _context = context;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var routes = await _context.Routes
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Take(5)
            .ToListAsync(cancellationToken);

        var airportIds = await _context.Airports
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        if (routes.Count == 0 || airportIds.Count == 0)
            return;

        foreach (var route in routes)
        {
            var stopAirportId = airportIds.FirstOrDefault(
                a => a != route.OriginAirportId && a != route.DestinationAirportId);

            if (stopAirportId == 0)
                continue;

            var hasAnyForRoute = await _context.RouteStopovers.AsNoTracking()
                .AnyAsync(x => x.RouteId == route.Id, cancellationToken);

            if (hasAnyForRoute)
                continue;

            var stopover = RouteStopover.Create(route.Id, stopAirportId, stopOrder: 1, layoverMin: 45);
            await _repository.SaveAsync(stopover, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
