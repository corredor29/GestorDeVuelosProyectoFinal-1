using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Application.UseCases;

public sealed class GetPeoplePhonesUseCase
{
    private readonly IPeoplePhonesRepository _repository;

    public GetPeoplePhonesUseCase(IPeoplePhonesRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<PersonPhone>> ExecuteAllAsync() => _repository.GetAllAsync();
    public Task<PersonPhone?> ExecuteByIdAsync(int id) => _repository.GetByIdAsync(PersonPhonesId.Create(id));
    public Task<IEnumerable<PersonPhone>> ExecuteByPersonIdAsync(int personId) => _repository.GetByPersonIdAsync(PeopleId.Create(personId));
    public Task<IEnumerable<PersonPhone>> ExecuteByPhoneNumberAsync(string phoneNumber) => _repository.GetByPhoneNumberAsync(PersonPhonesPhoneNumber.Create(phoneNumber));
    public Task<IEnumerable<PersonPhone>> ExecuteByPhoneCodeIdAsync(int phoneCodeId) => _repository.GetByPhoneCodeIdAsync(PhoneCodesId.Create(phoneCodeId));
    public Task<IEnumerable<PersonPhone>> ExecuteByPersonNameAsync(string personName) => _repository.GetByPersonNameAsync(personName);
}
