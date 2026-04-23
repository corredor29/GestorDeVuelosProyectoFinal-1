using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Rates.Application.UseCases;

public sealed class CreateRateUseCase
{
    private readonly IRatesRepository _repository;
    private readonly IRoutesRepository _routesRepository;
    private readonly ICabinTypesRepository _cabinTypesRepository;
    private readonly IPassengerTypesRepository _passengerTypesRepository;
    private readonly ISeasonsRepository _seasonsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRateUseCase(
        IRatesRepository repository,
        IRoutesRepository routesRepository,
        ICabinTypesRepository cabinTypesRepository,
        IPassengerTypesRepository passengerTypesRepository,
        ISeasonsRepository seasonsRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _routesRepository = routesRepository;
        _cabinTypesRepository = cabinTypesRepository;
        _passengerTypesRepository = passengerTypesRepository;
        _seasonsRepository = seasonsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int routeId, int cabinTypeId, int passengerTypeId, int seasonId, decimal basePrice, DateOnly? validFrom, DateOnly? validUntil)
    {
        await EnsureReferencesExistAsync(routeId, cabinTypeId, passengerTypeId, seasonId);

        var rate = Rate.Create(routeId, cabinTypeId, passengerTypeId, seasonId, basePrice, validFrom, validUntil);
        await _repository.SaveAsync(rate);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task EnsureReferencesExistAsync(int routeId, int cabinTypeId, int passengerTypeId, int seasonId)
    {
        if (await _routesRepository.GetByIdAsync(RouteId.Create(routeId)) is null)
            throw new InvalidOperationException($"No se encontro la ruta con ID {routeId}.");

        if (await _cabinTypesRepository.GetByIdAsync(CabinTypesId.Create(cabinTypeId)) is null)
            throw new InvalidOperationException($"No se encontro el tipo de cabina con ID {cabinTypeId}.");

        if (await _passengerTypesRepository.GetByIdAsync(PassengerTypesId.Create(passengerTypeId)) is null)
            throw new InvalidOperationException($"No se encontro el tipo de pasajero con ID {passengerTypeId}.");

        if (await _seasonsRepository.GetByIdAsync(SeasonsId.Create(seasonId)) is null)
            throw new InvalidOperationException($"No se encontro la temporada con ID {seasonId}.");
    }
}
