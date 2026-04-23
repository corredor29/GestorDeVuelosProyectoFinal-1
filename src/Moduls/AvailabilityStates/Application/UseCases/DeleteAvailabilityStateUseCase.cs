using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Application.UseCases;

public sealed class DeleteAvailabilityStateUseCase
{
    private readonly IAvailabilityStatesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAvailabilityStateUseCase(IAvailabilityStatesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(AvailabilityStatesId.Create(id));
        if (existing is null)
            throw new InvalidOperationException($"No se encontró el estado de disponibilidad con ID {id}.");

        await _repository.DeleteAsync(AvailabilityStatesId.Create(id));
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ExecuteByNameAsync(string name)
    {
        var existing = await _repository.GetByNameAsync(AvailabilityStatesName.Create(name));
        if (existing is null)
            throw new InvalidOperationException($"No se encontró el estado de disponibilidad con nombre '{name}'.");

        await _repository.DeleteByNameAsync(AvailabilityStatesName.Create(name));
        await _unitOfWork.SaveChangesAsync();
    }
}
