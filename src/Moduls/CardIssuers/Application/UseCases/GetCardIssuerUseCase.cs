using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.ValueObject;
using DomainAggregate = GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.Aggregate.CardIssuer;

namespace GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Application.UseCases;

public sealed class GetCardIssuerUseCase
{
    private readonly ICardIssuersRepository _repository;

    public GetCardIssuerUseCase(ICardIssuersRepository repository)
    {
        _repository = repository;
    }

    public async Task<DomainAggregate> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByIdAsync(CardIssuersId.Create(id));
        if (result is null)
            throw new KeyNotFoundException($"CardIssuer with id '{id}' was not found.");

        return result;
    }
}