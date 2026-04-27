using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Application.UseCases;
using DomainBooking = GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Aggregate.Booking;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Application.Services;

public sealed class BookingsService : IBookingsService
{
    private readonly GetBookingsUseCase _get;
    private readonly CreateBookingUseCase _create;
    private readonly UpdateBookingUseCase _update;
    private readonly DeleteBookingUseCase _delete;
    private readonly ChangeBookingStatusUseCase _changeStatus;

    private readonly RescheduleBookingUseCase _reschedule;
    private readonly AddToWaitingListUseCase _addToWaitingList;
    private readonly PromoteFromWaitingListUseCase _promote;

    public BookingsService(
        GetBookingsUseCase get,
        CreateBookingUseCase create,
        UpdateBookingUseCase update,
        DeleteBookingUseCase delete,
        ChangeBookingStatusUseCase changeStatus,
        RescheduleBookingUseCase reschedule,
        AddToWaitingListUseCase addToWaitingList,
        PromoteFromWaitingListUseCase promote)
    {
        _get              = get;
        _create           = create;
        _update           = update;
        _delete           = delete;
        _changeStatus     = changeStatus;
        _reschedule       = reschedule;
        _addToWaitingList = addToWaitingList;
        _promote          = promote;
    }

    public Task<IEnumerable<DomainBooking>> GetAllAsync(CancellationToken cancellationToken = default)
        => _get.GetAllAsync(cancellationToken);

    public Task<DomainBooking?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _get.GetByIdAsync(id, cancellationToken);

    public Task<IEnumerable<DomainBooking>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default)
        => _get.GetByClientIdAsync(clientId, cancellationToken);

    public Task<IEnumerable<DomainBooking>> GetByStatusIdAsync(int statusId, CancellationToken cancellationToken = default)
        => _get.GetByStatusIdAsync(statusId, cancellationToken);

    public Task<IEnumerable<DomainBooking>> GetByBookedAtRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
        => _get.GetByBookedAtRangeAsync(from, to, cancellationToken);

    public Task CreateAsync(int clientId, DateTime bookedAt, int bookingStatusId, decimal totalAmount, DateTime? expiresAt, CancellationToken cancellationToken = default)
        => _create.ExecuteAsync(clientId, bookedAt, bookingStatusId, totalAmount, expiresAt, cancellationToken);

    public Task UpdateAsync(int id, int clientId, DateTime bookedAt, int bookingStatusId, decimal totalAmount, DateTime? expiresAt, CancellationToken cancellationToken = default)
        => _update.ExecuteAsync(id, clientId, bookedAt, bookingStatusId, totalAmount, expiresAt, cancellationToken);

    public Task ConfirmAsync(int bookingId, CancellationToken cancellationToken = default)
        => _changeStatus.ConfirmAsync(bookingId, cancellationToken);

    public Task CancelAsync(int bookingId, CancellationToken cancellationToken = default)
        => _changeStatus.CancelAsync(bookingId, cancellationToken);

    public Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
        => _delete.ExecuteByIdAsync(id, cancellationToken);

    public Task DeleteByClientIdAsync(int clientId, CancellationToken cancellationToken = default)
        => _delete.ExecuteByClientIdAsync(clientId, cancellationToken);

    public Task<bool> RescheduleAsync(int bookingId, int currentFlightId, int newFlightId, string reason, CancellationToken cancellationToken = default)
        => _reschedule.ExecuteAsync(bookingId, currentFlightId, newFlightId, reason, cancellationToken);

    public Task AddToWaitingListAsync(int bookingId, int flightId, int priority = 0, CancellationToken cancellationToken = default)
        => _addToWaitingList.ExecuteAsync(bookingId, flightId, priority, cancellationToken);

    public Task<int?> PromoteFromWaitingListAsync(int cancelledFlightId, CancellationToken cancellationToken = default)
        => _promote.ExecuteAsync(cancelledFlightId, cancellationToken);
}