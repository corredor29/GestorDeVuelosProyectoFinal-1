using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.ValueObject;
using DomainAggregate = GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.Aggregate.CardTypes;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Application.UseCases;

public sealed class UpdateCardTypeUseCase
{
    private readonly ICardTypesRepository _repository;

    public UpdateCardTypeUseCase(ICardTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<DomainAggregate> ExecuteAsync(
        int id,
        string? newName,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(CardTypesId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"CardType with id '{id}' was not found.");

        if (newName is not null)
        {
            var nameInUse = await _repository.GetByNameAsync(CardTypesName.Create(newName));
            if (nameInUse is not null)
                throw new InvalidOperationException($"CardType with name '{newName}' already exists.");

            existing.UpdateName(newName);
        }

        await _repository.UpdateAsync(existing);

        return existing;
    }
}