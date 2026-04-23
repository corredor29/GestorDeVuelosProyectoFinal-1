using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Application.Services;

public sealed class PaymentStatusesService : IPaymentStatusesService
{
    private readonly IPaymentStatusesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentStatusesService(
        IPaymentStatusesRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PaymentStatuse> CreateAsync(
        int id,
        string name,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(PaymentStatuseId.Create(id));
        if (existing is not null)
            throw new InvalidOperationException($"PaymentStatus with id '{id}' already exists.");

        var paymentStatus = PaymentStatuse.Create(id, name);

        await _repository.SaveAsync(paymentStatus);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return paymentStatus;
    }

    public async Task<PaymentStatuse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(PaymentStatuseId.Create(id));
    }

    public Task<IEnumerable<PaymentStatuse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }

    public async Task<PaymentStatuse> UpdateAsync(
        int id,
        string? newName,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(PaymentStatuseId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"PaymentStatus with id '{id}' was not found.");

        if (newName is not null)
            existing.UpdateName(newName);

        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(PaymentStatuseId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(PaymentStatuseId.Create(id));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}