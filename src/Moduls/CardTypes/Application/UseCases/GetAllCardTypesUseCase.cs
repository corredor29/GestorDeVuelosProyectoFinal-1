using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.Repositories;
using DomainAggregate = GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.Aggregate.CardTypes;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Application.UseCases;

public sealed class GetAllCardTypesUseCase
{
    private readonly ICardTypesRepository _repository;

    public GetAllCardTypesUseCase(ICardTypesRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<DomainAggregate>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }
}