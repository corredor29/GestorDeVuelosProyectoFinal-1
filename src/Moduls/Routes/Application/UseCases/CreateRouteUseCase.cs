using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Routes.Application.UseCases;

public sealed class CreateRouteUseCase
{
    private readonly IRoutesRepository _repository;
    private readonly IAirportsRepository _airportsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRouteUseCase(
        IRoutesRepository repository,
        IAirportsRepository airportsRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _airportsRepository = airportsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int originAirportId, int destinationAirportId, int? distanceKm, int? estimatedDurationMin)
    {
        await EnsureAirportsExistAsync(originAirportId, destinationAirportId);

        var duplicate = await _repository.GetByOriginAndDestinationAsync(
            AirportsId.Create(originAirportId),
            AirportsId.Create(destinationAirportId));

        if (duplicate is not null)
            throw new InvalidOperationException("Ya existe una ruta con el mismo aeropuerto de origen y destino.");

        var route = Route.Create(originAirportId, destinationAirportId, distanceKm, estimatedDurationMin);

        await _repository.SaveAsync(route);
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
