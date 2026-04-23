using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Application.UseCases;

public sealed class CreateBaggageTypeUseCase
{
    private readonly IBaggageTypesRepository _repository;

    public CreateBaggageTypeUseCase(IBaggageTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaggageType> ExecuteAsync(
        int id,
        string name,
        decimal maxWeightKg,
        decimal basePrice,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByNameAsync(BaggageTypeName.Create(name));
        if (existing is not null)
            throw new InvalidOperationException($"BaggageType with name '{name}' already exists.");

        var baggageType = BaggageType.Create(id, name, maxWeightKg, basePrice);

        await _repository.SaveAsync(baggageType);

        return baggageType;
    }
}
