using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Application.UseCases;

public sealed class GetFlightAssignmentsUseCase
{
    private readonly IFlightAssignmentsRepository _repository;

    public GetFlightAssignmentsUseCase(IFlightAssignmentsRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<FlightAssignment>> GetAllAsync(CancellationToken cancellationToken = default)
        => _repository.GetAllAsync(cancellationToken);

    public Task<FlightAssignment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _repository.GetByIdAsync(FlightAssignmentId.Create(id), cancellationToken);

    public Task<IEnumerable<FlightAssignment>> GetByFlightIdAsync(int flightId, CancellationToken cancellationToken = default)
        => _repository.GetByFlightIdAsync(FlightsId.Create(flightId), cancellationToken);

    public Task<IEnumerable<FlightAssignment>> GetByStaffIdAsync(int staffId, CancellationToken cancellationToken = default)
        => _repository.GetByStaffIdAsync(PersonalId.Create(staffId), cancellationToken);

    public Task<IEnumerable<FlightAssignment>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default)
        => _repository.GetByRoleIdAsync(FlightRolesId.Create(roleId), cancellationToken);
}
