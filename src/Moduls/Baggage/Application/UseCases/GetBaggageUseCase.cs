using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Application.UseCases;

public sealed class GetBaggageUseCase
{
    private readonly IBaggageRepository _repository;

    public GetBaggageUseCase(IBaggageRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaggageRoot> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.FindByIdAsync(BaggageId.Create(id));
        if (result is null)
            throw new KeyNotFoundException($"Baggage with id '{id}' was not found.");

        return result;
    }
}