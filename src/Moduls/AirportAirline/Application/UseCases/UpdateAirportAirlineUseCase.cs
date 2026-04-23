using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Application.UseCases;

public sealed class UpdateAirportAirlineUseCase
{
    private readonly IAirportAirlineRepository _repository;
    private readonly IAirportsRepository _airportsRepository;
    private readonly IAirlinesRepository _airlinesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAirportAirlineUseCase(
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

    public async Task ExecuteAsync(int id, int airportId, int airlineId, string? terminal, DateTime startDate, DateTime? endDate, bool isActive)
    {
        var operation = await _repository.GetByIdAsync(AirportAirlineId.Create(id))
            ?? throw new InvalidOperationException($"No se encontró la operación con ID {id}.");

        var airport = await _airportsRepository.GetByIdAsync(AirportsId.Create(airportId));
        if (airport is null)
            throw new InvalidOperationException($"No se encontró el aeropuerto con ID {airportId}.");

        var airline = await _airlinesRepository.GetByIdAsync(AirlinesId.Create(airlineId));
        if (airline is null)
            throw new InvalidOperationException($"No se encontró la aerolínea con ID {airlineId}.");

        var duplicate = await _repository.GetByAirportAndAirlineAsync(AirportsId.Create(airportId), AirlinesId.Create(airlineId));
        if (duplicate is not null && duplicate.Id?.Value != id)
            throw new InvalidOperationException("Ya existe otra operación registrada para esa combinación de aeropuerto y aerolínea.");

        operation.Update(airportId, airlineId, terminal, startDate, endDate, isActive);
        await _repository.UpdateAsync(operation);
        await _unitOfWork.SaveChangesAsync();
    }
}
