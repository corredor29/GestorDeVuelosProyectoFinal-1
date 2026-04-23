using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Application.UseCases;

public sealed class UpdatePersonalPositionUseCase
{
    private readonly IPersonalPositionsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePersonalPositionUseCase(IPersonalPositionsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, string name)
    {
        var position = await _repository.GetByIdAsync(PersonalPositionsId.Create(id))
            ?? throw new InvalidOperationException($"No se encontró el cargo con ID {id}.");

        var duplicate = await _repository.GetByNameAsync(PersonalPositionsName.Create(name));
        if (duplicate is not null && duplicate.Id?.Value != id)
            throw new InvalidOperationException($"Ya existe otro cargo con nombre '{name}'.");

        position.Update(name);
        await _repository.UpdateAsync(position);
        await _unitOfWork.SaveChangesAsync();
    }
}
