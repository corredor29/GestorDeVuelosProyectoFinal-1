using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Application.UseCases;

public sealed class CreatePaymentStatusUseCase
{
    private readonly IPaymentStatusesRepository _repository;

    public CreatePaymentStatusUseCase(IPaymentStatusesRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaymentStatuse> ExecuteAsync(
        int id,
        string name,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(PaymentStatuseId.Create(id));
        if (existing is not null)
            throw new InvalidOperationException($"PaymentStatus with id '{id}' already exists.");

        var paymentStatus = PaymentStatuse.Create(id, name);

        await _repository.SaveAsync(paymentStatus); 

        return paymentStatus;
    }
}