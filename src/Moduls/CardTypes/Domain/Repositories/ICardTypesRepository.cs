using DomainAggregate = GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.Aggregate.CardTypes;
using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.Repositories;

public interface ICardTypesRepository
{
    Task<DomainAggregate?> GetByIdAsync(CardTypesId id);
    Task<DomainAggregate?> GetByNameAsync(CardTypesName name);
    Task<IEnumerable<DomainAggregate>> GetAllAsync();
    Task SaveAsync(DomainAggregate cardTypes);
    Task UpdateAsync(DomainAggregate cardTypes);
    Task DeleteAsync(CardTypesId id);
}