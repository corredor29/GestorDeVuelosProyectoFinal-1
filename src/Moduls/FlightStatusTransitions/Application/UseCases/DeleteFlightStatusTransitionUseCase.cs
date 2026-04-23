using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Application.UseCases;

public sealed class DeleteFlightStatusTransitionUseCase
{
    private readonly IFlightStatusTransitionsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFlightStatusTransitionUseCase(IFlightStatusTransitionsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(FlightStatusTransitionsId.Create(id), cancellationToken);
        if (existing is null)
            throw new InvalidOperationException($"No se encontro la transicion con ID {id}.");

        await _repository.DeleteByIdAsync(FlightStatusTransitionsId.Create(id), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
