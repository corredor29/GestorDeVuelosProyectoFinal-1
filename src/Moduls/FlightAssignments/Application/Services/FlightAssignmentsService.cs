using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Application.Services;

public sealed class FlightAssignmentsService : IFlightAssignmentsService
{
    private readonly GetFlightAssignmentsUseCase _get;
    private readonly CreateFlightAssignmentUseCase _create;
    private readonly UpdateFlightAssignmentUseCase _update;
    private readonly DeleteFlightAssignmentUseCase _delete;

    public FlightAssignmentsService(
        GetFlightAssignmentsUseCase get,
        CreateFlightAssignmentUseCase create,
        UpdateFlightAssignmentUseCase update,
        DeleteFlightAssignmentUseCase delete)
    {
        _get = get;
        _create = create;
        _update = update;
        _delete = delete;
    }

    public Task<IEnumerable<FlightAssignment>> GetAllAsync(CancellationToken cancellationToken = default)
        => _get.GetAllAsync(cancellationToken);

    public Task<FlightAssignment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _get.GetByIdAsync(id, cancellationToken);

    public Task<IEnumerable<FlightAssignment>> GetByFlightIdAsync(int flightId, CancellationToken cancellationToken = default)
        => _get.GetByFlightIdAsync(flightId, cancellationToken);

    public Task<IEnumerable<FlightAssignment>> GetByStaffIdAsync(int staffId, CancellationToken cancellationToken = default)
        => _get.GetByStaffIdAsync(staffId, cancellationToken);

    public Task<IEnumerable<FlightAssignment>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default)
        => _get.GetByRoleIdAsync(roleId, cancellationToken);

    public Task CreateAsync(int flightId, int staffId, int flightRoleId, CancellationToken cancellationToken = default)
        => _create.ExecuteAsync(flightId, staffId, flightRoleId, cancellationToken);

    public Task UpdateAsync(int id, int flightId, int staffId, int flightRoleId, CancellationToken cancellationToken = default)
        => _update.ExecuteAsync(id, flightId, staffId, flightRoleId, cancellationToken);

    public Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
        => _delete.ExecuteByIdAsync(id, cancellationToken);
}
