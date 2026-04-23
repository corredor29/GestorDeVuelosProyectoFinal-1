using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.Repositories;

public interface IRegionsRepository
{
    Task<Region?> GetByIdAsync(RegionId id);
    Task<Region?> GetByNameAsync(string name);
    Task<IEnumerable<Region>> GetByTypeAsync(string type);
    Task<IEnumerable<Region>> GetAllAsync();
    Task<IEnumerable<Region>> GetByCountryAsync(CountryId countryId);
    Task SaveAsync(Region region);
    Task UpdateAsync(Region region);
    Task DeleteAsync(RegionId id);
    Task DeleteByNameAsync(string name);
    Task DeleteByTypeAsync(string type);
}