using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Application.UseCases;

public sealed class UpdateDocumentTypeUseCase
{
    private readonly IDocumentTypesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDocumentTypeUseCase(IDocumentTypesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int id, string name, string code, CancellationToken cancellationToken = default)
    {
        var documentTypeId = DocumentTypesId.Create(id);
        var normalizedName = DocumentTypesName.Create(name);
        var normalizedCode = DocumentTypesCode.Create(code);

        var existing = await _repository.GetByIdAsync(documentTypeId);
        if (existing is null)
            throw new InvalidOperationException($"No se encontró el tipo de documento con ID {id}.");

        var duplicateByName = await _repository.GetByNameAsync(normalizedName);
        if (duplicateByName is not null && duplicateByName.Id.Value != id)
            throw new InvalidOperationException($"Ya existe otro tipo de documento con el nombre '{normalizedName.Value}'.");

        var duplicateByCode = await _repository.GetByCodeAsync(normalizedCode);
        if (duplicateByCode is not null && duplicateByCode.Id.Value != id)
            throw new InvalidOperationException($"Ya existe otro tipo de documento con el código '{normalizedCode.Value}'.");

        existing.Update(normalizedName.Value, normalizedCode.Value);

        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
