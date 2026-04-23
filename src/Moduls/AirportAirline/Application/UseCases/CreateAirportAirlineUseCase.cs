using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Application.UseCases;

public sealed class CreateAirportAirlineUseCase
{
    private readonly IAirportAirlineRepository _repository;
    private readonly IAirportsRepository _airportsRepository;
    private readonly IAirlinesRepository _airlinesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAirportAirlineUseCase(
        IAirportAirlineRepository repository,
        IAirportsRepository airportsRepository,
        IAirlinesRepository airlinesRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _airportsRepository = airportsRepository;
        _airlinesRepository = airlinesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int airportId, int airlineId, string? terminal, DateTime startDate, DateTime? endDate, bool isActive)
    {
        var airport = await _airportsRepository.GetByIdAsync(AirportsId.Create(airportId));
        if (airport is null)
            throw new InvalidOperationException($"No se encontró el aeropuerto con ID {airportId}.");

        var airline = await _airlinesRepository.GetByIdAsync(AirlinesId.Create(airlineId));
        if (airline is null)
            throw new InvalidOperationException($"No se encontró la aerolínea con ID {airlineId}.");

        var duplicate = await _repository.GetByAirportAndAirlineAsync(AirportsId.Create(airportId), AirlinesId.Create(airlineId));
        if (duplicate is not null)
            throw new InvalidOperationException("Ya existe una operación registrada para esa combinación de aeropuerto y aerolínea.");

        var operation = AirportAirlineOperation.Create(airportId, airlineId, terminal, startDate, endDate, isActive);
        await _repository.SaveAsync(operation);
        await _unitOfWork.SaveChangesAsync();
    }
}
