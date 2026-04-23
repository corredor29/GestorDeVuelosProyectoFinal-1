using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Application.UseCases;

public sealed class GetDocumentTypesUseCase
{
    private readonly IDocumentTypesRepository _repository;

    public GetDocumentTypesUseCase(IDocumentTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DocumentType>> ExecuteAllAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync();
    }

    public async Task<DocumentType?> ExecuteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(DocumentTypesId.Create(id));
    }

    public async Task<DocumentType?> ExecuteByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByNameAsync(DocumentTypesName.Create(name));
    }

    public async Task<DocumentType?> ExecuteByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByCodeAsync(DocumentTypesCode.Create(code));
    }
}
