using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Application.UseCases;

public sealed class UpdatePaymentStatusUseCase
{
    private readonly IPaymentStatusesRepository _repository;

    public UpdatePaymentStatusUseCase(IPaymentStatusesRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaymentStatuse> ExecuteAsync(
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

        return existing;
    }
}