using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Infrastructure.Entity;

public class InvoicesEntity
{
    public int Id { get; set; }
    public int Booking_Id { get; set; }
    public string InvoiceNumber { get; set; } = null!;
    public DateTime IssuedAt { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Taxes { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public BookingEntity Booking { get; set; } = null!;
    public ICollection<InvoiceItemsEntity> InvoiceItems { get; set; } = new List<InvoiceItemsEntity>();
}