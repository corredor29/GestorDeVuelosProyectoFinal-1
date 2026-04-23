using System;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Application.UseCases;

public sealed class UpdateTicketUseCase
{
    private readonly ITicketRepository _Ticketrepository;

    public UpdateTicketUseCase(ITicketRepository Ticketrepository)
    {
        _Ticketrepository = Ticketrepository;
    }

    public async Task<TicketAggregate> ExecuteAsync(
        int id,
        string? code,
        DateTime? issueDate,
        int? reservationPassengerId,
        int? statusId,
        CancellationToken cancellationToken = default)
    {
        var ticketId = TicketId.Create(id);

        var existing = await _Ticketrepository.GetByIdAsync(ticketId);

        if (existing is null)
            throw new KeyNotFoundException($"Ticket with id '{id}' was not found.");

        if (code is not null)
            existing.UpdateCode(code);

        if (issueDate is not null)
            existing.UpdateIssueDate(issueDate.Value);

        if (reservationPassengerId is not null)
            existing.UpdateReservationPassenger(reservationPassengerId.Value);

        if (statusId is not null)
            existing.UpdateStatus(statusId.Value);

        await _Ticketrepository.UpdateAsync(existing);

        return existing;
    }
}
