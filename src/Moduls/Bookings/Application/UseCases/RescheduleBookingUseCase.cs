using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.ReschedulingHistory.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.ReschedulingHistory.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.WaitingList.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Application.UseCases;

public sealed class RescheduleBookingUseCase
{
    private readonly IBookingsRepository _bookingsRepository;
    private readonly IBookingStatuseRepository _statusesRepository;
    private readonly IFlightsRepository _flightsRepository;
    private readonly IBookingFlightsRepository _bookingFlightsRepository;
    private readonly IReschedulingHistoryRepository _historyRepository;
    private readonly IWaitingListRepository _waitingListRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RescheduleBookingUseCase(
        IBookingsRepository bookingsRepository,
        IBookingStatuseRepository statusesRepository,
        IFlightsRepository flightsRepository,
        IBookingFlightsRepository bookingFlightsRepository,
        IReschedulingHistoryRepository historyRepository,
        IWaitingListRepository waitingListRepository,
        IUnitOfWork unitOfWork)
    {
        _bookingsRepository        = bookingsRepository;
        _statusesRepository        = statusesRepository;
        _flightsRepository         = flightsRepository;
        _bookingFlightsRepository  = bookingFlightsRepository;
        _historyRepository         = historyRepository;
        _waitingListRepository     = waitingListRepository;
        _unitOfWork                = unitOfWork;
    }

    public async Task<bool> ExecuteAsync(
        int bookingId,
        int currentFlightId,
        int newFlightId,
        string reason,
        CancellationToken cancellationToken = default)
    {
        // 1. Verificar que la reserva exista y esté Confirmada
        var booking = await _bookingsRepository.GetByIdAsync(BookingId.Create(bookingId), cancellationToken)
            ?? throw new InvalidOperationException($"No existe la reserva con id {bookingId}.");

        var confirmedStatus = await _statusesRepository.GetByNameAsync("Confirmed")
            ?? throw new InvalidOperationException("No existe el estado 'Confirmed' en el sistema.");

        if (booking.BookingStatusId.Value != confirmedStatus.Id.Value)
            throw new InvalidOperationException("Solo se pueden reprogramar reservas en estado Confirmada.");

        // 2. Verificar que el nuevo vuelo sea distinto
        if (currentFlightId == newFlightId)
            throw new InvalidOperationException("El nuevo vuelo no puede ser el mismo vuelo actual.");

        var newFlight = await _flightsRepository.GetByIdAsync(FlightsId.Create(newFlightId), cancellationToken)
            ?? throw new InvalidOperationException($"No existe el vuelo con id {newFlightId}.");

        var currentFlight = await _flightsRepository.GetByIdAsync(FlightsId.Create(currentFlightId), cancellationToken)
            ?? throw new InvalidOperationException($"No existe el vuelo actual con id {currentFlightId}.");

        // 3. Validar compatibilidad de ruta (mismo route_id)
        if (newFlight.RouteId.Value != currentFlight.RouteId.Value)
            throw new InvalidOperationException(
                $"El nuevo vuelo no es compatible: ruta {newFlight.RouteId.Value} distinta a la actual {currentFlight.RouteId.Value}.");

        // 4. Validar que la fecha del nuevo vuelo sea futura
        if (newFlight.DepartureAt.Value <= DateTime.UtcNow)
            throw new InvalidOperationException("El nuevo vuelo ya ha partido o está en curso.");

        // 5. Verificar disponibilidad
        if (!newFlight.HasAvailableSeats(1))
            return false;

        // 6. Primero decrementar el nuevo vuelo, luego liberar el anterior
        newFlight.DecrementAvailableSeats(1);
        newFlight.TouchUpdatedAt();
        await _flightsRepository.UpdateAsync(newFlight, cancellationToken);

        if (currentFlight.AvailableSeats.Value < currentFlight.TotalCapacity.Value)
        {
            currentFlight.IncrementAvailableSeats(1);
            currentFlight.TouchUpdatedAt();
            await _flightsRepository.UpdateAsync(currentFlight, cancellationToken);
        }

        var bookingFlight = await _bookingFlightsRepository.GetByBookingAndFlightAsync(
            BookingId.Create(bookingId),
            FlightsId.Create(currentFlightId),
            cancellationToken);

        if (bookingFlight is not null)
        {
            bookingFlight.Update(bookingId, newFlightId, bookingFlight.PartialAmount.Value);
            await _bookingFlightsRepository.UpdateAsync(bookingFlight, cancellationToken);
        }

        // 8. Cambiar estado de la reserva
        var rescheduledStatus = await _statusesRepository.GetByNameAsync("Rescheduled");
        var targetStatusId    = rescheduledStatus?.Id.Value ?? confirmedStatus.Id.Value;

        booking.ChangeStatus(targetStatusId);
        booking.TouchUpdatedAt();
        await _bookingsRepository.UpdateAsync(booking, cancellationToken);

        // 9. Registrar en historial
        var history = ReschedulingHistor.Create(bookingId, currentFlightId, newFlightId, reason);
        await _historyRepository.SaveAsync(history, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}