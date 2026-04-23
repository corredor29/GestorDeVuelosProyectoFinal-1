using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Application.Interfaces;

public interface IInvoiceItemsService
{
    Task<InvoiceItem> CreateAsync(int id, int facturaId, int tipoItemId, string descripcion, int cantidad, decimal precioUnitario, int? reservaPasajeroId = null, CancellationToken cancellationToken = default);
    Task<InvoiceItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<InvoiceItem>> GetByFacturaIdAsync(int facturaId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}