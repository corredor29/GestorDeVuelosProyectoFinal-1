using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Payments.Application.UseCases;

public sealed class CreatePaymentUseCase
{
    private readonly IPaymentsRepository _repository;

    public CreatePaymentUseCase(IPaymentsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Payment> ExecuteAsync(
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

        return payment;
    }
}