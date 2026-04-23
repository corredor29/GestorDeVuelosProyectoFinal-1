using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Application.UseCases;

public sealed class DeleteFlightStatusUseCase
{
    private readonly IFlightStatusRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFlightStatusUseCase(IFlightStatusRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(FlightStatusId.Create(id), cancellationToken);
        if (existing is null)
            throw new InvalidOperationException($"No se encontro el estado de vuelo con id {id}.");

        await _repository.DeleteAsync(FlightStatusId.Create(id), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ExecuteByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteByNameAsync(FlightStatusName.Create(name), cancellationToken);
        if (!deleted)
            throw new InvalidOperationException($"No se encontro el estado de vuelo con nombre '{name}'.");

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
