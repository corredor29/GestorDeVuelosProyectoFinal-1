using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;


namespace GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Application.UseCases;

public sealed class CreateInvoiceUseCase
{
    private readonly IInvoicesRepository _repository;

    public CreateInvoiceUseCase(IInvoicesRepository repository)
    {
        _repository = repository;
    }

    public async Task<Invoice> ExecuteAsync(
        int id,
        int bookingId,
        string numeroFactura,
        decimal subtotal,
        decimal impuestos,
        decimal total,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByInvoiceNumberAsync(InvoicesNumber.Create(numeroFactura));
        if (existing is not null)
            throw new InvalidOperationException($"Ya existe una factura con el numero '{numeroFactura}'.");

        var existingByReservation = await _repository.GetByReservationIdAsync(BookingId.Create(bookingId));
        if (existingByReservation is not null)
            throw new InvalidOperationException($"Ya existe una factura para la reserva '{bookingId}'.");

        var invoice = Invoice.Create(id, bookingId, numeroFactura, DateTime.UtcNow, subtotal, impuestos, total);

        await _repository.SaveAsync(invoice);

        return invoice;
    }
}