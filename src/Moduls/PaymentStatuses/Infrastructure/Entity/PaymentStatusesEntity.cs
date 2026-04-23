using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Infrastructure.Entity;

public class PaymentStatusesEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<PaymentsEntity> Payments { get; set; } = new List<PaymentsEntity>();
}