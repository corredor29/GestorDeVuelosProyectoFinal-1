using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Application.UseCases;

public sealed class GetAllPaymentMediumTypesUseCase
{
    private readonly IPaymentMediumTypesRepository _repository;

    public GetAllPaymentMediumTypesUseCase(IPaymentMediumTypesRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<PaymentMediumType>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }
}