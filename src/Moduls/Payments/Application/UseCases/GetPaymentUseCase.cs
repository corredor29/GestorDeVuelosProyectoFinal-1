using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Payments.Application.UseCases;

public sealed class GetPaymentUseCase
{
    private readonly IPaymentsRepository _repository;

    public GetPaymentUseCase(IPaymentsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Payment> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByIdAsync(PaymentsId.Create(id));
        if (result is null)
            throw new KeyNotFoundException($"Payment with id '{id}' was not found.");

        return result;
    }
}