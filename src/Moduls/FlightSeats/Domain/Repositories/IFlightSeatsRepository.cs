using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.Repositories;

public interface IFlightSeatsRepository
{
    Task AddAsync(FlightSeat flightSeat, CancellationToken cancellationToken = default);
    Task<FlightSeat?> GetByIdAsync(FlightSeatsId id, CancellationToken cancellationToken = default);
    Task<FlightSeat?> GetByCodeAsync(FlightSeatsCode code, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<FlightSeat>> GetByFlightIdAsync(FlightsId flightId, CancellationToken cancellationToken = default);
     Task<IReadOnlyCollection<FlightSeat>> GetByCabinTypeIdAsync(CabinTypesId cabinTypeId, CancellationToken cancellationToken = default);

     Task<IReadOnlyCollection<FlightSeat>> GetBySeatLocationTypeIdAsync(SeatLocationTypesId id, CancellationToken cancellationToken = default);

     Task<IReadOnlyCollection<FlightSeat>> GetAllAsync( CancellationToken cancellationToken = default);
     Task<bool> ExistsByCodeAsync (FlightSeatsCode code, CancellationToken cancellationToken = default);

     Task <bool> HasFutureFlightsAsync(FlightSeatsId id, CancellationToken cancellationToken = default);
     Task UpdateAsync(FlightSeat flightSeat, CancellationToken cancellationToken = default);
     Task<bool> DeleteByIdAsync(FlightSeatsId id, CancellationToken cancellationToken = default);
}
