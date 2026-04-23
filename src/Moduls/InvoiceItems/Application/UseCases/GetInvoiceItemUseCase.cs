using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Application.UseCases;

public sealed class GetInvoiceItemUseCase
{
    private readonly IInvoiceItemsRepository _repository;

    public GetInvoiceItemUseCase(IInvoiceItemsRepository repository)
    {
        _repository = repository;
    }

    public async Task<InvoiceItem> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByIdAsync(InvoiceItemsId.Create(id));
        if (result is null)
            throw new KeyNotFoundException($"Item de factura con id '{id}' no encontrado.");

        return result;
    }
}