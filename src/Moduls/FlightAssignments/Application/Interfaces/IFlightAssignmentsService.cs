using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Application.Interfaces;

public interface IFlightAssignmentsService
{
    Task<IEnumerable<FlightAssignment>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<FlightAssignment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IEnumerable<FlightAssignment>> GetByFlightIdAsync(int flightId, CancellationToken cancellationToken = default);

    Task<IEnumerable<FlightAssignment>> GetByStaffIdAsync(int staffId, CancellationToken cancellationToken = default);

    Task<IEnumerable<FlightAssignment>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default);

    Task CreateAsync(int flightId, int staffId, int flightRoleId, CancellationToken cancellationToken = default);

    Task UpdateAsync(int id, int flightId, int staffId, int flightRoleId, CancellationToken cancellationToken = default);

    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
}
