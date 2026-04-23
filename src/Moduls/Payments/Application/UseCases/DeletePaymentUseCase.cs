using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Payments.Application.UseCases;

public sealed class DeletePaymentUseCase
{
    private readonly IPaymentsRepository _repository;

    public DeletePaymentUseCase(IPaymentsRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(PaymentsId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(PaymentsId.Create(id));

        return true;
    }
}