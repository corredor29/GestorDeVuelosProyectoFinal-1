using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Application.UseCases;

public sealed class UpdateInvoiceItemTypeUseCase
{
    private readonly IInvoiceItemTypesRepository _repository;

    public UpdateInvoiceItemTypeUseCase(IInvoiceItemTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<InvoiceItemType> ExecuteAsync(
        int id,
        string newName,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(InvoiceItemTypesId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"InvoiceItemType with id '{id}' was not found.");

        var nameInUse = await _repository.GetByNameAsync(InvoiceItemTypesName.Create(newName));
        if (nameInUse is not null)
            throw new InvalidOperationException($"InvoiceItemType with name '{newName}' already exists.");

        existing.UpdateName(newName);

        await _repository.UpdateAsync(existing);

        return existing;
    }
}