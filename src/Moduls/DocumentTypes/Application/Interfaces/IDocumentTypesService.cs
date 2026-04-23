using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Application.Interfaces;

public interface IDocumentTypesService
{
    Task<IEnumerable<DocumentType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DocumentType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<DocumentType?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<DocumentType?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task CreateAsync(string name, string code, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, string name, string code, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task DeleteByNameAsync(string name, CancellationToken cancellationToken = default);
    Task DeleteByCodeAsync(string code, CancellationToken cancellationToken = default);
}
