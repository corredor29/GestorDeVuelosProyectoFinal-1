using System;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Application.UseCases;

public class CreateTicketStatesUseCase
{
   private readonly ITicketStatesRepository _TicketStatesrepository;

    public CreateTicketStatesUseCase(ITicketStatesRepository TicketStatesrepository)
    {
        _TicketStatesrepository = TicketStatesrepository;
    }

    public async Task<TicketState> ExecuteAsync(
        int id,
        string name,
        CancellationToken cancellationToken = default)
    {
        var existing = await _TicketStatesrepository.GetByIdAsync(
            Domain.ValueObject.TicketStatesId.Create(id)
        );

        if (existing is not null)
        {
            throw new InvalidOperationException($"TicketState with id '{id}' already exists.");
        }

        var ticketState = TicketState.Create(id, name);

        await _TicketStatesrepository.SaveAsync(ticketState);

        return ticketState;
    }
}
