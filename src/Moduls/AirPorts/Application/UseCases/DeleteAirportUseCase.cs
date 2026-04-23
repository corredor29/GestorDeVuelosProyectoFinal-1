using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Airports.Application.UseCases;

public sealed class DeleteAirportUseCase
{
    private readonly IAirportsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAirportUseCase(IAirportsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(AirportsId.Create(id));
        if (existing is null)
            throw new InvalidOperationException($"No se encontró el aeropuerto con ID {id}.");

        await _repository.DeleteAsync(AirportsId.Create(id));
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ExecuteByNameAsync(string name)
    {
        var airport = await _repository.GetByNameAsync(AirportsName.Create(name));
        if (airport is null)
            throw new InvalidOperationException($"No se encontró el aeropuerto con nombre '{name}'.");

        await _repository.DeleteByNameAsync(AirportsName.Create(name));
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ExecuteByIataCodeAsync(string iataCode)
    {
        var airport = await _repository.GetByIataCodeAsync(AirportsIataCode.Create(iataCode));
        if (airport is null)
            throw new InvalidOperationException($"No se encontró el aeropuerto con código IATA '{iataCode.ToUpperInvariant()}'.");

        await _repository.DeleteByIataCodeAsync(AirportsIataCode.Create(iataCode));
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ExecuteByIcaoCodeAsync(string icaoCode)
    {
        var code = AirportsIcaoCode.Create(icaoCode);
        var airport = await _repository.GetByIcaoCodeAsync(code);
        if (airport is null)
            throw new InvalidOperationException($"No se encontró el aeropuerto con código ICAO '{icaoCode.Trim().ToUpperInvariant()}'.");

        await _repository.DeleteByIcaoCodeAsync(code);
        await _unitOfWork.SaveChangesAsync();
    }
}
