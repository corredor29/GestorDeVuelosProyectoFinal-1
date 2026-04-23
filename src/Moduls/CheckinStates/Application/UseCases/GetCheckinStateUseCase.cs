using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.ValueObject;


namespace GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Application.UseCases;

public sealed class GetCheckinStateUseCase
{
    private readonly ICheckinStatesRepository _CheckinStatesrepository;

    public GetCheckinStateUseCase(ICheckinStatesRepository CheckinStatesrepository)
    {
        _CheckinStatesrepository = CheckinStatesrepository;
    }

    public async Task<CheckinState> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var checkinStateId = CheckinStatesId.Create(id);

        var result = await _CheckinStatesrepository.GetByIdAsync(checkinStateId, cancellationToken);

        if (result is null)
            throw new KeyNotFoundException($"CheckinState with id '{id}' was not found.");

        return result;
    }
}