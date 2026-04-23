using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.ValueObject;
using DomainAggregate = GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.Aggregate.CardTypes;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Application.UseCases;

public sealed class GetCardTypeUseCase
{
    private readonly ICardTypesRepository _repository;

    public GetCardTypeUseCase(ICardTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<DomainAggregate> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByIdAsync(CardTypesId.Create(id));
        if (result is null)
            throw new KeyNotFoundException($"CardType with id '{id}' was not found.");

        return result;
    }
}