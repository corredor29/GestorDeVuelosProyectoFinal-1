using System;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.ValueObject;

using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.Aggregate;

public sealed class Invoice
{
    public InvoicesId Id { get; private set; } = null!;

    public BookingId ReservaId { get; private set; } = null!; 
    public InvoicesNumber NumeroFactura { get; private set; } = null!;
    public InvoicesDate FechaEmision { get; private set; } = null!;
    public InvoicesAmount Subtotal { get; private set; } = null!;
    public InvoicesAmount Impuestos { get; private set; } = null!;
    public InvoicesAmount Total { get; private set; } = null!;

    private Invoice() { }

    public static Invoice Create(
        int id,
        int reservaId,
        string numeroFactura,
        DateTime fechaEmision,
        decimal subtotal,
        decimal impuestos,
        decimal total)
    {
        return new Invoice
        {
            Id = InvoicesId.Create(id),
            ReservaId = BookingId.Create(reservaId), // Ajustado aquí también
            NumeroFactura = InvoicesNumber.Create(numeroFactura),
            FechaEmision = InvoicesDate.Create(fechaEmision),
            Subtotal = InvoicesAmount.Create(subtotal, "subtotal"),
            Impuestos = InvoicesAmount.Create(impuestos, "impuestos"),
            Total = InvoicesAmount.Create(total, "total")
        };
    }
    internal void SetId(int id) => Id = InvoicesId.Create(id);
}
