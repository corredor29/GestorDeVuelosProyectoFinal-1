using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Application.UseCases;

public sealed class GetStaffAvailabilityUseCase
{
    private readonly IStaffAvailabilityRepository _repository;

    public GetStaffAvailabilityUseCase(IStaffAvailabilityRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<StaffAvailabilityRecord>> ExecuteAllAsync() => _repository.GetAllAsync();
    public Task<StaffAvailabilityRecord?> ExecuteByIdAsync(int id) => _repository.GetByIdAsync(StaffAvailabilityId.Create(id));
    public Task<IEnumerable<StaffAvailabilityRecord>> ExecuteByStaffIdAsync(int staffId) => _repository.GetByStaffIdAsync(PersonalId.Create(staffId));
    public Task<IEnumerable<StaffAvailabilityRecord>> ExecuteByDateRangeAsync(DateTime startsAt, DateTime endsAt) => _repository.GetByDateRangeAsync(startsAt, endsAt);
    public Task<bool> ExecuteHasBlockingAvailabilityAsync(int staffId, DateTime startsAt, DateTime endsAt) => _repository.HasBlockingAvailabilityAsync(PersonalId.Create(staffId), startsAt, endsAt);
}
