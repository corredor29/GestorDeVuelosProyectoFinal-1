using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Application.Interfaces;

public interface ICheckinsService
{
    Task<Checkin> CreateAsync(int id, int ticketId, int staffId, int flightSeatId, DateTime checkedInAt, int checkinStatusId, string boardingPassNumber, CancellationToken cancellationToken = default);
    Task<Checkin?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Checkin>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Checkin> UpdateAsync(int id, int? newCheckinStatusId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}