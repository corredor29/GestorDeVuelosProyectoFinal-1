using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Application.Services;

public sealed class PaymentMediumTypesService : IPaymentMediumTypesService
{
    private readonly IPaymentMediumTypesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentMediumTypesService(
        IPaymentMediumTypesRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PaymentMediumType> CreateAsync(
        int id,
        string name,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByNameAsync(PaymentMediumTypesName.Create(name));
        if (existing is not null)
            throw new InvalidOperationException($"PaymentMediumType with name '{name}' already exists.");

        var paymentMediumType = PaymentMediumType.Create(id, name);

        await _repository.SaveAsync(paymentMediumType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return paymentMediumType;
    }

    public async Task<PaymentMediumType?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(PaymentMediumTypesId.Create(id));
    }

    public Task<IEnumerable<PaymentMediumType>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }

    public async Task<PaymentMediumType> UpdateAsync(
        int id,
        string? newName,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(PaymentMediumTypesId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"PaymentMediumType with id '{id}' was not found.");

        if (newName is not null)
        {
            var nameInUse = await _repository.GetByNameAsync(PaymentMediumTypesName.Create(newName));
            if (nameInUse is not null)
                throw new InvalidOperationException($"PaymentMediumType with name '{newName}' already exists.");

            existing.UpdateName(newName);
        }

        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(PaymentMediumTypesId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(PaymentMediumTypesId.Create(id));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}