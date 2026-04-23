namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Application.Interfaces;

public interface IFlightsOperationalService
{
    /// <summary>Cambia el estado del vuelo solo si existe transicion permitida en flight_status_transitions.</summary>
    Task ChangeFlightStatusAsync(int flightId, int newFlightStatusId, CancellationToken cancellationToken = default);

    /// <summary>Confirma check-in: decrementa available_seats en 1.</summary>
    Task ConfirmCheckInAsync(int flightId, CancellationToken cancellationToken = default);

    /// <summary>Cancela check-in: incrementa available_seats en 1 (sin superar total_capacity).</summary>
    Task CancelCheckInAsync(int flightId, CancellationToken cancellationToken = default);

    /// <summary>Comprueba si la transicion de estado esta permitida.</summary>
    Task<bool> IsStatusTransitionAllowedAsync(int currentStatusId, int newStatusId, CancellationToken cancellationToken = default);
}
