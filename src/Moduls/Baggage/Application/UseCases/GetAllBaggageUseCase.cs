using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Application.UseCases;

public sealed class GetAllBaggageUseCase
{
    private readonly IBaggageRepository _repository;

    public GetAllBaggageUseCase(IBaggageRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<BaggageRoot>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.FindAllAsync();
    }
}