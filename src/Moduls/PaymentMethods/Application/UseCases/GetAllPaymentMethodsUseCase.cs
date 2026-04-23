using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Application.UseCases;

public sealed class GetAllPaymentMethodsUseCase
{
    private readonly IPaymentMethodsRepository _repository;

    public GetAllPaymentMethodsUseCase(IPaymentMethodsRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<PaymentMethod>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }
}