using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Application.UseCases;

public sealed class DeletePaymentStatusUseCase
{
    private readonly IPaymentStatusesRepository _repository;

    public DeletePaymentStatusUseCase(IPaymentStatusesRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(PaymentStatuseId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(PaymentStatuseId.Create(id));

        return true;
    }
}