using System;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Application.UseCases;

public class UpdateTicketStatesUseCase
{
    private readonly ITicketStatesRepository _TicketStaterepository;

    public UpdateTicketStatesUseCase(ITicketStatesRepository TicketStaterepository)
    {
        _TicketStaterepository = TicketStaterepository;
    }

    public async Task<TicketState> ExecuteAsync(
        int id,
        string newName,
        CancellationToken cancellationToken = default)
    {
        var ticketStateId = TicketStatesId.Create(id);

        var existing = await _TicketStaterepository.GetByIdAsync(ticketStateId, cancellationToken);

        if (existing is null)
        {
            throw new KeyNotFoundException($"TicketState with id '{id}' was not found.");
        }

        existing.UpdateName(newName);

        await _TicketStaterepository.SaveAsync(existing, cancellationToken);

        return existing;
    }
}
