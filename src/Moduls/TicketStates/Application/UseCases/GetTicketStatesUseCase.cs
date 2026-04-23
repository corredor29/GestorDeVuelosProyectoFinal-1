using System;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Application.UseCases;

public class GetTicketStatesUseCase
{
    private readonly ITicketStatesRepository _repository;

    public GetTicketStatesUseCase(ITicketStatesRepository repository)
    {
        _repository = repository;
    }

    public async Task<TicketState> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var ticketStateId = TicketStatesId.Create(id);

        var result = await _repository.GetByIdAsync(ticketStateId, cancellationToken);

        if (result is null)
        {
            throw new KeyNotFoundException($"TicketState with id '{id}' was not found.");
        }

        return result;
    }
}
