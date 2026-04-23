using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Application.UseCases;

public sealed class CreateInvoiceItemUseCase
{
    private readonly IInvoiceItemsRepository _repository;

    public CreateInvoiceItemUseCase(IInvoiceItemsRepository repository)
    {
        _repository = repository;
    }

    public async Task<InvoiceItem> ExecuteAsync(
        int id,
        int facturaId,
        int tipoItemId,
        string descripcion,
        int cantidad,
        decimal precioUnitario,
        int? reservaPasajeroId = null,
        CancellationToken cancellationToken = default)
    {
        var subtotal = cantidad * precioUnitario;

        var item = InvoiceItem.Create(
            id,
            facturaId,
            tipoItemId,
            descripcion,
            cantidad,
            precioUnitario,
            subtotal,
            reservaPasajeroId
        );

        await _repository.SaveAsync(item);

        return item;
    }
}