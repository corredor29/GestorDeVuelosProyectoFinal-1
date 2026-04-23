using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Repositories;

public interface IBookingStatuseRepository
{
    Task<Aggregate.BookingStatuses?> GetByIdAsync(BookingStatusesId id);
    Task<Aggregate.BookingStatuses?> GetByNameAsync(string name);
    Task<IEnumerable<Aggregate.BookingStatuses>> GetAllAsync();
    Task SaveAsync(Aggregate.BookingStatuses bookingStatuses);
    Task UpdateAsync(Aggregate.BookingStatuses bookingStatuses);
    Task DeleteAsync(BookingStatusesId id);
}
