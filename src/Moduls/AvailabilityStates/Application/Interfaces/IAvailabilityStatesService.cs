using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Application.Interfaces;

public interface IAvailabilityStatesService
{
    Task<IEnumerable<AvailabilityState>> GetAllAsync();
    Task<AvailabilityState?> GetByIdAsync(int id);
    Task<AvailabilityState?> GetByNameAsync(string name);
    Task<IEnumerable<AvailabilityState>> GetByStaffIdAsync(int staffId);
    Task CreateAsync(string name);
    Task UpdateAsync(int id, string name);
    Task DeleteByIdAsync(int id);
    Task DeleteByNameAsync(string name);
}
