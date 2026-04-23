using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.Moduls.Continents.Application.UseCases;

public sealed class GetContinentsUseCase
{
    private readonly IContinentsRepository _repository;

    public GetContinentsUseCase(IContinentsRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Continent>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Continent?> GetByNameAsync(string name)
    {
        return await _repository.GetByNameAsync(name);
    }
}