using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Application.UseCases;

public sealed class UpdateSeatLocationTypeUseCase
{
    private readonly ISeatLocationTypesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSeatLocationTypeUseCase(ISeatLocationTypesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, string newName, CancellationToken cancellationToken = default)
    {
        var seatLocationType = await _repository.GetByIdAsync(SeatLocationTypesId.Create(id), cancellationToken);
        if (seatLocationType is null)
            throw new InvalidOperationException($"No se encontro el tipo de asiento con id {id}.");

        var duplicate = await _repository.GetByNameStringAsync(newName, cancellationToken);
        if (duplicate is not null && duplicate.Id?.Value != id)
            throw new InvalidOperationException($"There is already another type of seat with the name '{newName}'.");

        seatLocationType.UpdateName(newName);
        await _repository.UpdateAsync(seatLocationType, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
