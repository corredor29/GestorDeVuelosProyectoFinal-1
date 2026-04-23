using CabinConfigurationAggregate = GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.Aggregate.CabinConfiguration;

namespace GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Application.Interfaces;

public interface ICabinConfigurationService
{
    Task<IEnumerable<CabinConfigurationAggregate>> GetAllAsync();
    Task<CabinConfigurationAggregate?> GetByIdAsync(int id);
    Task<IEnumerable<CabinConfigurationAggregate>> GetByAircraftIdAsync(int aircraftId);
    Task CreateAsync(int aircraftId, int cabinTypeId, int rowStart, int rowEnd, int seatsPerRow, string seatLetters);
    Task UpdateAsync(int id, int aircraftId, int cabinTypeId, int rowStart, int rowEnd, int seatsPerRow, string seatLetters);
    Task DeleteAsync(int id);
}
