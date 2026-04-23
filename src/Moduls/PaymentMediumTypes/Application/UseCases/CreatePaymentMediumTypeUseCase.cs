using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Application.UseCases;

public sealed class CreatePaymentMediumTypeUseCase
{
    private readonly IPaymentMediumTypesRepository _repository;

    public CreatePaymentMediumTypeUseCase(IPaymentMediumTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaymentMediumType> ExecuteAsync(
        int id,
        string name,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByNameAsync(PaymentMediumTypesName.Create(name));
        if (existing is not null)
            throw new InvalidOperationException($"PaymentMediumType with name '{name}' already exists.");

        var paymentMediumType = PaymentMediumType.Create(id, name);

        await _repository.SaveAsync(paymentMediumType);

        return paymentMediumType;
    }
}