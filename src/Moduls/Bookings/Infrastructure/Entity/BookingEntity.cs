using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Infrastructure.Entity;

public sealed class BookingEntity
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public DateTime BookedAt { get; set; }
    public int BookingStatusId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public CustomerEntity Customer { get; set; } = null!;
    public BookingStatusesEntity BookingStatus { get; set; } = null!;
    public ICollection<BookingFlightsEntity> BookingFlights { get; set; } = new List<BookingFlightsEntity>();
    public ICollection<InvoicesEntity> Invoices { get; set; } = new List<InvoicesEntity>();
    public ICollection<PaymentsEntity> Payments { get; set; } = new List<PaymentsEntity>();
}
