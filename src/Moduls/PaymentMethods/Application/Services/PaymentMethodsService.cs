using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Application.Services;

public sealed class PaymentMethodsService : IPaymentMethodsService
{
    private readonly IPaymentMethodsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentMethodsService(
        IPaymentMethodsRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PaymentMethod> CreateAsync(
        int id,
        string displayName,
        CancellationToken cancellationToken = default)
    {
        var paymentMethod = PaymentMethod.Create(id, displayName);

        await _repository.SaveAsync(paymentMethod);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return paymentMethod;
    }

    public async Task<PaymentMethod?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(PaymentMethodsId.Create(id));
    }

    public Task<IEnumerable<PaymentMethod>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }

    public async Task<PaymentMethod> UpdateAsync(
        int id,
        string newDisplayName,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(PaymentMethodsId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"PaymentMethod with id '{id}' was not found.");

        existing.UpdateDisplayName(newDisplayName);

        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(PaymentMethodsId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(PaymentMethodsId.Create(id));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}