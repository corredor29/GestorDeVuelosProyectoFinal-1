using System.Collections.Generic;
using System.Threading.Tasks;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.Repositories;

public interface IFlightAssignmentsRepository
{
    Task<FlightAssignment?> GetByIdAsync(FlightAssignmentId id, CancellationToken cancellationToken = default);

    Task<FlightAssignment?> GetByFlightAndStaffAsync(FlightsId flightId, PersonalId staffId, CancellationToken cancellationToken = default);

    Task<IEnumerable<FlightAssignment>> GetByFlightIdAsync(FlightsId flightId, CancellationToken cancellationToken = default);

    Task<IEnumerable<FlightAssignment>> GetByStaffIdAsync(PersonalId staffId, CancellationToken cancellationToken = default);

    Task<IEnumerable<FlightAssignment>> GetByRoleIdAsync(FlightRolesId flightRoleId, CancellationToken cancellationToken = default);

    Task<IEnumerable<FlightAssignment>> GetAllAsync(CancellationToken cancellationToken = default);

    Task SaveAsync(FlightAssignment assignment, CancellationToken cancellationToken = default);

    Task UpdateAsync(FlightAssignment assignment, CancellationToken cancellationToken = default);

    Task DeleteAsync(FlightAssignmentId id, CancellationToken cancellationToken = default);
}
