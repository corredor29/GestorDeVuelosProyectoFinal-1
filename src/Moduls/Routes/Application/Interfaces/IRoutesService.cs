using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.Routes.Application.Interfaces;

public interface IRoutesService
{
    Task<IEnumerable<Route>> GetAllAsync();
    Task<Route?> GetByIdAsync(int id);
    Task<IEnumerable<Route>> GetByOriginAirportIdAsync(int originAirportId);
    Task<IEnumerable<Route>> GetByDestinationAirportIdAsync(int destinationAirportId);
    Task<Route?> GetByOriginAndDestinationAsync(int originAirportId, int destinationAirportId);
    Task CreateAsync(int originAirportId, int destinationAirportId, int? distanceKm, int? estimatedDurationMin);
    Task UpdateAsync(int id, int originAirportId, int destinationAirportId, int? distanceKm, int? estimatedDurationMin);
    Task DeleteByIdAsync(int id);
}
