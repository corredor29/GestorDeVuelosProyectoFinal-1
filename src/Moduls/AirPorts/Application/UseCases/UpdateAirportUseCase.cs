using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Airports.Application.UseCases;

public sealed class UpdateAirportUseCase
{
    private readonly IAirportsRepository _repository;
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAirportUseCase(
        IAirportsRepository repository,
        ICityRepository cityRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, string name, string iataCode, string? icaoCode, int cityId)
    {
        var airport = await _repository.GetByIdAsync(AirportsId.Create(id))
            ?? throw new InvalidOperationException($"No se encontró el aeropuerto con ID {id}.");

        var city = await _cityRepository.GetByIdAsync(CityId.Create(cityId));
        if (city is null)
            throw new InvalidOperationException($"No se encontró la ciudad con ID {cityId}.");

        var duplicateByName = await _repository.GetByNameAsync(AirportsName.Create(name));
        if (duplicateByName is not null && duplicateByName.Id?.Value != id)
            throw new InvalidOperationException($"Ya existe otro aeropuerto con nombre '{name}'.");

        var duplicateByIata = await _repository.GetByIataCodeAsync(AirportsIataCode.Create(iataCode));
        if (duplicateByIata is not null && duplicateByIata.Id?.Value != id)
            throw new InvalidOperationException($"Ya existe otro aeropuerto con código IATA '{iataCode.ToUpperInvariant()}'.");

        if (!string.IsNullOrWhiteSpace(icaoCode))
        {
            var duplicateByIcao = await _repository.GetByIcaoCodeAsync(AirportsIcaoCode.Create(icaoCode));
            if (duplicateByIcao is not null && duplicateByIcao.Id?.Value != id)
                throw new InvalidOperationException($"Ya existe otro aeropuerto con código ICAO '{icaoCode.Trim().ToUpperInvariant()}'.");
        }

        airport.Update(name, iataCode, icaoCode, cityId);
        await _repository.UpdateAsync(airport);
        await _unitOfWork.SaveChangesAsync();
    }
}
