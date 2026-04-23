using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Application.UseCases;

public sealed class GetInvoiceItemTypeUseCase
{
    private readonly IInvoiceItemTypesRepository _repository;

    public GetInvoiceItemTypeUseCase(IInvoiceItemTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<InvoiceItemType> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByIdAsync(InvoiceItemTypesId.Create(id));
        if (result is null)
            throw new KeyNotFoundException($"InvoiceItemType with id '{id}' was not found.");

        return result;
    }
}