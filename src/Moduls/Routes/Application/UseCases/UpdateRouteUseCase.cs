using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Routes.Application.UseCases;

public sealed class UpdateRouteUseCase
{
    private readonly IRoutesRepository _repository;
    private readonly IAirportsRepository _airportsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRouteUseCase(
        IRoutesRepository repository,
        IAirportsRepository airportsRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _airportsRepository = airportsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, int originAirportId, int destinationAirportId, int? distanceKm, int? estimatedDurationMin)
    {
        var route = await _repository.GetByIdAsync(RouteId.Create(id));
        if (route is null)
            throw new InvalidOperationException($"No se encontro la ruta con ID {id}.");

        await EnsureAirportsExistAsync(originAirportId, destinationAirportId);

        var duplicate = await _repository.GetByOriginAndDestinationAsync(
            AirportsId.Create(originAirportId),
            AirportsId.Create(destinationAirportId));

        if (duplicate is not null && duplicate.Id?.Value != id)
            throw new InvalidOperationException("Ya existe otra ruta con el mismo aeropuerto de origen y destino.");

        route.Update(originAirportId, destinationAirportId, distanceKm, estimatedDurationMin);
        await _repository.UpdateAsync(route);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task EnsureAirportsExistAsync(int originAirportId, int destinationAirportId)
    {
        var origin = await _airportsRepository.GetByIdAsync(AirportsId.Create(originAirportId));
        if (origin is null)
            throw new InvalidOperationException($"No se encontro el aeropuerto origen con ID {originAirportId}.");

        var destination = await _airportsRepository.GetByIdAsync(AirportsId.Create(destinationAirportId));
        if (destination is null)
            throw new InvalidOperationException($"No se encontro el aeropuerto destino con ID {destinationAirportId}.");
    }
}
