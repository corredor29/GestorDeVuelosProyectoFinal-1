using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Regions.Application.UseCases;

public sealed class GetRegionsUseCase
{
    private readonly IRegionsRepository _repository;

    public GetRegionsUseCase(IRegionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Region>> ExecuteAsync()
        => await _repository.GetAllAsync();

    public async Task<IEnumerable<Region>> GetByCountryAsync(int countryId)
        => await _repository.GetByCountryAsync(CountryId.Create(countryId));

    public async Task<IEnumerable<Region>> GetByTypeAsync(string type)
        => await _repository.GetByTypeAsync(type);

    public async Task<Region?> GetByNameAsync(string name)
        => await _repository.GetByNameAsync(name);
}