using System;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.Repositories;

public interface IPaymentMethodsRepository
{
    Task<Aggregate.PaymentMethod?> GetByIdAsync(PaymentMethodsId id);
    Task<IEnumerable<Aggregate.PaymentMethod>> GetAllAsync();
    Task UpdateAsync(PaymentMethod paymentMethod); 

    Task SaveAsync(Aggregate.PaymentMethod paymentMethod);    
    Task DeleteAsync(PaymentMethodsId id);
}
