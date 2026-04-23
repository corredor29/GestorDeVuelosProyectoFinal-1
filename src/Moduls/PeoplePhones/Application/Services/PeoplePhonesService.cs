using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Application.Services;

// Fachada del módulo de teléfonos de personas.
// La UI habla con este servicio y él delega en los casos de uso.
public sealed class PeoplePhonesService : IPeoplePhonesService
{
    private readonly GetPeoplePhonesUseCase _getUseCase;
    private readonly CreatePeoplePhoneUseCase _createUseCase;
    private readonly UpdatePeoplePhoneUseCase _updateUseCase;
    private readonly DeletePeoplePhoneUseCase _deleteUseCase;

    public PeoplePhonesService(
        GetPeoplePhonesUseCase getUseCase,
        CreatePeoplePhoneUseCase createUseCase,
        UpdatePeoplePhoneUseCase updateUseCase,
        DeletePeoplePhoneUseCase deleteUseCase)
    {
        _getUseCase = getUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }

    public Task<IEnumerable<PersonPhone>> GetAllAsync() => _getUseCase.ExecuteAllAsync();
    public Task<PersonPhone?> GetByIdAsync(int id) => _getUseCase.ExecuteByIdAsync(id);
    public Task<IEnumerable<PersonPhone>> GetByPersonIdAsync(int personId) => _getUseCase.ExecuteByPersonIdAsync(personId);
    public Task<IEnumerable<PersonPhone>> GetByPhoneNumberAsync(string phoneNumber) => _getUseCase.ExecuteByPhoneNumberAsync(phoneNumber);
    public Task<IEnumerable<PersonPhone>> GetByPhoneCodeIdAsync(int phoneCodeId) => _getUseCase.ExecuteByPhoneCodeIdAsync(phoneCodeId);
    public Task<IEnumerable<PersonPhone>> GetByPersonNameAsync(string personName) => _getUseCase.ExecuteByPersonNameAsync(personName);
    public Task CreateAsync(int personId, int phoneCodeId, string phoneNumber, bool isPrimary) => _createUseCase.ExecuteAsync(personId, phoneCodeId, phoneNumber, isPrimary);
    public Task UpdateAsync(int id, int personId, int phoneCodeId, string phoneNumber, bool isPrimary) => _updateUseCase.ExecuteAsync(id, personId, phoneCodeId, phoneNumber, isPrimary);
    public Task DeleteByIdAsync(int id) => _deleteUseCase.ExecuteByIdAsync(id);
    public Task<int> DeleteByPhoneNumberAsync(string phoneNumber) => _deleteUseCase.ExecuteByPhoneNumberAsync(phoneNumber);
    public Task<int> DeleteByPhoneCodeIdAsync(int phoneCodeId) => _deleteUseCase.ExecuteByPhoneCodeIdAsync(phoneCodeId);
    public Task<int> DeleteByPersonNameAsync(string personName) => _deleteUseCase.ExecuteByPersonNameAsync(personName);
}
