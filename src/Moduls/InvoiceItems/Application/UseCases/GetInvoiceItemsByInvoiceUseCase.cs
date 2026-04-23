using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Application.UseCases;

public sealed class GetInvoiceItemsByInvoiceUseCase
{
    private readonly IInvoiceItemsRepository _repository;

    public GetInvoiceItemsByInvoiceUseCase(IInvoiceItemsRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<InvoiceItem>> ExecuteAsync(
        int facturaId,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetByFacturaIdAsync(InvoicesId.Create(facturaId));
    }
}