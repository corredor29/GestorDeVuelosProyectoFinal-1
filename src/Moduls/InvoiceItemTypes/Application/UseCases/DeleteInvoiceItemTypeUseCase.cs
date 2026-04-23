using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Application.UseCases;

public sealed class DeleteInvoiceItemTypeUseCase
{
    private readonly IInvoiceItemTypesRepository _repository;

    public DeleteInvoiceItemTypeUseCase(IInvoiceItemTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(InvoiceItemTypesId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(InvoiceItemTypesId.Create(id));

        return true;
    }
}
