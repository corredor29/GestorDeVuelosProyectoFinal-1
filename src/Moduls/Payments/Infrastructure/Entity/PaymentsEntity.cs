using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.Payments.Infrastructure.Entity;

public class PaymentsEntity
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
    public int PaymentStatusId { get; set; }
    public int PaymentMethodId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public BookingEntity Booking { get; set; } = null!;
    public PaymentStatusesEntity PaymentStatus { get; set; } = null!;
    public PaymentMethodsEntity PaymentMethod { get; set; } = null!;
}