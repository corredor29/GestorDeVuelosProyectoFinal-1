using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Application.UseCases;

public sealed class GetPaymentMethodUseCase
{
    private readonly IPaymentMethodsRepository _repository;

    public GetPaymentMethodUseCase(IPaymentMethodsRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaymentMethod> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByIdAsync(PaymentMethodsId.Create(id));
        if (result is null)
            throw new KeyNotFoundException($"PaymentMethod with id '{id}' was not found.");

        return result;
    }
}