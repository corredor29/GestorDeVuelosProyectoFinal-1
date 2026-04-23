using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Application.UseCases;

public sealed class GetPaymentStatusUseCase
{
    private readonly IPaymentStatusesRepository _repository;

    public GetPaymentStatusUseCase(IPaymentStatusesRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaymentStatuse> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByIdAsync(PaymentStatuseId.Create(id));
        if (result is null)
            throw new KeyNotFoundException($"PaymentStatus with id '{id}' was not found.");

        return result;
    }
}