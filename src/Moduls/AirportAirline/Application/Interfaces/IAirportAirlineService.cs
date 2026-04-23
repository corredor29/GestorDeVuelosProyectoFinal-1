using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Application.Interfaces;

public interface IAirportAirlineService
{
    Task<IEnumerable<AirportAirlineOperation>> GetAllAsync();
    Task<IEnumerable<AirportAirlineOperation>> GetActiveAsync();
    Task<AirportAirlineOperation?> GetByIdAsync(int id);
    Task<IEnumerable<AirportAirlineOperation>> GetByTerminalAsync(string terminal);
    Task<IEnumerable<AirportAirlineOperation>> GetByAirportIdAsync(int airportId);
    Task<IEnumerable<AirportAirlineOperation>> GetByAirlineIdAsync(int airlineId);
    Task<IEnumerable<AirportAirlineOperation>> GetByStartDateAsync(DateTime startDate);
    Task<IEnumerable<AirportAirlineOperation>> GetByEndDateAsync(DateTime endDate);
    Task CreateAsync(int airportId, int airlineId, string? terminal, DateTime startDate, DateTime? endDate, bool isActive);
    Task UpdateAsync(int id, int airportId, int airlineId, string? terminal, DateTime startDate, DateTime? endDate, bool isActive);
    Task DeactivateByIdAsync(int id);
    Task<int> DeactivateByTerminalAsync(string terminal);
    Task<int> DeactivateByAirportIdAsync(int airportId);
    Task<int> DeactivateByAirlineIdAsync(int airlineId);
    Task<int> DeactivateByStartDateAsync(DateTime startDate);
    Task<int> DeactivateByEndDateAsync(DateTime endDate);
    Task ReactivateAsync(int id);
}
