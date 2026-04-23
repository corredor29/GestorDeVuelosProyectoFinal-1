using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Rates.Application.UseCases;

public sealed class GetRatesUseCase
{
    private readonly IRatesRepository _repository;

    public GetRatesUseCase(IRatesRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Rate>> ExecuteAllAsync() => _repository.GetAllAsync();
    public Task<Rate?> ExecuteByIdAsync(int id) => _repository.GetByIdAsync(RatesId.Create(id));
    public Task<IEnumerable<Rate>> ExecuteByCombinationAsync(int routeId, int cabinTypeId, int passengerTypeId, int seasonId)
        => _repository.GetByCombinationAsync(RouteId.Create(routeId), CabinTypesId.Create(cabinTypeId), PassengerTypesId.Create(passengerTypeId), SeasonsId.Create(seasonId));
    public Task<IEnumerable<Rate>> ExecuteByRouteIdAsync(int routeId) => _repository.GetByRouteIdAsync(RouteId.Create(routeId));
    public Task<IEnumerable<Rate>> ExecuteByCabinTypeIdAsync(int cabinTypeId) => _repository.GetByCabinTypeIdAsync(CabinTypesId.Create(cabinTypeId));
}
