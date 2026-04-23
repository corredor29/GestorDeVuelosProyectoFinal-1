using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.Repositories;

public interface IAirportAirlineRepository
{
    Task<AirportAirlineOperation?> GetByIdAsync(AirportAirlineId id);
    Task<IEnumerable<AirportAirlineOperation>> GetAllAsync();
    Task<IEnumerable<AirportAirlineOperation>> GetActiveAsync();
    Task<IEnumerable<AirportAirlineOperation>> GetByTerminalAsync(AirportAirlineTerminal terminal);
    Task<IEnumerable<AirportAirlineOperation>> GetByAirportIdAsync(AirportsId airportId);
    Task<IEnumerable<AirportAirlineOperation>> GetByAirlineIdAsync(AirlinesId airlineId);
    Task<IEnumerable<AirportAirlineOperation>> GetByStartDateAsync(AirportAirlineStartDate startDate);
    Task<IEnumerable<AirportAirlineOperation>> GetByEndDateAsync(AirportAirlineEndDate endDate);
    Task<AirportAirlineOperation?> GetByAirportAndAirlineAsync(AirportsId airportId, AirlinesId airlineId);
    Task SaveAsync(AirportAirlineOperation airportAirline);
    Task UpdateAsync(AirportAirlineOperation airportAirline);
    Task DeleteAsync(AirportAirlineId id);
    Task<int> DeleteByTerminalAsync(AirportAirlineTerminal terminal);
    Task<int> DeleteByAirportIdAsync(AirportsId airportId);
    Task<int> DeleteByAirlineIdAsync(AirlinesId airlineId);
    Task<int> DeleteByStartDateAsync(AirportAirlineStartDate startDate);
    Task<int> DeleteByEndDateAsync(AirportAirlineEndDate endDate);
    Task ReactivateAsync(AirportAirlineId id);
}
