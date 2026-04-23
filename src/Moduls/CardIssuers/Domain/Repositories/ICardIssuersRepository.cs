using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.ValueObject;
using DomainAggregate = GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.Aggregate.CardIssuer;

namespace GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.Repositories;

public interface ICardIssuersRepository
{
    Task<DomainAggregate?> GetByIdAsync(CardIssuersId id);
    Task<DomainAggregate?> GetByNameAsync(CardIssuersName name);
    Task<IEnumerable<DomainAggregate>> GetAllAsync();
    Task SaveAsync(DomainAggregate cardIssuer);
    Task UpdateAsync(DomainAggregate cardIssuer);
    Task DeleteAsync(CardIssuersId id);
}