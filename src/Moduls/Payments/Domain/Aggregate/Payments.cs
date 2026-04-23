using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.Aggregate;

public sealed class Payment
{
    public PaymentsId Id { get; private set; } = null!;
    public BookingId BookingId { get; private set; } = null!;
    public PaymentsAmount Amount { get; private set; } = null!;
    public PaymentsDate Date { get; private set; } = null!;
    public PaymentStatuseId PaymentStatusId { get; private set; } = null!;
    public PaymentMethodsId PaymentMethodId { get; private set; } = null!;
    public PaymentsCreatedAt CreatedAt { get; private set; } = null!;
    public PaymentsUpdatedAt UpdatedAt { get; private set; } = null!;

    private Payment() { }

    private Payment(
        PaymentsId id,
        BookingId bookingId,
        PaymentsAmount amount,
        PaymentsDate date,
        PaymentStatuseId paymentStatusId,
        PaymentMethodsId paymentMethodId,
        PaymentsCreatedAt createdAt,
        PaymentsUpdatedAt updatedAt)
    {
        Id              = id;
        BookingId       = bookingId;
        Amount          = amount;
        Date            = date;
        PaymentStatusId = paymentStatusId;
        PaymentMethodId = paymentMethodId;
        CreatedAt       = createdAt;
        UpdatedAt       = updatedAt;
    }

    public static Payment Create(
        int id,
        int bookingId,
        decimal amount,
        DateTime date,
        int paymentStatusId,
        int paymentMethodId,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new Payment(
            PaymentsId.Create(id),
            BookingId.Create(bookingId),
            PaymentsAmount.Create(amount),
            PaymentsDate.Create(date),
            PaymentStatuseId.Create(paymentStatusId),
            PaymentMethodsId.Create(paymentMethodId),
            PaymentsCreatedAt.Create(createdAt),
            PaymentsUpdatedAt.Create(updatedAt)
        );
    }

    public void SetId(int id) => Id = PaymentsId.Create(id);

    public void UpdateDate(DateTime newDate)
    {
        Date = PaymentsDate.Create(newDate);
    }
    public void UpdateAmount(decimal newAmount)
    {
        Amount = PaymentsAmount.Create(newAmount);
    }

    public void UpdatePaymentStatus(int newPaymentStatusId)
    {
        PaymentStatusId = PaymentStatuseId.Create(newPaymentStatusId);
    }

    public void UpdatePaymentMethod(int newPaymentMethodId)
    {
        PaymentMethodId = PaymentMethodsId.Create(newPaymentMethodId);
    }

    public void UpdateUpdatedAt()
    {
        UpdatedAt = PaymentsUpdatedAt.Create(DateTime.UtcNow);
    }
}