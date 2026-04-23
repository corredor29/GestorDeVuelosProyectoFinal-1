using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Application.UseCases;

public sealed class DeleteAirportAirlineUseCase
{
    private readonly IAirportAirlineRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAirportAirlineUseCase(IAirportAirlineRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(AirportAirlineId.Create(id));
        if (existing is null)
            throw new InvalidOperationException($"No se encontró la operación con ID {id}.");

        await _repository.DeleteAsync(AirportAirlineId.Create(id));
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<int> ExecuteByTerminalAsync(string terminal)
    {
        var affected = await _repository.DeleteByTerminalAsync(AirportAirlineTerminal.Create(terminal));
        if (affected == 0)
            throw new InvalidOperationException("No se encontraron operaciones con esa terminal.");

        await _unitOfWork.SaveChangesAsync();
        return affected;
    }

    public async Task<int> ExecuteByAirportIdAsync(int airportId)
    {
        var affected = await _repository.DeleteByAirportIdAsync(AirportsId.Create(airportId));
        if (affected == 0)
            throw new InvalidOperationException($"No se encontraron operaciones para el aeropuerto con ID {airportId}.");

        await _unitOfWork.SaveChangesAsync();
        return affected;
    }

    public async Task<int> ExecuteByAirlineIdAsync(int airlineId)
    {
        var affected = await _repository.DeleteByAirlineIdAsync(AirlinesId.Create(airlineId));
        if (affected == 0)
            throw new InvalidOperationException($"No se encontraron operaciones para la aerolínea con ID {airlineId}.");

        await _unitOfWork.SaveChangesAsync();
        return affected;
    }

    public async Task<int> ExecuteByStartDateAsync(DateTime startDate)
    {
        var affected = await _repository.DeleteByStartDateAsync(AirportAirlineStartDate.Create(startDate));
        if (affected == 0)
            throw new InvalidOperationException("No se encontraron operaciones con esa fecha de inicio.");

        await _unitOfWork.SaveChangesAsync();
        return affected;
    }

    public async Task<int> ExecuteByEndDateAsync(DateTime endDate)
    {
        var affected = await _repository.DeleteByEndDateAsync(AirportAirlineEndDate.Create(endDate));
        if (affected == 0)
            throw new InvalidOperationException("No se encontraron operaciones con esa fecha de fin.");

        await _unitOfWork.SaveChangesAsync();
        return affected;
    }
}
