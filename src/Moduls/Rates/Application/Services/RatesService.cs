using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Application.UseCases;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Rates.Application.Services;

public sealed class RatesService : IRatesService
{
    private readonly GetRatesUseCase _getUseCase;
    private readonly CreateRateUseCase _createUseCase;
    private readonly UpdateRateUseCase _updateUseCase;
    private readonly DeleteRateUseCase _deleteUseCase;
    private readonly CalculateRatePriceUseCase _calculateUseCase;

    public RatesService(
        GetRatesUseCase getUseCase,
        CreateRateUseCase createUseCase,
        UpdateRateUseCase updateUseCase,
        DeleteRateUseCase deleteUseCase,
        CalculateRatePriceUseCase calculateUseCase)
    {
        _getUseCase = getUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
        _calculateUseCase = calculateUseCase;
    }

    public Task<IEnumerable<Rate>> GetAllAsync() => _getUseCase.ExecuteAllAsync();
    public Task<Rate?> GetByIdAsync(int id) => _getUseCase.ExecuteByIdAsync(id);
    public Task<IEnumerable<Rate>> GetByRouteIdAsync(int routeId) => _getUseCase.ExecuteByRouteIdAsync(routeId);
    public Task<IEnumerable<Rate>> GetByCabinTypeIdAsync(int cabinTypeId) => _getUseCase.ExecuteByCabinTypeIdAsync(cabinTypeId);
    public Task CreateAsync(int routeId, int cabinTypeId, int passengerTypeId, int seasonId, decimal basePrice, DateOnly? validFrom, DateOnly? validUntil) => _createUseCase.ExecuteAsync(routeId, cabinTypeId, passengerTypeId, seasonId, basePrice, validFrom, validUntil);
    public Task UpdateAsync(int id, int routeId, int cabinTypeId, int passengerTypeId, int seasonId, decimal basePrice, DateOnly? validFrom, DateOnly? validUntil) => _updateUseCase.ExecuteAsync(id, routeId, cabinTypeId, passengerTypeId, seasonId, basePrice, validFrom, validUntil);
    public Task DeleteByIdAsync(int id) => _deleteUseCase.ExecuteByIdAsync(id);
    public Task<decimal> CalculatePriceAsync(int id) => _calculateUseCase.ExecuteAsync(id);
}
