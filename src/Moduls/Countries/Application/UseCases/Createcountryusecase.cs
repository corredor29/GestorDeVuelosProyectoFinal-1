using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Countries.Application.UseCases;

public sealed class CreateCountryUseCase
{
    private readonly ICountriesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCountryUseCase(ICountriesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string name, string isoCode, int continentId)
    {
        var existingByIso = await _repository.GetByIsoCodeAsync(CountryIsoCode.Create(isoCode));
        if (existingByIso is not null)
            throw new InvalidOperationException($"A country with ISO code '{isoCode}' already exists.");

        var existingByName = await _repository.GetByNameAsync(name);
        if (existingByName is not null)
            throw new InvalidOperationException($"A country named '{name}' already exists.");
        var country = Country.Create(0, name, isoCode, continentId);

        await _repository.SaveAsync(country);
        await _unitOfWork.SaveChangesAsync();
    }
}