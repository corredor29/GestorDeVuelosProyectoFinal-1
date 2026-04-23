using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Application.Interfaces;

public interface IInvoiceItemTypesService
{
    Task<InvoiceItemType> CreateAsync(int id, string name, CancellationToken cancellationToken = default);
    Task<InvoiceItemType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<InvoiceItemType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<InvoiceItemType> UpdateAsync(int id, string newName, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}