using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Application.UseCases;

public sealed class GetAllCheckinStatesUseCase
{
    private readonly ICheckinStatesRepository _CheckinStatesrepository;

    public GetAllCheckinStatesUseCase(ICheckinStatesRepository CheckinStatesrepository)
    {
        _CheckinStatesrepository = CheckinStatesrepository;
    }

    public Task<IEnumerable<CheckinState>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return _CheckinStatesrepository.GetAllAsync(cancellationToken);
    }
}
