using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Application.UseCases;

public sealed class CreateInvoiceItemTypeUseCase
{
    private readonly IInvoiceItemTypesRepository _repository;

    public CreateInvoiceItemTypeUseCase(IInvoiceItemTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<InvoiceItemType> ExecuteAsync(
        int id,
        string name,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByNameAsync(InvoiceItemTypesName.Create(name));
        if (existing is not null)
            throw new InvalidOperationException($"InvoiceItemType with name '{name}' already exists.");

        var invoiceItemType = InvoiceItemType.Create(id, name);

        await _repository.SaveAsync(invoiceItemType);

        return invoiceItemType;
    }
}