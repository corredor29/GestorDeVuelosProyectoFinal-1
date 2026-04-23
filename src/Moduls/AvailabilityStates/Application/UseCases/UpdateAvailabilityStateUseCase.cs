using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Application.UseCases;

public sealed class UpdateAvailabilityStateUseCase
{
    private readonly IAvailabilityStatesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAvailabilityStateUseCase(IAvailabilityStatesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, string name)
    {
        var state = await _repository.GetByIdAsync(AvailabilityStatesId.Create(id))
            ?? throw new InvalidOperationException($"No se encontró el estado de disponibilidad con ID {id}.");

        var duplicate = await _repository.GetByNameAsync(AvailabilityStatesName.Create(name));
        if (duplicate is not null && duplicate.Id?.Value != id)
            throw new InvalidOperationException($"Ya existe otro estado de disponibilidad con nombre '{name}'.");

        state.Update(name);
        await _repository.UpdateAsync(state);
        await _unitOfWork.SaveChangesAsync();
    }
}
