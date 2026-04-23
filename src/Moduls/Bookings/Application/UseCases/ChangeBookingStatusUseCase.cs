using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Application.UseCases;

public sealed class ChangeBookingStatusUseCase
{
    private readonly IBookingsRepository _bookingsRepository;
    private readonly IBookingStatuseRepository _bookingStatusesRepository;
    private readonly IBookingStatusTransitionRepository _transitionsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeBookingStatusUseCase(
        IBookingsRepository bookingsRepository,
        IBookingStatuseRepository bookingStatusesRepository,
        IBookingStatusTransitionRepository transitionsRepository,
        IUnitOfWork unitOfWork)
    {
        _bookingsRepository = bookingsRepository;
        _bookingStatusesRepository = bookingStatusesRepository;
        _transitionsRepository = transitionsRepository;
        _unitOfWork = unitOfWork;
    }

    public Task ConfirmAsync(int bookingId, CancellationToken cancellationToken = default)
        => ExecuteAsync(bookingId, "Confirmed", cancellationToken);

    public Task CancelAsync(int bookingId, CancellationToken cancellationToken = default)
        => ExecuteAsync(bookingId, "Cancelled", cancellationToken);

    private async Task ExecuteAsync(int bookingId, string targetStatusName, CancellationToken cancellationToken)
    {
        var booking = await _bookingsRepository.GetByIdAsync(BookingId.Create(bookingId), cancellationToken);
        if (booking is null)
            throw new InvalidOperationException($"No existe la reserva con id {bookingId}.");

        var targetStatus = await _bookingStatusesRepository.GetByNameAsync(targetStatusName);
        if (targetStatus is null)
            throw new InvalidOperationException($"No existe el estado de reserva '{targetStatusName}'.");

        if (booking.BookingStatusId.Value == targetStatus.Id.Value)
            return;

        var allowed = await _transitionsRepository.IsTransitionAllowedAsync(
            booking.BookingStatusId.Value,
            targetStatus.Id.Value,
            cancellationToken);

        if (!allowed)
            throw new InvalidOperationException(
                $"Transicion de estado no permitida: {booking.BookingStatusId.Value} -> {targetStatus.Id.Value}.");

        booking.ChangeStatus(targetStatus.Id.Value);
        booking.TouchUpdatedAt();

        await _bookingsRepository.UpdateAsync(booking, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
