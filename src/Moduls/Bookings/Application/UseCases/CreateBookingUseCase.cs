using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Application.UseCases;

public sealed class CreateBookingUseCase
{
    private readonly IBookingsRepository _repository;
    private readonly ICustomersRepository _customersRepository;
    private readonly IBookingStatuseRepository _bookingStatusesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookingUseCase(
        IBookingsRepository repository,
        ICustomersRepository customersRepository,
        IBookingStatuseRepository bookingStatusesRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _customersRepository = customersRepository;
        _bookingStatusesRepository = bookingStatusesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(
        int clientId,
        DateTime bookedAt,
        int bookingStatusId,
        decimal totalAmount,
        DateTime? expiresAt,
        CancellationToken cancellationToken = default)
    {
        await EnsureReferencesExistAsync(clientId, bookingStatusId);

        var booking = Booking.Create(clientId, bookedAt, bookingStatusId, totalAmount, expiresAt);
        await _repository.SaveAsync(booking, cancellationToken);
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
