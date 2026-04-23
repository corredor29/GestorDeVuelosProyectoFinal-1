using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.ValueObject;
using DomainAggregate = GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.Aggregate.CardTypes;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Application.UseCases;

public sealed class CreateCardTypeUseCase
{
    private readonly ICardTypesRepository _repository;

    public CreateCardTypeUseCase(ICardTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<DomainAggregate> ExecuteAsync(
        int id,
        string name,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByNameAsync(CardTypesName.Create(name));
        if (existing is not null)
            throw new InvalidOperationException($"CardType with name '{name}' already exists.");

        var cardType = DomainAggregate.Create(id, name);

        await _repository.SaveAsync(cardType);

        return cardType;
    }
}