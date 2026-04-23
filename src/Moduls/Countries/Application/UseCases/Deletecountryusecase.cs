using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Countries.Application.UseCases;

public sealed class DeleteCountryUseCase
{
    private readonly ICountriesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCountryUseCase(ICountriesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task DeleteByNameAsync(string name)
    {
        var country = await _repository.GetByNameAsync(name)
            ?? throw new InvalidOperationException($"Country '{name}' not found.");

        await _repository.DeleteByNameAsync(country.Name.Value);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteByIsoCodeAsync(string isoCode)
    {
        await _repository.DeleteByIsoCodeAsync(isoCode);
        await _unitOfWork.SaveChangesAsync();
    }
}