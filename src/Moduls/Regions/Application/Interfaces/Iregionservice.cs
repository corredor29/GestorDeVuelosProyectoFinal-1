using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.Regions.Application.Interfaces;

public interface IRegionService
{
    Task<IEnumerable<Region>> GetAllAsync();
    Task<IEnumerable<Region>> GetByCountryIdAsync(int countryId);
    Task<IEnumerable<Region>> GetByTypeAsync(string type);
    Task<Region?> GetByNameAsync(string name);
    Task CreateAsync(string name, string type, int countryId);
    Task UpdateAsync(int id, string newName, string newType, int newCountryId);
    Task DeleteByNameAsync(string name);
    Task DeleteByTypeAsync(string type);
}