using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Cities.Application.UseCases;

public sealed class GetCityUseCase
{
    private readonly ICityRepository _repository;

    public GetCityUseCase(ICityRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<City>> ExecuteAsync()
        => await _repository.GetAllAsync();

    public async Task<IEnumerable<City>> GetByRegionAsync(int regionId)
        => await _repository.GetByCountryAsync(RegionId.Create(regionId));

    public async Task<City?> GetByNameAsync(string name)
        => await _repository.GetByNameAsync(name);

    public async Task<City?> GetByIdAsync(int id)
        => await _repository.GetByIdAsync(CityId.Create(id));
}