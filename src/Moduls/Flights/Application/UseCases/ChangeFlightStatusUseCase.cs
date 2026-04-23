using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Application.UseCases;

public sealed class ChangeFlightStatusUseCase
{
    private readonly IFlightsRepository _flights;
    private readonly IFlightStatusTransitionRepository _transitions;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeFlightStatusUseCase(
        IFlightsRepository flights,
        IFlightStatusTransitionRepository transitions,
        IUnitOfWork unitOfWork)
    {
        _flights = flights;
        _transitions = transitions;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int flightId, int newFlightStatusId, CancellationToken cancellationToken = default)
    {
        var flight = await _flights.GetByIdAsync(FlightsId.Create(flightId), cancellationToken);
        if (flight is null)
            throw new InvalidOperationException($"No existe el vuelo con id {flightId}.");

        var currentId = flight.FlightStatusId.Value;
        if (currentId == newFlightStatusId)
            return;

        var allowed = await _transitions.IsTransitionAllowedAsync(currentId, newFlightStatusId, cancellationToken);
        if (!allowed)
            throw new InvalidOperationException(
                $"Transicion de estado no permitida: {currentId} -> {newFlightStatusId}. Revise flight_status_transitions.");

        flight.UpdateFlightStatusId(newFlightStatusId);
        flight.TouchUpdatedAt();
        await _flights.UpdateAsync(flight, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
