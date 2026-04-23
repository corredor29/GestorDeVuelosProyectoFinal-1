using System.Collections.Generic;
using System.Threading.Tasks;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.Repositories;

public interface IFlightRolesRepository
{
    Task<FlightRole?> GetByIdAsync(FlightRolesId id, CancellationToken cancellationToken = default);

    Task<FlightRole?> GetByNameAsync(FlightRolesName name, CancellationToken cancellationToken = default);

    Task<FlightRole?> GetByNameStringAsync(string name, CancellationToken cancellationToken = default);

    Task<IEnumerable<FlightRole>> GetAllAsync(CancellationToken cancellationToken = default);

    Task SaveAsync(FlightRole flightRole, CancellationToken cancellationToken = default);

    Task UpdateAsync(FlightRole flightRole, CancellationToken cancellationToken = default);

    Task DeleteAsync(FlightRolesId id, CancellationToken cancellationToken = default);

    Task<bool> DeleteByNameAsync(FlightRolesName name, CancellationToken cancellationToken = default);
}
