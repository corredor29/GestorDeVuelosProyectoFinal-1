using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.Airports.Application.Interfaces;

public interface IAirportsService
{
    Task<IEnumerable<Airport>> GetAllAsync();
    Task<Airport?> GetByIdAsync(int id);
    Task<Airport?> GetByNameAsync(string name);
    Task<Airport?> GetByIataCodeAsync(string iataCode);
    Task<Airport?> GetByIcaoCodeAsync(string? icaoCode);
    Task<IEnumerable<Airport>> GetByCityIdAsync(int cityId);
    Task<IEnumerable<Airport>> GetByCityNameAsync(string cityName);
    Task CreateAsync(string name, string iataCode, string? icaoCode, int cityId);
    Task UpdateAsync(int id, string name, string iataCode, string? icaoCode, int cityId);
    Task DeleteByIdAsync(int id);
    Task DeleteByNameAsync(string name);
    Task DeleteByIataCodeAsync(string iataCode);
    Task DeleteByIcaoCodeAsync(string icaoCode);
}
