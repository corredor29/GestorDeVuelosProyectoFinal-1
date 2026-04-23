using System;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Application.UseCases;

public sealed class GetTicketUseCase
{
    private readonly ITicketRepository _Ticketrepository;

    public GetTicketUseCase(ITicketRepository Ticketrepository)
    {
        _Ticketrepository = Ticketrepository;
    }

    public async Task<TicketAggregate> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var ticketId = TicketId.Create(id);

        var result = await _Ticketrepository.GetByIdAsync(ticketId);

        if (result is null)
            throw new KeyNotFoundException($"Ticket with id '{id}' was not found.");

        return result;
    }
}