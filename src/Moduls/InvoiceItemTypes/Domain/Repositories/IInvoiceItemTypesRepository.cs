using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.Repositories;

public interface IInvoiceItemTypesRepository
{
    Task<InvoiceItemType?> GetByIdAsync(InvoiceItemTypesId id);
    Task<InvoiceItemType?> GetByNameAsync(InvoiceItemTypesName name);
    Task<IEnumerable<InvoiceItemType>> GetAllAsync();
    Task SaveAsync(InvoiceItemType invoiceItemType);
    Task UpdateAsync(InvoiceItemType invoiceItemType);
    Task DeleteAsync(InvoiceItemTypesId id);
}