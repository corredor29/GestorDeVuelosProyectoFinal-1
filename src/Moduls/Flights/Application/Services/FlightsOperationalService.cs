using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Application.UseCases;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Application.Services;

public sealed class FlightsOperationalService : IFlightsOperationalService
{
    private readonly ChangeFlightStatusUseCase _changeStatus;
    private readonly ConfirmCheckInUseCase _confirmCheckIn;
    private readonly CancelCheckInUseCase _cancelCheckIn;
    private readonly ValidateFlightStatusTransitionUseCase _validateTransition;

    public FlightsOperationalService(
        ChangeFlightStatusUseCase changeStatus,
        ConfirmCheckInUseCase confirmCheckIn,
        CancelCheckInUseCase cancelCheckIn,
        ValidateFlightStatusTransitionUseCase validateTransition)
    {
        _changeStatus = changeStatus;
        _confirmCheckIn = confirmCheckIn;
        _cancelCheckIn = cancelCheckIn;
        _validateTransition = validateTransition;
    }

    public Task ChangeFlightStatusAsync(int flightId, int newFlightStatusId, CancellationToken cancellationToken = default)
        => _changeStatus.ExecuteAsync(flightId, newFlightStatusId, cancellationToken);

    public Task ConfirmCheckInAsync(int flightId, CancellationToken cancellationToken = default)
        => _confirmCheckIn.ExecuteAsync(flightId, cancellationToken);

    public Task CancelCheckInAsync(int flightId, CancellationToken cancellationToken = default)
        => _cancelCheckIn.ExecuteAsync(flightId, cancellationToken);

    public Task<bool> IsStatusTransitionAllowedAsync(int currentStatusId, int newStatusId, CancellationToken cancellationToken = default)
        => _validateTransition.ExecuteAsync(currentStatusId, newStatusId, cancellationToken);
}
