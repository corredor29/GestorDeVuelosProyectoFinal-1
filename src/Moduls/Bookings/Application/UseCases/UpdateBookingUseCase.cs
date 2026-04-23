using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Application.UseCases;

public sealed class UpdateBookingUseCase
{
    private readonly IBookingsRepository _repository;
    private readonly ICustomersRepository _customersRepository;
    private readonly IBookingStatuseRepository _bookingStatusesRepository;
    private readonly IBookingStatusTransitionRepository _transitionsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBookingUseCase(
        IBookingsRepository repository,
        ICustomersRepository customersRepository,
        IBookingStatuseRepository bookingStatusesRepository,
        IBookingStatusTransitionRepository transitionsRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _customersRepository = customersRepository;
        _bookingStatusesRepository = bookingStatusesRepository;
        _transitionsRepository = transitionsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(
        int id,
        int clientId,
        DateTime bookedAt,
        int bookingStatusId,
        decimal totalAmount,
        DateTime? expiresAt,
        CancellationToken cancellationToken = default)
    {
        var booking = await _repository.GetByIdAsync(BookingId.Create(id), cancellationToken);
        if (booking is null)
            throw new InvalidOperationException($"No existe la reserva con id {id}.");

        await EnsureReferencesExistAsync(clientId, bookingStatusId);

        if (booking.BookingStatusId.Value != bookingStatusId)
        {
            var allowed = await _transitionsRepository.IsTransitionAllowedAsync(
                booking.BookingStatusId.Value,
                bookingStatusId,
                cancellationToken);

            if (!allowed)
                throw new InvalidOperationException(
                    $"Transicion de estado no permitida: {booking.BookingStatusId.Value} -> {bookingStatusId}.");
        }

        booking.Update(clientId, bookedAt, bookingStatusId, totalAmount, expiresAt);
        booking.TouchUpdatedAt();

        await _repository.UpdateAsync(booking, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureReferencesExistAsync(int clientId, int bookingStatusId)
    {
        if (await _customersRepository.GetByIdAsync(CustomersId.Create(clientId)) is null)
            throw new InvalidOperationException($"No existe el cliente con id {clientId}.");

        if (await _bookingStatusesRepository.GetByIdAsync(BookingStatusesId.Create(bookingStatusId)) is null)
            throw new InvalidOperationException($"No existe el estado de reserva con id {bookingStatusId}.");
    }
}
