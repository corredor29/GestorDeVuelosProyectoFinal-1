using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.Cities.Application.Interfaces;

public interface ICityService
{
    Task<IEnumerable<City>> GetAllAsync();
    Task<IEnumerable<City>> GetByRegionIdAsync(int regionId);
    Task<City?> GetByNameAsync(string name);
    Task<City?> GetByIdAsync(int id);
    Task CreateAsync(string name, int regionId);
    Task UpdateAsync(int id, string newName, int newRegionId);
    Task DeleteAsync(int id);
    Task DeleteByNameAsync(string name);
    Task DeleteByRegionIdAsync(int regionId);
}