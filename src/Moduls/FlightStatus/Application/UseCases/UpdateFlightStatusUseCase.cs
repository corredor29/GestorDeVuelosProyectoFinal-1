using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Application.UseCases;

public sealed class UpdateFlightStatusUseCase
{
    private readonly IFlightStatusRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFlightStatusUseCase(IFlightStatusRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, string newName, CancellationToken cancellationToken = default)
    {
        var flightStatus = await _repository.GetByIdAsync(FlightStatusId.Create(id), cancellationToken);
        if (flightStatus is null)
            throw new InvalidOperationException($"No se encontro el estado de vuelo con id {id}.");

        var duplicate = await _repository.GetByNameStringAsync(newName, cancellationToken);
        if (duplicate is not null && duplicate.Id?.Value != id)
            throw new InvalidOperationException($"Ya existe otro estado con el nombre '{newName}'.");

        flightStatus.UpdateName(newName);
        await _repository.UpdateAsync(flightStatus, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
