using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Application.Services;

public sealed class InvoicesService : IInvoicesService
{
    private readonly IInvoicesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public InvoicesService(
        IInvoicesRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Invoice> CreateAsync(
        int id,
        int reservaId,
        string numeroFactura,
        decimal subtotal,
        decimal impuestos,
        decimal total,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByInvoiceNumberAsync(InvoicesNumber.Create(numeroFactura));
        if (existing is not null)
            throw new InvalidOperationException($"Ya existe una factura con el numero '{numeroFactura}'.");

        var existingByReservation = await _repository.GetByReservationIdAsync(BookingId.Create(reservaId));
        if (existingByReservation is not null)
            throw new InvalidOperationException($"Ya existe una factura para la reserva '{reservaId}'.");

        var invoice = Invoice.Create(id, reservaId, numeroFactura, DateTime.UtcNow, subtotal, impuestos, total);

        await _repository.SaveAsync(invoice);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return invoice;
    }

    public async Task<Invoice?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(InvoicesId.Create(id));
    }

    public async Task<Invoice?> GetByInvoiceNumberAsync(
        string numeroFactura,
        CancellationToken cancellationToken = default)
    {
        return await _repository.GetByInvoiceNumberAsync(InvoicesNumber.Create(numeroFactura));
    }

    public async Task<Invoice?> GetByReservationIdAsync(
        int reservaId,
        CancellationToken cancellationToken = default)
    {
        return await _repository.GetByReservationIdAsync(BookingId.Create(reservaId));
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(InvoicesId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(InvoicesId.Create(id));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}