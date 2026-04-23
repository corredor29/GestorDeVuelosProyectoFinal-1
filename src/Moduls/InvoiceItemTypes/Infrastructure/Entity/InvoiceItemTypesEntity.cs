using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Infrastructure.Entity;

public class InvoiceItemTypesEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<InvoiceItemsEntity> InvoiceItems { get; set; } = new List<InvoiceItemsEntity>();

}
