using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.Repositories;

public interface IRatesRepository
{
    Task<Rate?> GetByIdAsync(RatesId id);
    Task<IEnumerable<Rate>> GetByCombinationAsync(RouteId routeId, CabinTypesId cabinTypeId, PassengerTypesId passengerTypeId, SeasonsId seasonId);
    Task<IEnumerable<Rate>> GetByRouteIdAsync(RouteId routeId);
    Task<IEnumerable<Rate>> GetByCabinTypeIdAsync(CabinTypesId cabinTypeId);
    Task<IEnumerable<Rate>> GetAllAsync();
    Task SaveAsync(Rate rate);
    Task UpdateAsync(Rate rate);
    Task DeleteByIdAsync(RatesId id);
    Task<bool> ExistsBySeasonIdAsync(SeasonsId seasonId);
}
