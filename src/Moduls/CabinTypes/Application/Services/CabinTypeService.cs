using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.CabinTypes.Application.UseCases;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CabinTypes.Application.Services;

public sealed class CabinTypeService : ICabinTypeService
{
    private readonly CreateCabinTypeUseCase _create;
    private readonly GetCabinTypesUseCase _get;
    private readonly UpdateCabinTypeUseCase _update;
    private readonly DeleteCabinTypeUseCase _delete;

    public CabinTypeService(
        CreateCabinTypeUseCase create,
        GetCabinTypesUseCase get,
        UpdateCabinTypeUseCase update,
        DeleteCabinTypeUseCase delete)
    {
        _create = create;
        _get = get;
        _update = update;
        _delete = delete;
    }

    public Task CreateAsync(string name) => _create.Execute(name);
    public Task<IEnumerable<CabinType>> GetAllAsync() => _get.Execute();
    public Task UpdateAsync(int id, string name) => _update.Execute(id, name);
    public Task DeleteByIdAsync(int id) => _delete.ExecuteByIdAsync(id);
    public Task DeleteByNameAsync(string name) => _delete.ExecuteByNameAsync(name);
}
