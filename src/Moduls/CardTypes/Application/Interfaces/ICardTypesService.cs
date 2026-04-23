using DomainAggregate = GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.Aggregate.CardTypes;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Application.Interfaces;

public interface ICardTypesService
{
    Task<DomainAggregate> CreateAsync(int id, string name, CancellationToken cancellationToken = default);
    Task<DomainAggregate?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainAggregate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DomainAggregate> UpdateAsync(int id, string? newName, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}