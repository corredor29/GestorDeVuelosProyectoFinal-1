using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.Repositories;

public interface ICountriesRepository
{
    Task<Country?> GetByIdAsync(CountryId id);
    Task<Country?> GetByNameAsync(string name);
    Task<Country?> GetByIsoCodeAsync(CountryIsoCode isoCode);
    Task<IEnumerable<Country>> GetByContinentAsync(ContinentsId continentId);
    Task<IEnumerable<Country>> GetAllAsync();
    Task SaveAsync(Country country);
    Task UpdateAsync(Country country);
    Task DeleteAsync(CountryId id);
    Task DeleteByNameAsync(string name);
    Task DeleteByIsoCodeAsync(string isoCode);
}