using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Application.UseCases;

public sealed class GetPersonalPositionsUseCase
{
    private readonly IPersonalPositionsRepository _repository;

    public GetPersonalPositionsUseCase(IPersonalPositionsRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<PersonalPosition>> ExecuteAllAsync() => _repository.GetAllAsync();
    public Task<PersonalPosition?> ExecuteByIdAsync(int id) => _repository.GetByIdAsync(PersonalPositionsId.Create(id));
    public Task<PersonalPosition?> ExecuteByNameAsync(string name) => _repository.GetByNameAsync(PersonalPositionsName.Create(name));
}
