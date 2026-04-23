using System.Collections.Generic;
using System.Threading.Tasks;
using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.Repositories;

public interface ISeatLocationTypesRepository
{
    Task<SeatLocationType?> GetByIdAsync(SeatLocationTypesId id,CancellationToken cancellationToken = default);
    Task<SeatLocationType?> GetByNameAsync(SeatLocationTypesName name,CancellationToken cancellationToken = default);
    Task<SeatLocationType?> GetByNameStringAsync(string name,CancellationToken cancellationToken = default);
    Task<IEnumerable<SeatLocationType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(SeatLocationType seatLocationType, CancellationToken cancellationToken = default);
    Task UpdateAsync(SeatLocationType seatLocationType, CancellationToken cancellationToken = default);
    Task DeleteAsync(SeatLocationTypesId id, CancellationToken cancellationToken = default);
    Task<bool> DeleteByNameAsync(SeatLocationTypesName name, CancellationToken cancellationToken = default);
}