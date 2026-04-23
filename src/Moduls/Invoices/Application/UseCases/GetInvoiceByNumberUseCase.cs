using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Application.UseCases;

public sealed class GetInvoiceByNumberUseCase
{
    private readonly IInvoicesRepository _repository;

    public GetInvoiceByNumberUseCase(IInvoicesRepository repository)
    {
        _repository = repository;
    }

    public async Task<Invoice> ExecuteAsync(
        string numeroFactura,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByInvoiceNumberAsync(InvoicesNumber.Create(numeroFactura));
        if (result is null)
            throw new KeyNotFoundException($"Factura con numero '{numeroFactura}' no encontrada.");

        return result;
    }
}