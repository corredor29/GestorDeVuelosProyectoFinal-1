using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.WaitingList.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.WaitingList.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Application.UseCases;

public sealed class AddToWaitingListUseCase
{
    private readonly IBookingsRepository _bookingsRepository;
    private readonly IBookingStatuseRepository _statusesRepository;
    private readonly IFlightsRepository _flightsRepository;
    private readonly IWaitingListRepository _waitingListRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddToWaitingListUseCase(
        IBookingsRepository bookingsRepository,
        IBookingStatuseRepository statusesRepository,
        IFlightsRepository flightsRepository,
        IWaitingListRepository waitingListRepository,
        IUnitOfWork unitOfWork)
    {
        _bookingsRepository    = bookingsRepository;
        _statusesRepository    = statusesRepository;
        _flightsRepository     = flightsRepository;
        _waitingListRepository = waitingListRepository;
        _unitOfWork            = unitOfWork;
    }

    public async Task ExecuteAsync(
        int bookingId,
        int flightId,
        int priority = 0,
        CancellationToken cancellationToken = default)
    {
        // 1. Verificar que la reserva exista
        var booking = await _bookingsRepository.GetByIdAsync(BookingId.Create(bookingId), cancellationToken)
            ?? throw new InvalidOperationException($"No existe la reserva con id {bookingId}.");

        // 2. Verificar que el vuelo exista
        _ = await _flightsRepository.GetByIdAsync(FlightsId.Create(flightId), cancellationToken)
            ?? throw new InvalidOperationException($"No existe el vuelo con id {flightId}.");

        // 3. Verificar que no esté ya en espera para este vuelo
        var alreadyWaiting = await _waitingListRepository.ExistsAsync(bookingId, flightId, cancellationToken);
        if (alreadyWaiting)
            throw new InvalidOperationException($"La reserva {bookingId} ya está en lista de espera para el vuelo {flightId}.");

        // 4. Registrar en lista de espera
        var entry = WaitingLis.Create(bookingId, flightId, priority);
        await _waitingListRepository.SaveAsync(entry, cancellationToken);

        // 5. Cambiar estado de la reserva a "En espera" si existe ese estado
        var waitingStatus = await _statusesRepository.GetByNameAsync("OnHold");
        if (waitingStatus is not null && booking.BookingStatusId.Value != waitingStatus.Id.Value)
        {
            booking.ChangeStatus(waitingStatus.Id.Value);
            booking.TouchUpdatedAt();
            await _bookingsRepository.UpdateAsync(booking, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}