using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Application.UseCases;

public sealed class DeleteCheckinUseCase
{
    private readonly ICheckinsRepository _repository;

    public DeleteCheckinUseCase(ICheckinsRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(CheckinsId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(CheckinsId.Create(id));

        return true;
    }
}