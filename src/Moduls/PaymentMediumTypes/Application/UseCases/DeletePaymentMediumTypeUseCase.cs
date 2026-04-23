using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Application.UseCases;

public sealed class DeletePaymentMediumTypeUseCase
{
    private readonly IPaymentMediumTypesRepository _repository;

    public DeletePaymentMediumTypeUseCase(IPaymentMediumTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(PaymentMediumTypesId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(PaymentMediumTypesId.Create(id));

        return true;
    }
}