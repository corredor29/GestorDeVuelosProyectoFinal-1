using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Application.Services;

// Fachada simple del catálogo de tipos de vía.
// La consola usa este servicio y él delega en los casos de uso correspondientes.
public sealed class StreetTypeService : IStreetTypeService
{
    private readonly GetStreetTypesUseCase _getUseCase;
    private readonly CreateStreetTypeUseCase _createUseCase;
    private readonly UpdateStreetTypeUseCase _updateUseCase;
    private readonly DeleteStreetTypeUseCase _deleteUseCase;

    public StreetTypeService(
        GetStreetTypesUseCase getUseCase,
        CreateStreetTypeUseCase createUseCase,
        UpdateStreetTypeUseCase updateUseCase,
        DeleteStreetTypeUseCase deleteUseCase)
    {
        _getUseCase = getUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }

    public async Task<IEnumerable<StreetType>> GetAllAsync() => await _getUseCase.ExecuteAllAsync();
    public async Task<StreetType?> GetByIdAsync(int id) => await _getUseCase.ExecuteByIdAsync(id);
    public async Task<StreetType?> GetByNameAsync(string name) => await _getUseCase.ExecuteByNameAsync(name);
    public async Task CreateAsync(string name) => await _createUseCase.ExecuteAsync(name);
    public async Task UpdateAsync(int id, string newName) => await _updateUseCase.ExecuteAsync(id, newName);
    public async Task DeleteAsync(int id) => await _deleteUseCase.ExecuteAsync(id);
    public async Task DeleteByNameAsync(string name) => await _deleteUseCase.ExecuteByNameAsync(name);
}
