using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.ValueObject;
using DomainAggregate = GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.Aggregate.CardIssuer;

namespace GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Application.UseCases;

public sealed class UpdateCardIssuerUseCase
{
    private readonly ICardIssuersRepository _repository;

    public UpdateCardIssuerUseCase(ICardIssuersRepository repository)
    {
        _repository = repository;
    }

    public async Task<DomainAggregate> ExecuteAsync(
        int id,
        string? newName,
        string? newIssuerNumber,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(CardIssuersId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"CardIssuer with id '{id}' was not found.");

        if (newName is not null)
        {
            var nameInUse = await _repository.GetByNameAsync(CardIssuersName.Create(newName));
            if (nameInUse is not null)
                throw new InvalidOperationException($"CardIssuer with name '{newName}' already exists.");

            existing.UpdateName(newName);
        }

        if (newIssuerNumber is not null)
            existing.UpdateIssuerNumber(newIssuerNumber);

        await _repository.UpdateAsync(existing);

        return existing;
    }
}