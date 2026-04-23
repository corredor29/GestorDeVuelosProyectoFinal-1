using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.Repositories;

public interface ICheckinsRepository
{
    Task<Checkin?> GetByIdAsync(CheckinsId id);
    Task<Checkin?> GetByBoardingPassAsync(CheckinsBoardingPassNumber boardingPassNumber);
    Task<Checkin?> GetByTicketIdAsync(TicketId ticketId);
    Task<IEnumerable<Checkin>> GetAllAsync();
    Task SaveAsync(Checkin checkin);
    Task UpdateAsync(Checkin checkin);
    Task DeleteAsync(CheckinsId id);
}