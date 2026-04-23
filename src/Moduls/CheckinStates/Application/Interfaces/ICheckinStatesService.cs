using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Application.Interfaces;

public interface ICheckinStatesService
{
    Task<CheckinState> CreateAsync(int id, string name, CancellationToken cancellationToken = default);
    Task<CheckinState?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CheckinState>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CheckinState> UpdateAsync(int id, string newName, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}