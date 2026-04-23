using GestorDeVuelosProyectoFinal.Moduls.People.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.People.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.People.Application.Services;

// Este servicio funciona como fachada simple para el módulo de personas.
// La lógica real vive en los casos de uso y aquí solo la exponemos de forma cómoda para la UI.
public sealed class PersonService : IPersonService
{
    private readonly GetPersonsUseCase _getUseCase;
    private readonly CreatePersonUseCase _createUseCase;
    private readonly UpdatePersonUseCase _updateUseCase;
    private readonly DeletePersonUseCase _deleteUseCase;

    public PersonService(
        GetPersonsUseCase getUseCase,
        CreatePersonUseCase createUseCase,
        UpdatePersonUseCase updateUseCase,
        DeletePersonUseCase deleteUseCase)
    {
        _getUseCase = getUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }

    public Task<IEnumerable<Person>> GetAllAsync() => _getUseCase.ExecuteAllAsync();
    public Task<Person?> GetByIdAsync(int id) => _getUseCase.ExecuteByIdAsync(id);
    public Task<IEnumerable<Person>> GetByFirstNameAsync(string firstName) => _getUseCase.ExecuteByFirstNameAsync(firstName);
    public Task<IEnumerable<Person>> GetByLastNameAsync(string lastName) => _getUseCase.ExecuteByLastNameAsync(lastName);
    public Task<IEnumerable<Person>> GetByDocumentNumberAsync(string documentNumber) => _getUseCase.ExecuteByDocumentNumberAsync(documentNumber);
    public Task CreateAsync(int documentTypeId, string documentNumber, string firstName, string lastName, DateTime? birthDate, char? gender, int? addressId) =>
        _createUseCase.ExecuteAsync(documentTypeId, documentNumber, firstName, lastName, birthDate, gender, addressId);
    public Task UpdateAsync(int id, int documentTypeId, string documentNumber, string firstName, string lastName, DateTime? birthDate, char? gender, int? addressId) =>
        _updateUseCase.ExecuteAsync(id, documentTypeId, documentNumber, firstName, lastName, birthDate, gender, addressId);
    public Task DeleteByIdAsync(int id) => _deleteUseCase.ExecuteByIdAsync(id);
    public Task<int> DeleteByFirstNameAsync(string firstName) => _deleteUseCase.ExecuteByFirstNameAsync(firstName);
    public Task<int> DeleteByLastNameAsync(string lastName) => _deleteUseCase.ExecuteByLastNameAsync(lastName);
    public Task<int> DeleteByDocumentNumberAsync(string documentNumber) => _deleteUseCase.ExecuteByDocumentNumberAsync(documentNumber);
}
