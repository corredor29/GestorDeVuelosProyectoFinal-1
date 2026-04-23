using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Application.Services;

public sealed class PersonalPositionsService : IPersonalPositionsService
{
    private readonly GetPersonalPositionsUseCase _getUseCase;
    private readonly CreatePersonalPositionUseCase _createUseCase;
    private readonly UpdatePersonalPositionUseCase _updateUseCase;
    private readonly DeletePersonalPositionUseCase _deleteUseCase;

    public PersonalPositionsService(
        GetPersonalPositionsUseCase getUseCase,
        CreatePersonalPositionUseCase createUseCase,
        UpdatePersonalPositionUseCase updateUseCase,
        DeletePersonalPositionUseCase deleteUseCase)
    {
        _getUseCase = getUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }

    public Task<IEnumerable<PersonalPosition>> GetAllAsync() => _getUseCase.ExecuteAllAsync();
    public Task<PersonalPosition?> GetByIdAsync(int id) => _getUseCase.ExecuteByIdAsync(id);
    public Task<PersonalPosition?> GetByNameAsync(string name) => _getUseCase.ExecuteByNameAsync(name);
    public Task CreateAsync(string name) => _createUseCase.ExecuteAsync(name);
    public Task UpdateAsync(int id, string name) => _updateUseCase.ExecuteAsync(id, name);
    public Task DeleteByIdAsync(int id) => _deleteUseCase.ExecuteByIdAsync(id);
    public Task DeleteByNameAsync(string name) => _deleteUseCase.ExecuteByNameAsync(name);
}
