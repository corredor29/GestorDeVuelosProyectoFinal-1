using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Infrastructure.Entity;

public class PaymentMethodsEntity
{
    public int Id { get; set; }
    public int PaymentMethodTypeId { get; set; }
    public int? CardTypeId { get; set; }
    public int? CardIssuerId { get; set; }
    public string DisplayName { get; set; } = null!;
    public PaymentMediumTypesEntity PaymentMethodType { get; set; } = null!;
    public CardTypesEntity? CardType { get; set; }
    public CardIssuerEntity? CardIssuer { get; set; }
    public ICollection<PaymentsEntity> Payments { get; set; } = new List<PaymentsEntity>();

}