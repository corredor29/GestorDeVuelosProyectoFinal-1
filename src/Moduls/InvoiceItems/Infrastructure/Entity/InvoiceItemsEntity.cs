using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Infrastructure.Entity;

public class InvoiceItemsEntity
{
    public int Id { get; set; }
    public int Invoice_Id { get; set; }
    public int Item_Type_Id { get; set; }
    public string Description { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
    public int? BookingPassenger_Id { get; set; }
    public InvoicesEntity Invoice { get; set; } = null!;
    public InvoiceItemTypesEntity ItemType { get; set; } = null!;
}
