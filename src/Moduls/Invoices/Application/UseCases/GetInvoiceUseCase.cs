using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Application.UseCases;

public sealed class GetInvoiceUseCase
{
    private readonly IInvoicesRepository _repository;

    public GetInvoiceUseCase(IInvoicesRepository repository)
    {
        _repository = repository;
    }

    public async Task<Invoice> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByIdAsync(InvoicesId.Create(id));
        if (result is null)
            throw new KeyNotFoundException($"Factura con id '{id}' no encontrada.");

        return result;
    }
}
