using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Infrastructure.Entity;

public class CardIssuerEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string IssuerNumber { get; set; } = null!;
    public ICollection<PaymentMethodsEntity> PaymentMethods { get; set; } = new List<PaymentMethodsEntity>();
}