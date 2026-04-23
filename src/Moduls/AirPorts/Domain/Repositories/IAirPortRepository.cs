using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Repositories;

public interface IAirportsRepository
{
    Task<Airport?> GetByIdAsync(AirportsId id);
    Task<Airport?> GetByNameAsync(AirportsName name);
    Task<Airport?> GetByIataCodeAsync(AirportsIataCode iataCode);
    Task<Airport?> GetByIcaoCodeAsync(AirportsIcaoCode icaoCode);
    Task<IEnumerable<Airport>> GetByCityIdAsync(CityId cityId);
    Task<IEnumerable<Airport>> GetByCityNameAsync(string cityName);
    Task<IEnumerable<Airport>> GetAllAsync();
    Task SaveAsync(Airport airport);
    Task UpdateAsync(Airport airport);
    Task DeleteAsync(AirportsId id);
    Task DeleteByNameAsync(AirportsName name);
    Task DeleteByIataCodeAsync(AirportsIataCode iataCode);
    Task DeleteByIcaoCodeAsync(AirportsIcaoCode icaoCode);
}
