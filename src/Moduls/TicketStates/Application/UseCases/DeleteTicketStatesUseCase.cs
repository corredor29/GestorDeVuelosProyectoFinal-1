using System;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.ValueObject;
namespace GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Application.UseCases;

public class DeleteTicketStatesUseCase
{
    private readonly ITicketStatesRepository _repository;

    public DeleteTicketStatesUseCase(ITicketStatesRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var ticketStateId = TicketStatesId.Create(id);

        var existing = await _repository.GetByIdAsync(ticketStateId, cancellationToken);

        if (existing is null)
        {
            return false;
        }

        await _repository.DeleteAsync(ticketStateId, cancellationToken);

        return true;
    }
}
