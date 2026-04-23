using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Payments.Application.UseCases;

public sealed class UpdatePaymentUseCase
{
    private readonly IPaymentsRepository _repository;

    public UpdatePaymentUseCase(IPaymentsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Payment> ExecuteAsync(
        int id,
        decimal newAmount,
        DateTime newDate,
        int newPaymentStatusId,
        int newPaymentMethodId,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(PaymentsId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"Payment with id '{id}' was not found.");

        existing.UpdateAmount(newAmount);
        existing.UpdateDate(newDate);
        existing.UpdatePaymentStatus(newPaymentStatusId);
        existing.UpdatePaymentMethod(newPaymentMethodId);

        await _repository.UpdateAsync(existing);

        return existing;
    }
}