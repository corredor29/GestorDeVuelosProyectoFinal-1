using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Application.UseCases;

public sealed class CreateAvailabilityStateUseCase
{
    private readonly IAvailabilityStatesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAvailabilityStateUseCase(IAvailabilityStatesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string name)
    {
        var duplicate = await _repository.GetByNameAsync(AvailabilityStatesName.Create(name));
        if (duplicate is not null)
            throw new InvalidOperationException($"Ya existe un estado de disponibilidad con nombre '{name}'.");

        var state = AvailabilityState.Create(name);
        await _repository.SaveAsync(state);
        await _unitOfWork.SaveChangesAsync();
    }
}
