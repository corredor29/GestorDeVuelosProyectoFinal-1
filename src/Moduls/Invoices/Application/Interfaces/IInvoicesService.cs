using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Application.Interfaces;

public interface IInvoicesService
{
    Task<Invoice> CreateAsync(int id, int bookingId, string numeroFactura, decimal subtotal, decimal impuestos, decimal total, CancellationToken cancellationToken = default);
    Task<Invoice?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Invoice?> GetByInvoiceNumberAsync(string numeroFactura, CancellationToken cancellationToken = default);
    Task<Invoice?> GetByReservationIdAsync(int bookingId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}