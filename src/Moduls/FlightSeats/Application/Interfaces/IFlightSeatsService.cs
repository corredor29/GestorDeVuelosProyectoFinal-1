using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Application.Interfaces;

public interface IFlightSeatsService
{
    Task<FlightSeat> CreateAsync(
        int flightId, 
        int cabinTypeId, 
        int seatLocationTypeId, 
        bool isOccupied, 
        string code,
        CancellationToken cancellationToken = default);

    Task<FlightSeat?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<FlightSeat?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<FlightSeat>> GetByFlightIdAsync(int flightId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<FlightSeat>> GetByCabinTypeIdAsync(int cabinTypeId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<FlightSeat>> GetBySeatLocationTypeIdAsync(int seatLocationTypeId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<FlightSeat>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<FlightSeat> UpdateAsync(
        int id, 
        int flightId, 
        int cabinTypeId, 
        int seatLocationTypeId, 
        bool isOccupied, 
        string code,
        CancellationToken cancellationToken = default);

    Task<bool> isOccupiedAsync(int id, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
}
