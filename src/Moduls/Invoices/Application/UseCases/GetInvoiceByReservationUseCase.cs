using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Application.UseCases;

public sealed class GetInvoiceByReservationUseCase
{
    private readonly IInvoicesRepository _repository;

    public GetInvoiceByReservationUseCase(IInvoicesRepository repository)
    {
        _repository = repository;
    }

    public async Task<Invoice> ExecuteAsync(
        int reservaId,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByReservationIdAsync(BookingId.Create(reservaId));
        if (result is null)
            throw new KeyNotFoundException($"Factura para la reserva '{reservaId}' no encontrada.");

        return result;
    }
}