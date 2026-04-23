using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Application.Interfaces;

public interface IBaggageTypesService
{
    Task<BaggageType> CreateAsync(int id, string name, decimal maxWeightKg, decimal basePrice, CancellationToken cancellationToken = default);
    Task<BaggageType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BaggageType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BaggageType> UpdateAsync(int id, string? newName, decimal? newMaxWeightKg, decimal? newBasePrice, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
