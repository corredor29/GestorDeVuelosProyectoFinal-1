using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Application.UseCases;
using DomainFlightStatus = GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.Aggregate.FlightStatus;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Application.Services;

public sealed class FlightStatusService : IFlightStatusService
{
    private readonly GetFlightStatusesUseCase _get;
    private readonly CreateFlightStatusUseCase _create;
    private readonly UpdateFlightStatusUseCase _update;
    private readonly DeleteFlightStatusUseCase _delete;

    public FlightStatusService(
        GetFlightStatusesUseCase get,
        CreateFlightStatusUseCase create,
        UpdateFlightStatusUseCase update,
        DeleteFlightStatusUseCase delete)
    {
        _get = get;
        _create = create;
        _update = update;
        _delete = delete;
    }

    public Task<IEnumerable<DomainFlightStatus>> GetAllAsync(CancellationToken cancellationToken = default)
        => _get.GetAllAsync(cancellationToken);

    public Task<DomainFlightStatus?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _get.GetByIdAsync(id, cancellationToken);

    public Task<DomainFlightStatus?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => _get.GetByNameAsync(name, cancellationToken);

    public Task CreateAsync(string name, CancellationToken cancellationToken = default)
        => _create.ExecuteAsync(name, cancellationToken);

    public Task UpdateAsync(int id, string name, CancellationToken cancellationToken = default)
        => _update.ExecuteAsync(id, name, cancellationToken);

    public Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
        => _delete.ExecuteByIdAsync(id, cancellationToken);

    public Task DeleteByNameAsync(string name, CancellationToken cancellationToken = default)
        => _delete.ExecuteByNameAsync(name, cancellationToken);
}
