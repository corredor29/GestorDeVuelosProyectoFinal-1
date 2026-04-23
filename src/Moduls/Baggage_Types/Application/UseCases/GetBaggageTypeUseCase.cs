using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Application.UseCases;

public sealed class GetBaggageTypeUseCase
{
    private readonly IBaggageTypesRepository _repository;

    public GetBaggageTypeUseCase(IBaggageTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaggageType> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByIdAsync(BaggageTypeId.Create(id));
        if (result is null)
            throw new KeyNotFoundException($"BaggageType with id '{id}' was not found.");

        return result;
    }
}