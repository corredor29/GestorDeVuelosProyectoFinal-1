using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Application.UseCases;

public sealed class UpdatePaymentMediumTypeUseCase
{
    private readonly IPaymentMediumTypesRepository _repository;

    public UpdatePaymentMediumTypeUseCase(IPaymentMediumTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaymentMediumType> ExecuteAsync(
        int id,
        string? newName,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(PaymentMediumTypesId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"PaymentMediumType with id '{id}' was not found.");

        if (newName is not null)
        {
            var nameInUse = await _repository.GetByNameAsync(PaymentMediumTypesName.Create(newName));
            if (nameInUse is not null)
                throw new InvalidOperationException($"PaymentMediumType with name '{newName}' already exists.");

            existing.UpdateName(newName);
        }

        await _repository.UpdateAsync(existing);

        return existing;
    }
}