using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Infrastructure.Entity;

public class PaymentMediumTypesEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<PaymentMethodsEntity> PaymentMethods { get; set; } = new List<PaymentMethodsEntity>();
}