using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;
using DomainFlights = GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Aggregate.Flights;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Repositories;

public interface IFlightsRepository
{
    Task<DomainFlights?> GetByIdAsync(FlightsId id, CancellationToken cancellationToken = default);

    Task<DomainFlights?> GetByFlightCodeAsync(string flightCode, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DomainFlights>> GetByAircraftIdAsync(int aircraftId, CancellationToken cancellationToken = default);

    /// <summary>Vuelos cuyo avion tiene la matricula indicada.</summary>
    Task<IReadOnlyList<DomainFlights>> GetByAircraftRegistrationAsync(string registration, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DomainFlights>> GetByTotalCapacityAsync(int totalCapacity, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DomainFlights>> GetByRouteIdAsync(int routeId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DomainFlights>> GetByFlightStatusIdAsync(int flightStatusId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DomainFlights>> GetByAirlineIdAsync(int airlineId, CancellationToken cancellationToken = default);

    /// <summary>Rutas que salen o llegan al aeropuerto indicado.</summary>
    Task<IReadOnlyList<DomainFlights>> GetByAirportIdAsync(int airportId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DomainFlights>> GetByDepartureBetweenAsync(DateTime fromUtcInclusive, DateTime toUtcInclusive, CancellationToken cancellationToken = default);

    Task<IEnumerable<DomainFlights>> GetAllAsync(CancellationToken cancellationToken = default);

    Task SaveAsync(DomainFlights flight, CancellationToken cancellationToken = default);

    Task UpdateAsync(DomainFlights flight, CancellationToken cancellationToken = default);

    Task DeleteAsync(FlightsId id, CancellationToken cancellationToken = default);

    Task<int> DeleteByFlightCodeAsync(string flightCode, CancellationToken cancellationToken = default);

    Task<int> DeleteByAircraftIdAsync(int aircraftId, CancellationToken cancellationToken = default);

    Task<int> DeleteByAircraftRegistrationAsync(string registration, CancellationToken cancellationToken = default);

    Task<int> DeleteByTotalCapacityAsync(int totalCapacity, CancellationToken cancellationToken = default);

    Task<int> DeleteByRouteIdAsync(int routeId, CancellationToken cancellationToken = default);

    Task<int> DeleteByFlightStatusIdAsync(int flightStatusId, CancellationToken cancellationToken = default);

    Task<int> DeleteByAirlineIdAsync(int airlineId, CancellationToken cancellationToken = default);

    Task<int> DeleteByAirportIdAsync(int airportId, CancellationToken cancellationToken = default);

    Task<int> DeleteByDepartureBetweenAsync(DateTime fromUtcInclusive, DateTime toUtcInclusive, CancellationToken cancellationToken = default);
}
