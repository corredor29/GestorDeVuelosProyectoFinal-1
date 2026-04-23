using GestorDeVuelosProyectoFinal.Moduls.People.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.People.Application.UseCases;

public sealed class GetPersonsUseCase
{
    private readonly IPeopleRepository _repository;

    public GetPersonsUseCase(IPeopleRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Person>> ExecuteAllAsync() => _repository.GetAllAsync();

    public Task<Person?> ExecuteByIdAsync(int id) => _repository.GetByIdAsync(PeopleId.Create(id));

    public Task<IEnumerable<Person>> ExecuteByFirstNameAsync(string firstName) =>
        _repository.GetByFirstNameAsync(PeopleNames.Create(firstName));

    public Task<IEnumerable<Person>> ExecuteByLastNameAsync(string lastName) =>
        _repository.GetByLastNameAsync(PeopleLastNames.Create(lastName));

    public Task<IEnumerable<Person>> ExecuteByDocumentNumberAsync(string documentNumber) =>
        _repository.GetByDocumentNumberAsync(PeopleDocumentNumber.Create(documentNumber));
}
