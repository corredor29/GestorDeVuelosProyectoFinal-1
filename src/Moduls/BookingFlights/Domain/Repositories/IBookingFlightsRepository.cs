using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.Repositories;

public interface IBookingFlightsRepository
{
    Task<BookingFlight?> GetByIdAsync(BookingFlightsId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingFlight>> GetByBookingIdAsync(BookingId bookingId, CancellationToken cancellationToken = default);
    Task<BookingFlight?> GetByBookingAndFlightAsync(BookingId bookingId, FlightsId flightId, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingFlight>> GetAllAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(BookingFlight bookingFlight, CancellationToken cancellationToken = default);
    Task UpdateAsync(BookingFlight bookingFlight, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(BookingFlightsId id, CancellationToken cancellationToken = default);
}
