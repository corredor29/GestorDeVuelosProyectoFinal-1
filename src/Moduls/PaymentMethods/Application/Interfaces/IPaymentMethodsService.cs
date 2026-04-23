using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Application.Interfaces;

public interface IPaymentMethodsService
{
    Task<PaymentMethod> CreateAsync(int id, string displayName, CancellationToken cancellationToken = default);
    Task<PaymentMethod?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentMethod>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PaymentMethod> UpdateAsync(int id, string newDisplayName, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}