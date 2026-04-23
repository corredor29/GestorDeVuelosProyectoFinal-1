using System.Collections.Generic;
using System.Threading.Tasks;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.Repositories;

public interface IRoutesRepository
{
    Task<Route?> GetByIdAsync(RouteId id);
    Task<IEnumerable<Route>> GetByOriginAirportIdAsync(AirportsId originAirportId);
    Task<IEnumerable<Route>> GetByDestinationAirportIdAsync(AirportsId destinationAirportId);
    Task<Route?> GetByOriginAndDestinationAsync(AirportsId origin, AirportsId destination);
    Task<IEnumerable<Route>> GetAllAsync();
    Task SaveAsync(Route route);
    Task UpdateAsync(Route route);
    Task DeleteByIdAsync(RouteId id);
}
