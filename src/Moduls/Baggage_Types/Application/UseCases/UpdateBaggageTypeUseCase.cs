using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Application.UseCases;

public sealed class UpdateBaggageTypeUseCase
{
    private readonly IBaggageTypesRepository _repository;

    public UpdateBaggageTypeUseCase(IBaggageTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaggageType> ExecuteAsync(
        int id,
        string? newName,
        decimal? newMaxWeightKg,
        decimal? newBasePrice,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(BaggageTypeId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"BaggageType with id '{id}' was not found.");

        if (newName is not null)
        {
            var nameInUse = await _repository.GetByNameAsync(BaggageTypeName.Create(newName));
            if (nameInUse is not null)
                throw new InvalidOperationException($"BaggageType with name '{newName}' already exists.");

            existing.UpdateName(newName);
        }

        if (newMaxWeightKg is not null)
            existing.UpdateMaxWeight(newMaxWeightKg.Value);

        if (newBasePrice is not null)
            existing.UpdateBasePrice(newBasePrice.Value);

        await _repository.UpdateAsync(existing);

        return existing;
    }
}