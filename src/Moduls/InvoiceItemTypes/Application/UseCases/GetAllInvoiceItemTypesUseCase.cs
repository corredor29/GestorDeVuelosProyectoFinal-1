using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Application.UseCases;

public sealed class GetAllInvoiceItemTypesUseCase
{
    private readonly IInvoiceItemTypesRepository _repository;

    public GetAllInvoiceItemTypesUseCase(IInvoiceItemTypesRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<InvoiceItemType>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }
}