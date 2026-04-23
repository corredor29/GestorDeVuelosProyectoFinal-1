using System;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Application.Interfaces;

public interface ITicketService
{
    Task<TicketAggregate> CreateAsync(
        string code,
        DateTime issueDate,
        int reservationPassengerId,
        int statusId,
        CancellationToken cancellationToken = default);

    Task<TicketAggregate?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IEnumerable<TicketAggregate>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TicketAggregate> UpdateAsync(
        int id,
        string? code,
        DateTime? issueDate,
        int? reservationPassengerId,
        int? statusId,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}