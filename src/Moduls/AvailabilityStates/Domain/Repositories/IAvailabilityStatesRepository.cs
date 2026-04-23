using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.Repositories;

public interface IAvailabilityStatesRepository
{
    Task<AvailabilityState?> GetByIdAsync(AvailabilityStatesId id);
    Task<AvailabilityState?> GetByNameAsync(AvailabilityStatesName name);
    Task<IEnumerable<AvailabilityState>> GetByStaffIdAsync(PersonalId staffId);
    Task<IEnumerable<AvailabilityState>> GetAllAsync();
    Task SaveAsync(AvailabilityState state);
    Task UpdateAsync(AvailabilityState state);
    Task DeleteAsync(AvailabilityStatesId id);
    Task DeleteByNameAsync(AvailabilityStatesName name);
}
