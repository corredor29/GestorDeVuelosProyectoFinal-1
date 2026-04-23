using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Application.Interfaces;

public interface IBaggageService
{
    Task<BaggageRoot> CreateAsync(int id, int checkinId, int baggageTypeId, decimal weightKg, decimal chargedPrice, CancellationToken cancellationToken = default);
    Task<BaggageRoot?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BaggageRoot>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BaggageRoot> UpdateAsync(int id, decimal? newWeightKg, decimal? newChargedPrice, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}