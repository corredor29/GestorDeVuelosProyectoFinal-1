using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Application.UseCases;

public sealed class DeleteStaffAvailabilityUseCase
{
    private readonly IStaffAvailabilityRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteStaffAvailabilityUseCase(IStaffAvailabilityRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(StaffAvailabilityId.Create(id));
        if (existing is null)
            throw new InvalidOperationException($"No se encontro la disponibilidad con ID {id}.");

        await _repository.DeleteByIdAsync(StaffAvailabilityId.Create(id));
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<int> ExecuteByStaffIdAsync(int staffId)
    {
        var affected = await _repository.DeleteByStaffIdAsync(PersonalId.Create(staffId));
        if (affected == 0)
            throw new InvalidOperationException($"No se encontraron disponibilidades para el personal con ID {staffId}.");

        await _unitOfWork.SaveChangesAsync();
        return affected;
    }
}
