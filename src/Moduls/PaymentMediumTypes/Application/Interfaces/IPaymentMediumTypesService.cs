using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Application.Interfaces;

public interface IPaymentMediumTypesService
{
    Task<PaymentMediumType> CreateAsync(int id, string name, CancellationToken cancellationToken = default);
    Task<PaymentMediumType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentMediumType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PaymentMediumType> UpdateAsync(int id, string? newName, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
