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

/// <summary>
/// Se ejecuta después de cancelar una reserva.
/// Libera el asiento y promueve automáticamente al primer candidato en lista de espera.
/// </summary>
public sealed class PromoteFromWaitingListUseCase
{
    private readonly IFlightsRepository _flightsRepository;
    private readonly IWaitingListRepository _waitingListRepository;
    private readonly IBookingsRepository _bookingsRepository;
    private readonly IBookingStatuseRepository _statusesRepository;
    private readonly IReschedulingHistoryRepository _historyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PromoteFromWaitingListUseCase(
        IFlightsRepository flightsRepository,
        IWaitingListRepository waitingListRepository,
        IBookingsRepository bookingsRepository,
        IBookingStatuseRepository statusesRepository,
        IReschedulingHistoryRepository historyRepository,
        IUnitOfWork unitOfWork)
    {
        _flightsRepository     = flightsRepository;
        _waitingListRepository = waitingListRepository;
        _bookingsRepository    = bookingsRepository;
        _statusesRepository    = statusesRepository;
        _historyRepository     = historyRepository;
        _unitOfWork            = unitOfWork;
    }

    public async Task<int?> ExecuteAsync(int cancelledFlightId, CancellationToken cancellationToken = default)
    {
        // 1. Liberar el asiento en el vuelo
        var flight = await _flightsRepository.GetByIdAsync(FlightsId.Create(cancelledFlightId), cancellationToken)
            ?? throw new InvalidOperationException($"No existe el vuelo con id {cancelledFlightId}.");

        flight.IncrementAvailableSeats(1);
        await _flightsRepository.UpdateAsync(flight, cancellationToken);

        // 2. Buscar el primer candidato en lista de espera
        var candidate = await _waitingListRepository.GetNextCandidateAsync(cancelledFlightId, cancellationToken);
        if (candidate is null)
        {
            // No hay candidatos, solo guardamos la liberación del asiento
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return null;
        }

        // 3. Ocupar el asiento para el candidato
        flight.DecrementAvailableSeats(1);
        await _flightsRepository.UpdateAsync(flight, cancellationToken);

        // 4. Promover al candidato en la lista de espera
        candidate.Promote();
        await _waitingListRepository.UpdateAsync(candidate, cancellationToken);

        // 5. Actualizar estado de la reserva promovida a Confirmed
        var booking = await _bookingsRepository.GetByIdAsync(BookingId.Create(candidate.BookingId), cancellationToken);
        if (booking is not null)
        {
            var confirmedStatus = await _statusesRepository.GetByNameAsync("Confirmed");
            if (confirmedStatus is not null)
            {
                booking.ChangeStatus(confirmedStatus.Id.Value);
                booking.TouchUpdatedAt();
                await _bookingsRepository.UpdateAsync(booking, cancellationToken);
            }
        }

        // 6. Registrar el movimiento en historial
        var history = ReschedulingHistor.Create(
            candidate.BookingId,
            cancelledFlightId,
            cancelledFlightId,   // mismo vuelo — promovido desde espera
            "Promovido automáticamente desde lista de espera por liberación de cupo.");
        await _historyRepository.SaveAsync(history, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return candidate.BookingId;
    }
}