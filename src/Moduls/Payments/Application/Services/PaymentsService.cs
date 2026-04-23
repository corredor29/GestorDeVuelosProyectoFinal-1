using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Payments.Application.Services;

public sealed class PaymentsService : IPaymentsService
{
    private readonly IPaymentsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentsService(
        IPaymentsRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Payment> CreateAsync(
        int id,
        int bookingId,
        decimal amount,
        DateTime paidAt,
        int paymentStatusId,
        int paymentMethodId,
        CancellationToken cancellationToken = default)
    {
        var payment = Payment.Create(id, bookingId, amount, paidAt, paymentStatusId, paymentMethodId, DateTime.UtcNow, DateTime.UtcNow);

        await _repository.SaveAsync(payment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return payment;
    }

    public async Task<Payment?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(PaymentsId.Create(id));
    }

    public Task<IEnumerable<Payment>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(PaymentsId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(PaymentsId.Create(id));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}