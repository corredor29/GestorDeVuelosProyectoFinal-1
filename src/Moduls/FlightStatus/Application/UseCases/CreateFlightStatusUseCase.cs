using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.Repositories;
using DomainFlightStatus = GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.Aggregate.FlightStatus;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Application.UseCases;

public sealed class CreateFlightStatusUseCase
{
    private readonly IFlightStatusRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFlightStatusUseCase(IFlightStatusRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string name, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByNameStringAsync(name, cancellationToken);
        if (existing is not null)
            throw new InvalidOperationException($"Ya existe un estado de vuelo con el nombre '{existing.Name.Value}'.");

        var aggregate = DomainFlightStatus.Create(name);
        await _repository.SaveAsync(aggregate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
