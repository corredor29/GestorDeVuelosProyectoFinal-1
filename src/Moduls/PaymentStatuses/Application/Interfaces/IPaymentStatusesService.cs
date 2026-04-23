using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Application.Interfaces;

public interface IPaymentStatusesService
{
    Task<PaymentStatuse> CreateAsync(int id, string name, CancellationToken cancellationToken = default);
    Task<PaymentStatuse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentStatuse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PaymentStatuse> UpdateAsync(int id, string? newName, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}