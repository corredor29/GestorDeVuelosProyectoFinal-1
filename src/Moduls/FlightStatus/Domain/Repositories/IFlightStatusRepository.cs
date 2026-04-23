using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.ValueObject;
using DomainFlightStatus = GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.Aggregate.FlightStatus;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.Repositories;

public interface IFlightStatusRepository
{
    Task<DomainFlightStatus?> GetByIdAsync(FlightStatusId id, CancellationToken cancellationToken = default);

    Task<DomainFlightStatus?> GetByNameAsync(FlightStatusName name, CancellationToken cancellationToken = default);

    Task<DomainFlightStatus?> GetByNameStringAsync(string name, CancellationToken cancellationToken = default);

    Task<IEnumerable<DomainFlightStatus>> GetAllAsync(CancellationToken cancellationToken = default);

    Task SaveAsync(DomainFlightStatus flightStatus, CancellationToken cancellationToken = default);

    Task UpdateAsync(DomainFlightStatus flightStatus, CancellationToken cancellationToken = default);

    Task DeleteAsync(FlightStatusId id, CancellationToken cancellationToken = default);


    Task<bool> DeleteByNameAsync(FlightStatusName name, CancellationToken cancellationToken = default);
}
