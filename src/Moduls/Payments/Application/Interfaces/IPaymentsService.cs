using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Payments.Application.Interfaces;

public interface IPaymentsService
{
    Task<Payment> CreateAsync(int id, int bookingId, decimal amount, DateTime paidAt, int paymentStatusId, int paymentMethodId, CancellationToken cancellationToken = default);
    Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}