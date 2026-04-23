using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.Repositories;

public interface ICheckinStatesRepository
{
    Task<CheckinState?> GetByIdAsync(CheckinStatesId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CheckinState>> GetAllAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(CheckinState checkinState, CancellationToken cancellationToken = default);
    Task DeleteAsync(CheckinStatesId id, CancellationToken cancellationToken = default);
}