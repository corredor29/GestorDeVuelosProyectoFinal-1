using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.Countries.Application.Interfaces;

public interface ICountryService
{
    Task<IEnumerable<Country>> GetAllAsync();
    Task<IEnumerable<Country>> GetByContinentIdAsync(int continentId);
    /// <summary>Resuelve el continente por nombre y devuelve los países asociados.</summary>
    Task<IEnumerable<Country>> GetByContinentNameAsync(string continentName);
    Task<Country?> GetByIsoCodeAsync(string isoCode);
    Task<Country?> GetByNameAsync(string name);
    Task CreateAsync(string name, string isoCode, int continentId);
    Task UpdateAsync(string currentIsoCode, string newName, string newIsoCode, int newContinentId);
    Task DeleteByNameAsync(string name);
    Task DeleteByIsoCodeAsync(string isoCode);
}