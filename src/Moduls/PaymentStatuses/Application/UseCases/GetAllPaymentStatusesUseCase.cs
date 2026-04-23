using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Application.UseCases;

public sealed class GetAllPaymentStatusesUseCase
{
    private readonly IPaymentStatusesRepository _repository;

    public GetAllPaymentStatusesUseCase(IPaymentStatusesRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<PaymentStatuse>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }
}