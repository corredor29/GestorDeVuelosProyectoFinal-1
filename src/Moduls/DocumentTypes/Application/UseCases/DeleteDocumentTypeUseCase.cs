using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Application.UseCases;

public sealed class DeleteDocumentTypeUseCase
{
    private readonly IDocumentTypesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDocumentTypeUseCase(IDocumentTypesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var documentTypeId = DocumentTypesId.Create(id);
        var existing = await _repository.GetByIdAsync(documentTypeId);

        if (existing is null)
            throw new InvalidOperationException($"No se encontró el tipo de documento con ID {id}.");

        await _repository.DeleteAsync(documentTypeId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ExecuteByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var normalizedName = DocumentTypesName.Create(name);
        var existing = await _repository.GetByNameAsync(normalizedName);

        if (existing is null)
            throw new InvalidOperationException($"No se encontró el tipo de documento con nombre '{normalizedName.Value}'.");

        await _repository.DeleteByNameAsync(normalizedName);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ExecuteByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalizedCode = DocumentTypesCode.Create(code);
        var existing = await _repository.GetByCodeAsync(normalizedCode);

        if (existing is null)
            throw new InvalidOperationException($"No se encontró el tipo de documento con código '{normalizedCode.Value}'.");

        await _repository.DeleteByCodeAsync(normalizedCode);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
