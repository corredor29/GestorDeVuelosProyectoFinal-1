using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Application.UseCases;

public sealed class GetPaymentMediumTypeUseCase
{
    private readonly IPaymentMediumTypesRepository _repository;

    public GetPaymentMediumTypeUseCase(IPaymentMediumTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaymentMediumType> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByIdAsync(PaymentMediumTypesId.Create(id));
        if (result is null)
            throw new KeyNotFoundException($"PaymentMediumType with id '{id}' was not found.");

        return result;
    }
}