using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Application.UseCases;

public sealed class GetAllBaggageTypesUseCase
{
    private readonly IBaggageTypesRepository _repository;

    public GetAllBaggageTypesUseCase(IBaggageTypesRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<BaggageType>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }
}