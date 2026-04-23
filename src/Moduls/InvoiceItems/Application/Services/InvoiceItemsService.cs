using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Application.Services;

public sealed class InvoiceItemsService : IInvoiceItemsService
{
    private readonly IInvoiceItemsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public InvoiceItemsService(
        IInvoiceItemsRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<InvoiceItem> CreateAsync(
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
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return item;
    }

    public async Task<InvoiceItem?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(InvoiceItemsId.Create(id));
    }

    public Task<IEnumerable<InvoiceItem>> GetByFacturaIdAsync(
        int facturaId,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetByFacturaIdAsync(InvoicesId.Create(facturaId));
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(InvoiceItemsId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(InvoiceItemsId.Create(id));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
