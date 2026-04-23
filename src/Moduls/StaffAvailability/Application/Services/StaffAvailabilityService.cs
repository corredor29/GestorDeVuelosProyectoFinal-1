using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Application.Services;

public sealed class StaffAvailabilityService : IStaffAvailabilityService
{
    private readonly GetStaffAvailabilityUseCase _getUseCase;
    private readonly CreateStaffAvailabilityUseCase _createUseCase;
    private readonly UpdateStaffAvailabilityUseCase _updateUseCase;
    private readonly DeleteStaffAvailabilityUseCase _deleteUseCase;

    public StaffAvailabilityService(
        GetStaffAvailabilityUseCase getUseCase,
        CreateStaffAvailabilityUseCase createUseCase,
        UpdateStaffAvailabilityUseCase updateUseCase,
        DeleteStaffAvailabilityUseCase deleteUseCase)
    {
        _getUseCase = getUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }

    public Task<IEnumerable<StaffAvailabilityRecord>> GetAllAsync() => _getUseCase.ExecuteAllAsync();
    public Task<StaffAvailabilityRecord?> GetByIdAsync(int id) => _getUseCase.ExecuteByIdAsync(id);
    public Task<IEnumerable<StaffAvailabilityRecord>> GetByStaffIdAsync(int staffId) => _getUseCase.ExecuteByStaffIdAsync(staffId);
    public Task<IEnumerable<StaffAvailabilityRecord>> GetByDateRangeAsync(DateTime startsAt, DateTime endsAt) => _getUseCase.ExecuteByDateRangeAsync(startsAt, endsAt);
    public Task<bool> HasBlockingAvailabilityAsync(int staffId, DateTime startsAt, DateTime endsAt) => _getUseCase.ExecuteHasBlockingAvailabilityAsync(staffId, startsAt, endsAt);
    public Task CreateAsync(int staffId, int availabilityStatusId, DateTime startsAt, DateTime endsAt, string? notes) => _createUseCase.ExecuteAsync(staffId, availabilityStatusId, startsAt, endsAt, notes);
    public Task UpdateAsync(int id, int staffId, int availabilityStatusId, DateTime startsAt, DateTime endsAt, string? notes) => _updateUseCase.ExecuteAsync(id, staffId, availabilityStatusId, startsAt, endsAt, notes);
    public Task DeleteByIdAsync(int id) => _deleteUseCase.ExecuteByIdAsync(id);
    public Task<int> DeleteByStaffIdAsync(int staffId) => _deleteUseCase.ExecuteByStaffIdAsync(staffId);
}
