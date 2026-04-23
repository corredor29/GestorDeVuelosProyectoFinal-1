using System.Collections.Generic;
using System.Threading.Tasks;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.Repositories;

public interface IInvoiceItemsRepository
{
    Task<InvoiceItem?> GetByIdAsync(InvoiceItemsId id);
    
    Task<IEnumerable<InvoiceItem>> GetByFacturaIdAsync(InvoicesId facturaId);

    Task SaveAsync(InvoiceItem item);
    Task DeleteAsync(InvoiceItemsId id);
}