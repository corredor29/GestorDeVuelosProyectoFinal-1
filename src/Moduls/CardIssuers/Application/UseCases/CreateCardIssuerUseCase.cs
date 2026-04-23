using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.ValueObject;
using DomainAggregate = GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.Aggregate.CardIssuer;

namespace GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Application.UseCases;

public sealed class CreateCardIssuerUseCase
{
    private readonly ICardIssuersRepository _repository;

    public CreateCardIssuerUseCase(ICardIssuersRepository repository)
    {
        _repository = repository;
    }

    public async Task<DomainAggregate> ExecuteAsync(
        int id,
        string name,
        string issuerNumber,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByNameAsync(CardIssuersName.Create(name));
        if (existing is not null)
            throw new InvalidOperationException($"CardIssuer with name '{name}' already exists.");

        var cardIssuer = DomainAggregate.Create(id, name, issuerNumber);

        await _repository.SaveAsync(cardIssuer);

        return cardIssuer;
    }
}