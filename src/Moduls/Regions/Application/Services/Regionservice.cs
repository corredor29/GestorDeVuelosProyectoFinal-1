using GestorDeVuelosProyectoFinal.Moduls.Regions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.Regions.Application.Services;

public sealed class RegionService : IRegionService
{
    // La capa de aplicación mantiene el contrato simple para la UI del admin.
    private readonly GetRegionsUseCase _getAll;
    private readonly CreateRegionUseCase _create;
    private readonly UpdateRegionUseCase _update;
    private readonly DeleteRegionUseCase _delete;

    public RegionService(
        GetRegionsUseCase getAll,
        CreateRegionUseCase create,
        UpdateRegionUseCase update,
        DeleteRegionUseCase delete)
    {
        _getAll = getAll;
        _create = create;
        _update = update;
        _delete = delete;
    }

    public Task<IEnumerable<Region>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IEnumerable<Region>> GetByCountryIdAsync(int countryId)
        => _getAll.GetByCountryAsync(countryId);

    // El tipo también funciona como filtro útil porque varias regiones comparten la misma clasificación.
    public Task<IEnumerable<Region>> GetByTypeAsync(string type)
        => _getAll.GetByTypeAsync(type);

    public Task<Region?> GetByNameAsync(string name)
        => _getAll.GetByNameAsync(name);

    public Task CreateAsync(string name, string type, int countryId)
        => _create.ExecuteAsync(name, type, countryId);

    public Task UpdateAsync(int id, string newName, string newType, int newCountryId)
        => _update.ExecuteAsync(id, newName, newType, newCountryId);

    public Task DeleteByNameAsync(string name)
        => _delete.DeleteByNameAsync(name);

    public Task DeleteByTypeAsync(string type)
        => _delete.DeleteByTypeAsync(type);
}
