using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.ValueObject;
using DomainFlightStatus = GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.Aggregate.FlightStatus;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Application.UseCases;

public sealed class GetFlightStatusesUseCase
{
    private readonly IFlightStatusRepository _repository;

    public GetFlightStatusesUseCase(IFlightStatusRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<DomainFlightStatus>> GetAllAsync(CancellationToken cancellationToken = default)
        => _repository.GetAllAsync(cancellationToken);

    public async Task<DomainFlightStatus?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _repository.GetByIdAsync(FlightStatusId.Create(id), cancellationToken);

    public Task<DomainFlightStatus?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => _repository.GetByNameStringAsync(name, cancellationToken);
}
