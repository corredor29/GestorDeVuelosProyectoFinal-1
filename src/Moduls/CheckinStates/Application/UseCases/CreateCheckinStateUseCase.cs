using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Application.UseCases;

public sealed class CreateCheckinStateUseCase
{
    private readonly ICheckinStatesRepository _CheckinStatesrepository;

    public CreateCheckinStateUseCase(ICheckinStatesRepository CheckinStatesrepository)
    {
        _CheckinStatesrepository = CheckinStatesrepository;
    }

    public async Task<CheckinState> ExecuteAsync(
        int id,
        string name,
        CancellationToken cancellationToken = default)
    {
        var existing = await _CheckinStatesrepository.GetByIdAsync(CheckinStatesId.Create(id), cancellationToken);

        if (existing is not null)
            throw new InvalidOperationException($"CheckinState with id '{id}' already exists.");

        var checkinState = CheckinState.Create(id, name);

        await _CheckinStatesrepository.SaveAsync(checkinState, cancellationToken);

        return checkinState;
    }
}
