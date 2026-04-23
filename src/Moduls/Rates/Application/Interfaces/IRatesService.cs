using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Rates.Application.Interfaces;

public interface IRatesService
{
    Task<IEnumerable<Rate>> GetAllAsync();
    Task<Rate?> GetByIdAsync(int id);
    Task<IEnumerable<Rate>> GetByRouteIdAsync(int routeId);
    Task<IEnumerable<Rate>> GetByCabinTypeIdAsync(int cabinTypeId);
    Task CreateAsync(int routeId, int cabinTypeId, int passengerTypeId, int seasonId, decimal basePrice, DateOnly? validFrom, DateOnly? validUntil);
    Task UpdateAsync(int id, int routeId, int cabinTypeId, int passengerTypeId, int seasonId, decimal basePrice, DateOnly? validFrom, DateOnly? validUntil);
    Task DeleteByIdAsync(int id);
    Task<decimal> CalculatePriceAsync(int id);
}
