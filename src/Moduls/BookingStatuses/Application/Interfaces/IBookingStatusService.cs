using StatusAggregate = GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Aggregate.BookingStatuses;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Application.Interfaces;

public interface IBookingStatusService
{
    Task CreateStatus(string name);
    Task UpdateStatus(int id, string name);
    Task<StatusAggregate?> GetStatusById(int id);
    Task<IEnumerable<StatusAggregate>> GetAllStatuses();
    Task DeleteStatus(int id);
}