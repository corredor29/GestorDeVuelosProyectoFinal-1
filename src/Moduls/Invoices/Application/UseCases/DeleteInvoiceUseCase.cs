using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Application.UseCases;

public sealed class DeleteInvoiceUseCase
{
    private readonly IInvoicesRepository _repository;

    public DeleteInvoiceUseCase(IInvoicesRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(InvoicesId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(InvoicesId.Create(id));

        return true;
    }
}