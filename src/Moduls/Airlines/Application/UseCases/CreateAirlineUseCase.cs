using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.Application.UseCases;

public sealed class CreateAirlineUseCase
{
    private readonly IAirlinesRepository _repository;
    private readonly ICountriesRepository _countriesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAirlineUseCase(
        IAirlinesRepository repository,
        ICountriesRepository countriesRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _countriesRepository = countriesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string name, string iataCode, int originCountryId, bool isActive)
    {
        var existingByName = await _repository.GetByNameAsync(AirlinesName.Create(name));
        if (existingByName is not null)
            throw new InvalidOperationException($"Ya existe una aerolínea con nombre '{name}'.");

        var existingByIata = await _repository.GetByIataCodeAsync(AirlinesIataCode.Create(iataCode));
        if (existingByIata is not null)
            throw new InvalidOperationException($"Ya existe una aerolínea con código IATA '{iataCode.ToUpperInvariant()}'.");

        var country = await _countriesRepository.GetByIdAsync(CountryId.Create(originCountryId));
        if (country is null)
            throw new InvalidOperationException($"No se encontró el país con ID {originCountryId}.");

        var airline = Airline.Create(name, iataCode, originCountryId, isActive);
        await _repository.SaveAsync(airline);
        await _unitOfWork.SaveChangesAsync();
    }
}
