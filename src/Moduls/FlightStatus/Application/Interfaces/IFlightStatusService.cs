using DomainFlightStatus = GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.Aggregate.FlightStatus;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Application.Interfaces;

public interface IFlightStatusService
{
    Task<IEnumerable<DomainFlightStatus>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<DomainFlightStatus?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<DomainFlightStatus?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task CreateAsync(string name, CancellationToken cancellationToken = default);

    Task UpdateAsync(int id, string name, CancellationToken cancellationToken = default);

    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);

    Task DeleteByNameAsync(string name, CancellationToken cancellationToken = default);
}
