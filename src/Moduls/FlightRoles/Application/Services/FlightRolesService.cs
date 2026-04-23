using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Application.Services;

public sealed class FlightRolesService : IFlightRolesService
{
    private readonly GetFlightRolesUseCase _get;
    private readonly CreateFlightRoleUseCase _create;
    private readonly UpdateFlightRoleUseCase _update;
    private readonly DeleteFlightRoleUseCase _delete;

    public FlightRolesService(
        GetFlightRolesUseCase get,
        CreateFlightRoleUseCase create,
        UpdateFlightRoleUseCase update,
        DeleteFlightRoleUseCase delete)
    {
        _get = get;
        _create = create;
        _update = update;
        _delete = delete;
    }

    public Task<IEnumerable<FlightRole>> GetAllAsync(CancellationToken cancellationToken = default)
        => _get.GetAllAsync(cancellationToken);

    public Task<FlightRole?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _get.GetByIdAsync(id, cancellationToken);

    public Task<FlightRole?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
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
