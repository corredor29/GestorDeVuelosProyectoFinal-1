using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Application.UseCases;

public sealed class CreateDocumentTypeUseCase
{
    private readonly IDocumentTypesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDocumentTypeUseCase(IDocumentTypesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(string name, string code, CancellationToken cancellationToken = default)
    {
        var normalizedName = DocumentTypesName.Create(name);
        var normalizedCode = DocumentTypesCode.Create(code);

        var existingByName = await _repository.GetByNameAsync(normalizedName);
        if (existingByName is not null)
            throw new InvalidOperationException($"Ya existe un tipo de documento con el nombre '{normalizedName.Value}'.");

        var existingByCode = await _repository.GetByCodeAsync(normalizedCode);
        if (existingByCode is not null)
            throw new InvalidOperationException($"Ya existe un tipo de documento con el código '{normalizedCode.Value}'.");

        var documentType = DocumentType.Create(normalizedName.Value, normalizedCode.Value);

        await _repository.SaveAsync(documentType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
