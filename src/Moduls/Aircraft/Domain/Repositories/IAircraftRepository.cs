using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.ValueObject;
using AircraftAggregate = GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Aggregate.Aircraft;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Domain.Repositories;

public interface IAircraftRepository
{
    Task AddAsync(AircraftAggregate aircraft, CancellationToken cancellationToken = default);
    Task<AircraftAggregate?> GetByIdAsync(AircraftId id, CancellationToken cancellationToken = default);
    Task<AircraftAggregate?> GetByRegistrationAsync(AircraftRegistration registration, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AircraftAggregate>> GetByAirlineIdAsync(AirlinesId airlineId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<AircraftAggregate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByRegistrationAsync(AircraftRegistration registration, CancellationToken cancellationToken = default);
    Task<bool> HasFutureFlightsAsync(AircraftId id, CancellationToken cancellationToken = default);
    Task UpdateAsync(AircraftAggregate aircraft, CancellationToken cancellationToken = default);
    Task<bool> DeleteByIdAsync(AircraftId id, CancellationToken cancellationToken = default);
}
