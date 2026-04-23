using System;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Application.UseCases;

public sealed class CreateTicketUseCase
{
    private readonly ITicketRepository _Ticketrepository;

    public CreateTicketUseCase(ITicketRepository Ticketrepository)
    {
        _Ticketrepository = Ticketrepository;
    }

    public async Task<TicketAggregate> ExecuteAsync(
        int id,
        string code,
        DateTime issueDate,
        int reservationPassengerId,
        int statusId,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        var ticketAggregate = TicketAggregate.Create(
            id,
            code,
            issueDate,
            now,
            now,
            reservationPassengerId,
            statusId
        );

        await _Ticketrepository.SaveAsync(ticketAggregate);

        return ticketAggregate;
    }
}