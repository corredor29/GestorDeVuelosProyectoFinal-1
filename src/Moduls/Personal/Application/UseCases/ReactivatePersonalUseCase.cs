using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Personal.Application.UseCases;

public sealed class ReactivatePersonalUseCase
{
    private readonly IPersonalRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ReactivatePersonalUseCase(IPersonalRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(PersonalId.Create(id));
        if (existing is null)
            throw new InvalidOperationException($"No se encontró el empleado con ID {id}.");

        await _repository.ReactivateAsync(PersonalId.Create(id));
        await _unitOfWork.SaveChangesAsync();
    }
}
