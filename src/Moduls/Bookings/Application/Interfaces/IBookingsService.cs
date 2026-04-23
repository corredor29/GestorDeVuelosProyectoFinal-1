using DomainBooking = GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Aggregate.Booking;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Application.Interfaces;

public interface IBookingsService
{
    Task<IEnumerable<DomainBooking>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DomainBooking?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainBooking>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainBooking>> GetByStatusIdAsync(int statusId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainBooking>> GetByBookedAtRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task CreateAsync(int clientId, DateTime bookedAt, int bookingStatusId, decimal totalAmount, DateTime? expiresAt, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, int clientId, DateTime bookedAt, int bookingStatusId, decimal totalAmount, DateTime? expiresAt, CancellationToken cancellationToken = default);
    Task ConfirmAsync(int bookingId, CancellationToken cancellationToken = default);
    Task CancelAsync(int bookingId, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task DeleteByClientIdAsync(int clientId, CancellationToken cancellationToken = default);
}
