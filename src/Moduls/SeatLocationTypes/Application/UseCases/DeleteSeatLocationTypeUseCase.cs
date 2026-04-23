using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Application.UseCases;

public sealed class DeleteSeatLocationTypeUseCase
{
    private readonly ISeatLocationTypesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSeatLocationTypeUseCase(ISeatLocationTypesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(SeatLocationTypesId.Create(id), cancellationToken);
        if (existing is null)
            throw new InvalidOperationException($"The seat type with id was not found {id}.");

        await _repository.DeleteAsync(SeatLocationTypesId.Create(id), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ExecuteByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteByNameAsync(SeatLocationTypesName.Create(name), cancellationToken);
        if (!deleted)
            throw new InvalidOperationException($"The seat type with name '{name}' was not found.");

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
