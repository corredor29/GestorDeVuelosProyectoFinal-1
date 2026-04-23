using GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PersonEmails.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.Application.UseCases;

public sealed class GetPersonEmailsUseCase
{
    private readonly IPersonEmailsRepository _repository;

    public GetPersonEmailsUseCase(IPersonEmailsRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<PersonEmail>> ExecuteAsync() => _repository.GetAllAsync();

    public Task<IEnumerable<PersonEmail>> GetByPersonIdAsync(int personId) => _repository.GetByPersonIdAsync(personId);

    public Task<PersonEmail?> GetByIdAsync(int id) => _repository.GetByIdAsync(PersonEmailsId.Create(id));
}
