using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.People.Application.UseCases;

public sealed class UpdatePersonUseCase
{
    private readonly IPeopleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePersonUseCase(IPeopleRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(
        int id,
        int documentTypeId,
        string documentNumber,
        string firstName,
        string lastName,
        DateTime? birthDate,
        char? gender,
        int? addressId)
    {
        var personId = PeopleId.Create(id);
        var documentType = DocumentTypesId.Create(documentTypeId);
        var docNumber = PeopleDocumentNumber.Create(documentNumber);

        var existing = await _repository.GetByIdAsync(personId);
        if (existing is null)
            throw new InvalidOperationException($"No se encontró la persona con ID {id}.");

        var duplicate = await _repository.GetByDocumentAsync(documentType, docNumber);
        if (duplicate is not null && duplicate.Id.Value != id)
            throw new InvalidOperationException("Ya existe otra persona con ese tipo y número de documento.");

        existing.Update(documentTypeId, documentNumber, firstName, lastName, birthDate, gender, addressId, DateTime.Now);

        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync();
    }
}
