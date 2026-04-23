using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Application.Interfaces;

public interface IPeoplePhonesService
{
    Task<IEnumerable<PersonPhone>> GetAllAsync();
    Task<PersonPhone?> GetByIdAsync(int id);
    Task<IEnumerable<PersonPhone>> GetByPersonIdAsync(int personId);
    Task<IEnumerable<PersonPhone>> GetByPhoneNumberAsync(string phoneNumber);
    Task<IEnumerable<PersonPhone>> GetByPhoneCodeIdAsync(int phoneCodeId);
    Task<IEnumerable<PersonPhone>> GetByPersonNameAsync(string personName);
    Task CreateAsync(int personId, int phoneCodeId, string phoneNumber, bool isPrimary);
    Task UpdateAsync(int id, int personId, int phoneCodeId, string phoneNumber, bool isPrimary);
    Task DeleteByIdAsync(int id);
    Task<int> DeleteByPhoneNumberAsync(string phoneNumber);
    Task<int> DeleteByPhoneCodeIdAsync(int phoneCodeId);
    Task<int> DeleteByPersonNameAsync(string personName);
}
