using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Aggregate;

public sealed class Booking
{
    public BookingId? Id { get; private set; }
    public CustomersId ClientId { get; private set; } = null!;
    public BookingBookedAt BookedAt { get; private set; } = null!;
    public BookingStatusesId BookingStatusId { get; private set; } = null!;
    public BookingTotalAmount TotalAmount { get; private set; } = null!;
    public BookingExpiresAt ExpiresAt { get; private set; } = null!;
    public BookingCreatedAt CreatedAt { get; private set; } = null!;
    public BookingUpdatedAt UpdatedAt { get; private set; } = null!;

    private Booking() { }

    private Booking(
        BookingId? id,
        CustomersId clientId,
        BookingBookedAt bookedAt,
        BookingStatusesId bookingStatusId,
        BookingTotalAmount totalAmount,
        BookingExpiresAt expiresAt,
        BookingCreatedAt createdAt,
        BookingUpdatedAt updatedAt)
    {
        Id = id;
        ClientId = clientId;
        BookedAt = bookedAt;
        BookingStatusId = bookingStatusId;
        TotalAmount = totalAmount;
        ExpiresAt = expiresAt;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Booking Create(
        int clientId,
        DateTime bookedAt,
        int bookingStatusId,
        decimal totalAmount,
        DateTime? expiresAt)
    {
        ValidateExpiration(bookedAt, expiresAt);

        var now = DateTime.UtcNow;
        return new Booking(
            id: null,
            clientId: CustomersId.Create(clientId),
            bookedAt: BookingBookedAt.Create(bookedAt),
            bookingStatusId: BookingStatusesId.Create(bookingStatusId),
            totalAmount: BookingTotalAmount.Create(totalAmount),
            expiresAt: BookingExpiresAt.Create(expiresAt),
            createdAt: BookingCreatedAt.Create(now),
            updatedAt: BookingUpdatedAt.Create(now));
    }

    public static Booking FromPrimitives(
        int id,
        int clientId,
        DateTime bookedAt,
        int bookingStatusId,
        decimal totalAmount,
        DateTime? expiresAt,
        DateTime createdAt,
        DateTime updatedAt)
    {
        ValidateExpiration(bookedAt, expiresAt);

        return new Booking(
            id: BookingId.Create(id),
            clientId: CustomersId.Create(clientId),
            bookedAt: BookingBookedAt.Create(bookedAt),
            bookingStatusId: BookingStatusesId.Create(bookingStatusId),
            totalAmount: BookingTotalAmount.Create(totalAmount),
            expiresAt: BookingExpiresAt.Create(expiresAt),
            createdAt: BookingCreatedAt.Create(createdAt),
            updatedAt: BookingUpdatedAt.Create(updatedAt));
    }

    public void Update(
        int clientId,
        DateTime bookedAt,
        int bookingStatusId,
        decimal totalAmount,
        DateTime? expiresAt)
    {
        ValidateExpiration(bookedAt, expiresAt);

        ClientId = CustomersId.Create(clientId);
        BookedAt = BookingBookedAt.Create(bookedAt);
        BookingStatusId = BookingStatusesId.Create(bookingStatusId);
        TotalAmount = BookingTotalAmount.Create(totalAmount);
        ExpiresAt = BookingExpiresAt.Create(expiresAt);
    }

    public void ChangeStatus(int bookingStatusId)
    {
        BookingStatusId = BookingStatusesId.Create(bookingStatusId);
    }

    public void TouchUpdatedAt()
    {
        UpdatedAt = BookingUpdatedAt.Create(DateTime.UtcNow);
    }

    private static void ValidateExpiration(DateTime bookedAt, DateTime? expiresAt)
    {
        if (expiresAt.HasValue && expiresAt.Value < bookedAt)
            throw new ArgumentException("La fecha de expiracion no puede ser menor a booked_at.");
    }
}
