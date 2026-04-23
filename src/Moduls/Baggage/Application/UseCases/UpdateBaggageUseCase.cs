using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Application.UseCases;

public sealed class UpdateBaggageUseCase
{
    private readonly IBaggageRepository _repository;

    public UpdateBaggageUseCase(IBaggageRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaggageRoot> ExecuteAsync(
        int id,
        decimal? newWeightKg,
        decimal? newChargedPrice,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByIdAsync(BaggageId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"Baggage with id '{id}' was not found.");

        if (newWeightKg is not null)
            existing.UpdateWeightKg(newWeightKg.Value);

        if (newChargedPrice is not null)
            existing.UpdateChargedPrice(newChargedPrice.Value);

        await _repository.UpdateAsync(existing);

        return existing;
    }
}