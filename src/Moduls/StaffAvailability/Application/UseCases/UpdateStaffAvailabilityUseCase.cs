using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Application.UseCases;

public sealed class UpdateStaffAvailabilityUseCase
{
    private readonly IStaffAvailabilityRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStaffAvailabilityUseCase(IStaffAvailabilityRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, int staffId, int availabilityStatusId, DateTime startsAt, DateTime endsAt, string? notes)
    {
        var availability = await _repository.GetByIdAsync(StaffAvailabilityId.Create(id));
        if (availability is null)
            throw new InvalidOperationException($"No se encontro la disponibilidad con ID {id}.");

        availability.Update(staffId, availabilityStatusId, startsAt, endsAt, notes);
        await _repository.UpdateAsync(availability);
        await _unitOfWork.SaveChangesAsync();
    }
}
