using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.Repositories;

public interface IPaymentStatusesRepository
{
    Task<Aggregate.PaymentStatuse?> GetByIdAsync(PaymentStatuseId id);
    Task<IEnumerable<Aggregate.PaymentStatuse>> GetAllAsync();
    Task SaveAsync(Aggregate.PaymentStatuse paymentStatus);
    Task UpdateAsync(Aggregate.PaymentStatuse paymentStatus);
    Task DeleteAsync(PaymentStatuseId id);
}