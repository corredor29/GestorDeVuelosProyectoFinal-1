using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Application.UseCases;

public sealed class ValidateFlightStatusTransitionUseCase
{
    private readonly IFlightStatusTransitionRepository _transitions;

    public ValidateFlightStatusTransitionUseCase(IFlightStatusTransitionRepository transitions)
    {
        _transitions = transitions;
    }

    public Task<bool> ExecuteAsync(int currentStatusId, int newStatusId, CancellationToken cancellationToken = default)
    {
        if (currentStatusId == newStatusId)
            return Task.FromResult(true);

        return _transitions.IsTransitionAllowedAsync(currentStatusId, newStatusId, cancellationToken);
    }
}
