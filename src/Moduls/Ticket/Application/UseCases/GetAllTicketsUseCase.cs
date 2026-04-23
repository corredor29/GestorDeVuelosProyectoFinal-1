using System;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Application.UseCases;

public sealed class GetAllTicketsUseCase
{
    private readonly ITicketRepository _repository;

    public GetAllTicketsUseCase(ITicketRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TicketAggregate>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync();
    }
}