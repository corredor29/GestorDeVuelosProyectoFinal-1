using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Application.Interfaces;

public interface ISeatLocationTypesService
{
    Task<IEnumerable<SeatLocationType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SeatLocationType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<SeatLocationType?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task CreateAsync(string name, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, string name, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task DeleteByNameAsync(string name, CancellationToken cancellationToken = default);
}
