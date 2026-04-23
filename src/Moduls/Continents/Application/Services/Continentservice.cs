using GestorDeVuelosProyectoFinal.Moduls.Continents.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.Continents.Application.Services;

public sealed class ContinentService : IContinentService
{
    // El servicio queda liviano porque solo coordina los casos de uso del módulo.
    private readonly GetContinentsUseCase _getAll;
    private readonly CreateContinentUseCase _create;
    private readonly UpdateContinentUseCase _update;
    private readonly DeleteContinentUseCase _delete;

    public ContinentService(
        GetContinentsUseCase getAll,
        CreateContinentUseCase create,
        UpdateContinentUseCase update,
        DeleteContinentUseCase delete)
    {
        _getAll = getAll;
        _create = create;
        _update = update;
        _delete = delete;
    }

    public Task<IEnumerable<Continent>> GetAllAsync()
        => _getAll.ExecuteAsync();

    // La UI trabaja por nombre en varios puntos, por eso el servicio lo expone directo.
    public Task<Continent?> GetByNameAsync(string name)
        => _getAll.GetByNameAsync(name);

    public Task CreateAsync(string name)
        => _create.ExecuteAsync(name);

    public Task UpdateAsync(string currentName, string newName)
        => _update.ExecuteAsync(currentName, newName);

    public Task DeleteAsync(string name)
        => _delete.ExecuteAsync(name);
}
