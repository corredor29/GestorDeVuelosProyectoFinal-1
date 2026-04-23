using System;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Application.UseCases;

public sealed class DeleteTicketUseCase
{
    private readonly ITicketRepository _Ticketrepository;

    public DeleteTicketUseCase(ITicketRepository Ticketrepository)
    {
        _Ticketrepository = Ticketrepository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var ticketId = TicketId.Create(id);

        var existing = await _Ticketrepository.GetByIdAsync(ticketId);

        if (existing is null)
            return false;

        await _Ticketrepository.DeleteAsync(ticketId);

        return true;
    }
}