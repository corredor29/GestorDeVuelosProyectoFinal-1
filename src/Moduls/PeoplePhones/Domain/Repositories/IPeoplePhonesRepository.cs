using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.Repositories;

public interface IPeoplePhonesRepository
{
    Task<PersonPhone?> GetByIdAsync(PersonPhonesId id);
    Task<IEnumerable<PersonPhone>> GetByPersonIdAsync(PeopleId personId);
    Task<PersonPhone?> GetPrimaryByPersonIdAsync(PeopleId personId);
    Task<IEnumerable<PersonPhone>> GetByPhoneNumberAsync(PersonPhonesPhoneNumber phoneNumber);
    Task<IEnumerable<PersonPhone>> GetByPhoneCodeIdAsync(PhoneCodesId phoneCodeId);
    Task<IEnumerable<PersonPhone>> GetByPersonNameAsync(string personName);
    Task<IEnumerable<PersonPhone>> GetAllAsync();
    Task SaveAsync(PersonPhone personPhone);
    Task UpdateAsync(PersonPhone personPhone);
    Task DeleteAsync(PersonPhonesId id);
    Task<int> DeleteByPhoneNumberAsync(PersonPhonesPhoneNumber phoneNumber);
    Task<int> DeleteByPhoneCodeIdAsync(PhoneCodesId phoneCodeId);
    Task<int> DeleteByPersonNameAsync(string personName);
}
