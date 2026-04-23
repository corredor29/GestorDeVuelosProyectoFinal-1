using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.Application.UseCases;

public sealed class UpdateAirlineUseCase
{
    private readonly IAirlinesRepository _repository;
    private readonly ICountriesRepository _countriesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAirlineUseCase(
        IAirlinesRepository repository,
        ICountriesRepository countriesRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _countriesRepository = countriesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, string name, string iataCode, int originCountryId, bool isActive)
    {
        var airline = await _repository.GetByIdAsync(AirlinesId.Create(id))
            ?? throw new InvalidOperationException($"No se encontró la aerolínea con ID {id}.");

        var duplicateByName = await _repository.GetByNameAsync(AirlinesName.Create(name));
        if (duplicateByName is not null && duplicateByName.Id?.Value != id)
            throw new InvalidOperationException($"Ya existe otra aerolínea con nombre '{name}'.");

        var duplicateByIata = await _repository.GetByIataCodeAsync(AirlinesIataCode.Create(iataCode));
        if (duplicateByIata is not null && duplicateByIata.Id?.Value != id)
            throw new InvalidOperationException($"Ya existe otra aerolínea con código IATA '{iataCode.ToUpperInvariant()}'.");

        var country = await _countriesRepository.GetByIdAsync(CountryId.Create(originCountryId));
        if (country is null)
            throw new InvalidOperationException($"No se encontró el país con ID {originCountryId}.");

        airline.Update(name, iataCode, originCountryId, isActive);
        await _repository.UpdateAsync(airline);
        await _unitOfWork.SaveChangesAsync();
    }
}
