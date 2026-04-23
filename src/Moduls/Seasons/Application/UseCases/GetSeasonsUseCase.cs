using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Seasons.Application.UseCases;

public sealed class GetSeasonsUseCase
{
    private readonly ISeasonsRepository _repository;

    public GetSeasonsUseCase(ISeasonsRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Season>> ExecuteAllAsync() => _repository.GetAllAsync();
    public Task<Season?> ExecuteByIdAsync(int id) => _repository.GetByIdAsync(SeasonsId.Create(id));
    public Task<Season?> ExecuteByNameAsync(string name) => _repository.GetByNameAsync(SeasonsName.Create(name));
}
