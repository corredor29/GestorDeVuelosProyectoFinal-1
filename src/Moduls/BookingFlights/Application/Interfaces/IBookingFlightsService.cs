using DomainBookingFlight = GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.Aggregate.BookingFlight;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Application.Interfaces;

public interface IBookingFlightsService
{
    Task<IEnumerable<DomainBookingFlight>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DomainBookingFlight?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainBookingFlight>> GetByBookingIdAsync(int bookingId, CancellationToken cancellationToken = default);
    Task CreateAsync(int bookingId, int flightId, decimal partialAmount, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, int bookingId, int flightId, decimal partialAmount, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
}
