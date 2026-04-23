using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Application.UseCases;

public sealed class UpdateCheckinStateUseCase
{
    private readonly ICheckinStatesRepository _CheckinStatesrepository;

    public UpdateCheckinStateUseCase(ICheckinStatesRepository CheckinStatesrepository)
    {
        _CheckinStatesrepository = CheckinStatesrepository;
    }

    public async Task<CheckinState> ExecuteAsync(
        int id,
        string newName,
        CancellationToken cancellationToken = default)
    {
        var checkinStateId = CheckinStatesId.Create(id);

        var existing = await _CheckinStatesrepository.GetByIdAsync(checkinStateId, cancellationToken);

        if (existing is null)
            throw new KeyNotFoundException($"CheckinState with id '{id}' was not found.");

        existing.UpdateName(newName);

        await _CheckinStatesrepository.SaveAsync(existing, cancellationToken);

        return existing;
    }
}