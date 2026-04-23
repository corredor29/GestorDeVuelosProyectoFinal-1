using GestorDeVuelosProyectoFinal.Moduls.Cities.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Cities.Application.Services;

public sealed class CityService : ICityService
{
    // Aquí la UI conversa con casos de uso y repositorio sin conocer detalles de EF.
    private readonly ICityRepository _repository;
    private readonly CreateCityUseCase _createUseCase;
    private readonly UpdateCityUseCase _updateUseCase;
    private readonly DeleteCityUseCase _deleteUseCase;

    public CityService(
        ICityRepository repository,
        CreateCityUseCase createUseCase,
        UpdateCityUseCase updateUseCase,
        DeleteCityUseCase deleteUseCase)
    {
        _repository = repository;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }

    public async Task<IEnumerable<City>> GetAllAsync() => await _repository.GetAllAsync();
    // El repositorio conserva el nombre histórico del método, pero aquí ya se expone como región.
    public async Task<IEnumerable<City>> GetByRegionIdAsync(int regionId) => await _repository.GetByCountryAsync(RegionId.Create(regionId));
    public async Task<City?> GetByNameAsync(string name) => await _repository.GetByNameAsync(name);
    public async Task<City?> GetByIdAsync(int id) => await _repository.GetByIdAsync(CityId.Create(id));
    public async Task CreateAsync(string name, int regionId) => await _createUseCase.ExecuteAsync(name, regionId);
    public async Task UpdateAsync(int id, string newName, int newRegionId) => await _updateUseCase.ExecuteAsync(id, newName, newRegionId);
    public async Task DeleteAsync(int id) => await _deleteUseCase.ExecuteAsync(id);
    public async Task DeleteByNameAsync(string name) => await _deleteUseCase.ExecuteByNameAsync(name);
    public async Task DeleteByRegionIdAsync(int regionId) => await _deleteUseCase.ExecuteByRegionAsync(regionId);
}
