using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Application.UseCases;

public sealed class CreatePaymentMethodUseCase
{
    private readonly IPaymentMethodsRepository _repository;

    public CreatePaymentMethodUseCase(IPaymentMethodsRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaymentMethod> ExecuteAsync(
        int id,
        string displayName,
        CancellationToken cancellationToken = default)
    {
        var paymentMethod = PaymentMethod.Create(id, displayName);

        await _repository.SaveAsync(paymentMethod);

        return paymentMethod;
    }
}