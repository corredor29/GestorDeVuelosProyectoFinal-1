using GestorDeVuelosProyectoFinal.Moduls.Seasons.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.Seasons.Application.Services;

public sealed class SeasonsService : ISeasonsService
{
    private readonly GetSeasonsUseCase _getUseCase;
    private readonly CreateSeasonUseCase _createUseCase;
    private readonly UpdateSeasonUseCase _updateUseCase;
    private readonly DeleteSeasonUseCase _deleteUseCase;

    public SeasonsService(
        GetSeasonsUseCase getUseCase,
        CreateSeasonUseCase createUseCase,
        UpdateSeasonUseCase updateUseCase,
        DeleteSeasonUseCase deleteUseCase)
    {
        _getUseCase = getUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }

    public Task<IEnumerable<Season>> GetAllAsync() => _getUseCase.ExecuteAllAsync();
    public Task<Season?> GetByIdAsync(int id) => _getUseCase.ExecuteByIdAsync(id);
    public Task<Season?> GetByNameAsync(string name) => _getUseCase.ExecuteByNameAsync(name);
    public Task CreateAsync(string name, string? description, decimal priceFactor) => _createUseCase.ExecuteAsync(name, description, priceFactor);
    public Task UpdateAsync(int id, string name, string? description, decimal priceFactor) => _updateUseCase.ExecuteAsync(id, name, description, priceFactor);
    public Task DeleteByIdAsync(int id) => _deleteUseCase.ExecuteByIdAsync(id);
    public Task DeleteByNameAsync(string name) => _deleteUseCase.ExecuteByNameAsync(name);
}
