using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Application.UseCases;

public sealed class DeleteBaggageUseCase
{
    private readonly IBaggageRepository _repository;

    public DeleteBaggageUseCase(IBaggageRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.FindByIdAsync(BaggageId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(BaggageId.Create(id));

        return true;
    }
}