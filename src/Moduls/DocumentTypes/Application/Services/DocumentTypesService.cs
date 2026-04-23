using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Application.Services;

// Servicio del catálogo de tipos de documento.
// Su trabajo aquí es mantener una API limpia para la consola y otros módulos.
public sealed class DocumentTypesService : IDocumentTypesService
{
    private readonly GetDocumentTypesUseCase _getUseCase;
    private readonly CreateDocumentTypeUseCase _createUseCase;
    private readonly UpdateDocumentTypeUseCase _updateUseCase;
    private readonly DeleteDocumentTypeUseCase _deleteUseCase;

    public DocumentTypesService(
        GetDocumentTypesUseCase getUseCase,
        CreateDocumentTypeUseCase createUseCase,
        UpdateDocumentTypeUseCase updateUseCase,
        DeleteDocumentTypeUseCase deleteUseCase)
    {
        _getUseCase = getUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }

    public async Task<IEnumerable<DocumentType>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _getUseCase.ExecuteAllAsync(cancellationToken);

    public async Task<DocumentType?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _getUseCase.ExecuteByIdAsync(id, cancellationToken);

    public async Task<DocumentType?> GetByNameAsync(string name, CancellationToken cancellationToken = default) =>
        await _getUseCase.ExecuteByNameAsync(name, cancellationToken);

    public async Task<DocumentType?> GetByCodeAsync(string code, CancellationToken cancellationToken = default) =>
        await _getUseCase.ExecuteByCodeAsync(code, cancellationToken);

    public async Task CreateAsync(string name, string code, CancellationToken cancellationToken = default) =>
        await _createUseCase.ExecuteAsync(name, code, cancellationToken);

    public async Task UpdateAsync(int id, string name, string code, CancellationToken cancellationToken = default) =>
        await _updateUseCase.ExecuteAsync(id, name, code, cancellationToken);

    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _deleteUseCase.ExecuteByIdAsync(id, cancellationToken);

    public async Task DeleteByNameAsync(string name, CancellationToken cancellationToken = default) =>
        await _deleteUseCase.ExecuteByNameAsync(name, cancellationToken);

    public async Task DeleteByCodeAsync(string code, CancellationToken cancellationToken = default) =>
        await _deleteUseCase.ExecuteByCodeAsync(code, cancellationToken);
}
