using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Countries.Application.UseCases;

public sealed class GetCountriesUseCase
{
    private readonly ICountriesRepository _repository;

    public GetCountriesUseCase(ICountriesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Country>> ExecuteAsync()
        => await _repository.GetAllAsync();

    public async Task<IEnumerable<Country>> GetByContinentAsync(ContinentsId continentId)
        => await _repository.GetByContinentAsync(continentId);

    public async Task<Country?> GetByIsoCodeAsync(CountryIsoCode isoCode)
        => await _repository.GetByIsoCodeAsync(isoCode);

    public async Task<Country?> GetByNameAsync(string name)
        => await _repository.GetByNameAsync(name);
}