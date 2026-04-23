using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Application.UseCases;

public sealed class DeleteCheckinStateUseCase
{
    private readonly ICheckinStatesRepository _CheckinStatesrepository;

    public DeleteCheckinStateUseCase(ICheckinStatesRepository CheckinStatesrepository)
    {
        _CheckinStatesrepository = CheckinStatesrepository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var checkinStateId = CheckinStatesId.Create(id);

        var existing = await _CheckinStatesrepository.GetByIdAsync(checkinStateId, cancellationToken);

        if (existing is null)
            return false;

        await _CheckinStatesrepository.DeleteAsync(checkinStateId, cancellationToken);

        return true;
    }
}