using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Application.UseCases;

public sealed class CreatePersonalPositionUseCase
{
    private readonly IPersonalPositionsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePersonalPositionUseCase(IPersonalPositionsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string name)
    {
        var duplicate = await _repository.GetByNameAsync(PersonalPositionsName.Create(name));
        if (duplicate is not null)
            throw new InvalidOperationException($"Ya existe un cargo con nombre '{name}'.");

        var position = PersonalPosition.Create(name);
        await _repository.SaveAsync(position);
        await _unitOfWork.SaveChangesAsync();
    }
}
