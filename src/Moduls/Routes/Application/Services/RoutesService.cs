using GestorDeVuelosProyectoFinal.Moduls.Routes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.Routes.Application.Services;

public sealed class RoutesService : IRoutesService
{
    // La capa de aplicación expone las combinaciones más usadas por el menú de rutas.
    private readonly GetRoutesUseCase _getUseCase;
    private readonly CreateRouteUseCase _createUseCase;
    private readonly UpdateRouteUseCase _updateUseCase;
    private readonly DeleteRouteUseCase _deleteUseCase;

    public RoutesService(
        GetRoutesUseCase getUseCase,
        CreateRouteUseCase createUseCase,
        UpdateRouteUseCase updateUseCase,
        DeleteRouteUseCase deleteUseCase)
    {
        _getUseCase = getUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }

    public Task<IEnumerable<Route>> GetAllAsync() => _getUseCase.ExecuteAllAsync();
    public Task<Route?> GetByIdAsync(int id) => _getUseCase.ExecuteByIdAsync(id);
    public Task<IEnumerable<Route>> GetByOriginAirportIdAsync(int originAirportId) => _getUseCase.ExecuteByOriginAirportIdAsync(originAirportId);
    public Task<IEnumerable<Route>> GetByDestinationAirportIdAsync(int destinationAirportId) => _getUseCase.ExecuteByDestinationAirportIdAsync(destinationAirportId);
    public Task<Route?> GetByOriginAndDestinationAsync(int originAirportId, int destinationAirportId) => _getUseCase.ExecuteByOriginAndDestinationAsync(originAirportId, destinationAirportId);
    public Task CreateAsync(int originAirportId, int destinationAirportId, int? distanceKm, int? estimatedDurationMin) => _createUseCase.ExecuteAsync(originAirportId, destinationAirportId, distanceKm, estimatedDurationMin);
    public Task UpdateAsync(int id, int originAirportId, int destinationAirportId, int? distanceKm, int? estimatedDurationMin) => _updateUseCase.ExecuteAsync(id, originAirportId, destinationAirportId, distanceKm, estimatedDurationMin);
    public Task DeleteByIdAsync(int id) => _deleteUseCase.ExecuteByIdAsync(id);
}
