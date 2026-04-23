using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Application.UseCases;

public sealed class DeleteCardTypeUseCase
{
    private readonly ICardTypesRepository _repository;

    public DeleteCardTypeUseCase(ICardTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(CardTypesId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(CardTypesId.Create(id));

        return true;
    }
}