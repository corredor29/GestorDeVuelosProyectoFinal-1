using System;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Domain.Aggregate;

public sealed class InvoiceItem
{
    public InvoiceItemsId Id { get; private set; } = null!;
    public InvoicesId FacturaId { get; private set; } = null!;
    public InvoiceItemTypesId TipoItemId { get; private set; } = null!;
    public InvoiceItemsDescription Descripcion { get; private set; } = null!;
    public InvoiceItemsQuantity Cantidad { get; private set; } = null!;
    public InvoiceItemsPrice PrecioUnitario { get; private set; } = null!;
    public InvoiceItemsPrice Subtotal { get; private set; } = null!;
    
    public int? ReservaPasajeroId { get; private set; }

    private InvoiceItem() { }

    public static InvoiceItem Create(
        int id,
        int facturaId,
        int tipoItemId,
        string descripcion,
        int cantidad,
        decimal precioUnitario,
        decimal subtotal,
        int? reservaPasajeroId = null)
    {
        return new InvoiceItem
        {
            Id = InvoiceItemsId.Create(id),
            FacturaId = InvoicesId.Create(facturaId),
            TipoItemId = InvoiceItemTypesId.Create(tipoItemId),
            Descripcion = InvoiceItemsDescription.Create(descripcion),
            Cantidad = InvoiceItemsQuantity.Create(cantidad),
            PrecioUnitario = InvoiceItemsPrice.Create(precioUnitario, "precio unitario"),
            Subtotal = InvoiceItemsPrice.Create(subtotal, "subtotal"),
            ReservaPasajeroId = reservaPasajeroId
        };
    }
    internal void SetId(int id) => Id = InvoiceItemsId.Create(id);
}
