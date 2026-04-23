using GestorDeVuelosProyectoFinal.Moduls.People.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.Moduls.People.Application.UseCases;

public sealed class CreatePersonUseCase
{
    private readonly IPeopleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePersonUseCase(IPeopleRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(
        int documentTypeId,
        string documentNumber,
        string firstName,
        string lastName,
        DateTime? birthDate,
        char? gender,
        int? addressId)
    {
        var documentType = DocumentTypesId.Create(documentTypeId);
        var docNumber = PeopleDocumentNumber.Create(documentNumber);

        var existing = await _repository.GetByDocumentAsync(documentType, docNumber);
        if (existing is not null)
            throw new InvalidOperationException("Ya existe una persona con ese tipo y número de documento.");

        var now = DateTime.Now;
        var person = Person.Create(documentTypeId, documentNumber, firstName, lastName, birthDate, gender, addressId, now, now);

        await _repository.SaveAsync(person);
        await _unitOfWork.SaveChangesAsync();
    }
}
