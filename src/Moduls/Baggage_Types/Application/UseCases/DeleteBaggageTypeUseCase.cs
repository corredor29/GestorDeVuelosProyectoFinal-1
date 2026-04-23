using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Application.UseCases;

public sealed class DeleteBaggageTypeUseCase
{
    private readonly IBaggageTypesRepository _repository;

    public DeleteBaggageTypeUseCase(IBaggageTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(BaggageTypeId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(BaggageTypeId.Create(id));

        return true;
    }
}
