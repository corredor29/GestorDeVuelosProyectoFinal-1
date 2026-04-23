using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Countries.Application.UseCases;

public sealed class UpdateCountryUseCase
{
    private readonly ICountriesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCountryUseCase(ICountriesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string currentIsoCode, string newName, string newIsoCode, int newContinentId)
    {
        var country = await _repository.GetByIsoCodeAsync(CountryIsoCode.Create(currentIsoCode))
            ?? throw new InvalidOperationException($"Country with ISO code '{currentIsoCode}' not found.");

        // Check new ISO code is not taken by another country
        if (!string.Equals(currentIsoCode, newIsoCode, StringComparison.OrdinalIgnoreCase))
        {
            var duplicate = await _repository.GetByIsoCodeAsync(CountryIsoCode.Create(newIsoCode));
            if (duplicate is not null)
                throw new InvalidOperationException($"A country with ISO code '{newIsoCode}' already exists.");
        }

        country.UpdateName(newName);
        country.UpdateIsoCode(newIsoCode);
        if (newContinentId > 0)
            country.UpdateContinent(newContinentId);

        await _repository.UpdateAsync(country);
        await _unitOfWork.SaveChangesAsync();
    }
}