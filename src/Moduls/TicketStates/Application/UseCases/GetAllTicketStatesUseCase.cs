using System;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Domain.Repositories;
namespace GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Application.UseCases;

public class GetAllTicketStatesUseCase
{
    private readonly ITicketStatesRepository _TicketStatesrepository;

    public GetAllTicketStatesUseCase(ITicketStatesRepository TicketStatesrepository)
    {
        _TicketStatesrepository = TicketStatesrepository;
    }

    public Task<IReadOnlyCollection<TicketState>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return _TicketStatesrepository.GetAllAsync(cancellationToken);
    }
}
