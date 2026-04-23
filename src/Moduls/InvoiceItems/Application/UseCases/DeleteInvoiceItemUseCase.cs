using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Application.UseCases;

public sealed class DeleteInvoiceItemUseCase
{
    private readonly IInvoiceItemsRepository _repository;

    public DeleteInvoiceItemUseCase(IInvoiceItemsRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(InvoiceItemsId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(InvoiceItemsId.Create(id));

        return true;
    }
}
