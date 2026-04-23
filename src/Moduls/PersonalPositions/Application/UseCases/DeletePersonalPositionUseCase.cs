using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Application.UseCases;

public sealed class DeletePersonalPositionUseCase
{
    private readonly IPersonalPositionsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePersonalPositionUseCase(IPersonalPositionsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(PersonalPositionsId.Create(id));
        if (existing is null)
            throw new InvalidOperationException($"No se encontró el cargo con ID {id}.");

        await _repository.DeleteAsync(PersonalPositionsId.Create(id));
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ExecuteByNameAsync(string name)
    {
        var existing = await _repository.GetByNameAsync(PersonalPositionsName.Create(name));
        if (existing is null)
            throw new InvalidOperationException($"No se encontró el cargo con nombre '{name}'.");

        await _repository.DeleteByNameAsync(PersonalPositionsName.Create(name));
        await _unitOfWork.SaveChangesAsync();
    }
}
