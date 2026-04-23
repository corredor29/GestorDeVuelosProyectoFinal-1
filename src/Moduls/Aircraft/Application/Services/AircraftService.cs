using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Application.UseCases;
using AircraftAggregate = GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Aggregate.Aircraft;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Application.Services;

public sealed class AircraftService : IAircraftService
{
    private readonly GetAllAircraftUseCase _getAll;
    private readonly GetAircraftByIdUseCase _getById;
    private readonly GetAircraftByRegistrationUseCase _getByRegistration;
    private readonly GetAircraftByAirlineIdUseCase _getByAirline;
    private readonly CreateAircraftUseCase _create;
    private readonly UpdtaeAircraftUseCase _update;
    private readonly DeleteAircraftUseCase _deactivate;

    public AircraftService(
        GetAllAircraftUseCase getAll,
        GetAircraftByIdUseCase getById,
        GetAircraftByRegistrationUseCase getByRegistration,
        GetAircraftByAirlineIdUseCase getByAirline,
        CreateAircraftUseCase create,
        UpdtaeAircraftUseCase update,
        DeleteAircraftUseCase deactivate)
    {
        _getAll = getAll;
        _getById = getById;
        _getByRegistration = getByRegistration;
        _getByAirline = getByAirline;
        _create = create;
        _update = update;
        _deactivate = deactivate;
    }

    public Task<IReadOnlyCollection<AircraftAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
        => _getAll.ExecuteAsync(cancellationToken);

    public Task<AircraftAggregate?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _getById.ExecuteAsync(id, cancellationToken);

    public Task<AircraftAggregate?> GetByRegistrationAsync(string registration, CancellationToken cancellationToken = default)
        => _getByRegistration.ExecuteAsync(registration, cancellationToken);

    public Task<IReadOnlyCollection<AircraftAggregate>> GetByAirlineIdAsync(int airlineId, CancellationToken cancellationToken = default)
        => _getByAirline.ExecuteAsync(airlineId, cancellationToken);

    public Task<AircraftAggregate> CreateAsync(
        int modelId,
        int airlineId,
        string registration,
        DateTime? manufacturedDate,
        bool isActive,
        CancellationToken cancellationToken = default)
        => _create.ExecuteAsync(modelId, airlineId, registration, manufacturedDate, isActive, cancellationToken);

    public Task<AircraftAggregate> UpdateAsync(
        int id,
        int modelId,
        int airlineId,
        string registration,
        DateTime? manufacturedDate,
        bool isActive,
        CancellationToken cancellationToken = default)
        => _update.ExecuteAsync(id, modelId, airlineId, registration, manufacturedDate, isActive, cancellationToken);

    public Task<bool> DeactivateAsync(int id, CancellationToken cancellationToken = default)
        => _deactivate.ExecuteAsync(id, cancellationToken);
}
