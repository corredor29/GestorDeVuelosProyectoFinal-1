using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Application.Interfaces;

public interface IFlightRolesService
{
    Task<IEnumerable<FlightRole>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<FlightRole?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<FlightRole?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task CreateAsync(string name, CancellationToken cancellationToken = default);

    Task UpdateAsync(int id, string name, CancellationToken cancellationToken = default);

    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);

    Task DeleteByNameAsync(string name, CancellationToken cancellationToken = default);
}
