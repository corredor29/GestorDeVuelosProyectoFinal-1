using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.Repositories;

public interface IPaymentMediumTypesRepository
{
    Task<Aggregate.PaymentMediumType?> GetByIdAsync(PaymentMediumTypesId id);
    Task<Aggregate.PaymentMediumType?> GetByNameAsync(PaymentMediumTypesName name);
    Task<IEnumerable<Aggregate.PaymentMediumType>> GetAllAsync();
    Task SaveAsync(Aggregate.PaymentMediumType paymentMediumTypes);
    Task UpdateAsync(Aggregate.PaymentMediumType paymentMediumTypes);
    Task DeleteAsync(PaymentMediumTypesId id);
}