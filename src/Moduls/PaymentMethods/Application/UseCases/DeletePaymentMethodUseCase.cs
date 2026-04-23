using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Application.UseCases;

public sealed class DeletePaymentMethodUseCase
{
    private readonly IPaymentMethodsRepository _repository;

    public DeletePaymentMethodUseCase(IPaymentMethodsRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(PaymentMethodsId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(PaymentMethodsId.Create(id));

        return true;
    }
}