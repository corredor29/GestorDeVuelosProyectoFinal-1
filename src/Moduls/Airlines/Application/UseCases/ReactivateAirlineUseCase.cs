using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.Application.UseCases;

public sealed class ReactivateAirlineUseCase
{
    private readonly IAirlinesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ReactivateAirlineUseCase(IAirlinesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id)
    {
        var existing = await _repository.GetByIdAsync(AirlinesId.Create(id));
        if (existing is null)
            throw new InvalidOperationException($"No se encontró la aerolínea con ID {id}.");

        await _repository.ReactivateAsync(AirlinesId.Create(id));
        await _unitOfWork.SaveChangesAsync();
    }
}
