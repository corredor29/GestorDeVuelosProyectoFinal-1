using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Application.UseCases;

public sealed class CreateStaffAvailabilityUseCase
{
    private readonly IStaffAvailabilityRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateStaffAvailabilityUseCase(IStaffAvailabilityRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int staffId, int availabilityStatusId, DateTime startsAt, DateTime endsAt, string? notes)
    {
        _ = PersonalId.Create(staffId);
        _ = AvailabilityStatesId.Create(availabilityStatusId);

        var availability = StaffAvailabilityRecord.Create(staffId, availabilityStatusId, startsAt, endsAt, notes);

        await _repository.SaveAsync(availability);
        await _unitOfWork.SaveChangesAsync();
    }
}
