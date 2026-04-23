using DomainAggregate = GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.Aggregate.CardIssuer;

namespace GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Application.Interfaces;

public interface ICardIssuersService
{
    Task<DomainAggregate> CreateAsync(int id, string name, string issuerNumber, CancellationToken cancellationToken = default);
    Task<DomainAggregate?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainAggregate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DomainAggregate> UpdateAsync(int id, string? newName, string? newIssuerNumber, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}