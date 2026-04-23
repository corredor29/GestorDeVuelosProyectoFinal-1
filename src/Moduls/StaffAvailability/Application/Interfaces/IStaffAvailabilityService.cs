using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Application.Interfaces;

public interface IStaffAvailabilityService
{
    Task<IEnumerable<StaffAvailabilityRecord>> GetAllAsync();
    Task<StaffAvailabilityRecord?> GetByIdAsync(int id);
    Task<IEnumerable<StaffAvailabilityRecord>> GetByStaffIdAsync(int staffId);
    Task<IEnumerable<StaffAvailabilityRecord>> GetByDateRangeAsync(DateTime startsAt, DateTime endsAt);
    Task<bool> HasBlockingAvailabilityAsync(int staffId, DateTime startsAt, DateTime endsAt);
    Task CreateAsync(int staffId, int availabilityStatusId, DateTime startsAt, DateTime endsAt, string? notes);
    Task UpdateAsync(int id, int staffId, int availabilityStatusId, DateTime startsAt, DateTime endsAt, string? notes);
    Task DeleteByIdAsync(int id);
    Task<int> DeleteByStaffIdAsync(int staffId);
}
