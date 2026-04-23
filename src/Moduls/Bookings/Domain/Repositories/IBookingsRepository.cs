using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Repositories;

public interface IBookingsRepository
{
    Task<Booking?> GetByIdAsync(BookingId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetByClientIdAsync(CustomersId clientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetByStatusIdAsync(BookingStatusesId statusId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetByBookedAtRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetAllAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(Booking booking, CancellationToken cancellationToken = default);
    Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(BookingId id, CancellationToken cancellationToken = default);
    Task DeleteByClientIdAsync(CustomersId clientId, CancellationToken cancellationToken = default);
}
