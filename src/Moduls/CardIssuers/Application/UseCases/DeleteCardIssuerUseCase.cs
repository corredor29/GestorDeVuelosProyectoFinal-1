using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Application.UseCases;

public sealed class DeleteCardIssuerUseCase
{
    private readonly ICardIssuersRepository _repository;

    public DeleteCardIssuerUseCase(ICardIssuersRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(CardIssuersId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(CardIssuersId.Create(id));

        return true;
    }
}