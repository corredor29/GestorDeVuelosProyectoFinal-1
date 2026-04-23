using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Application.UseCases;

public sealed class CreateBaggageUseCase
{
    private readonly IBaggageRepository _repository;

    public CreateBaggageUseCase(IBaggageRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaggageRoot> ExecuteAsync(
        int id,
        int checkinId,
        int baggageTypeId,
        decimal weightKg,
        decimal chargedPrice,
        CancellationToken cancellationToken = default)
    {
        var baggage = BaggageRoot.Create(id, checkinId, baggageTypeId, weightKg, chargedPrice);

        await _repository.SaveAsync(baggage);

        return baggage;
    }
}