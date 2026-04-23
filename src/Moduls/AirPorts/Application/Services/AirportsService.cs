using GestorDeVuelosProyectoFinal.Moduls.Airports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.Airports.Application.Services;

public sealed class AirportsService : IAirportsService
{
    // El servicio mantiene agrupados todos los accesos típicos por código, nombre y ciudad.
    private readonly GetAirportsUseCase _getUseCase;
    private readonly CreateAirportUseCase _createUseCase;
    private readonly UpdateAirportUseCase _updateUseCase;
    private readonly DeleteAirportUseCase _deleteUseCase;

    public AirportsService(
        GetAirportsUseCase getUseCase,
        CreateAirportUseCase createUseCase,
        UpdateAirportUseCase updateUseCase,
        DeleteAirportUseCase deleteUseCase)
    {
        _getUseCase = getUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }

    public Task<IEnumerable<Airport>> GetAllAsync() => _getUseCase.ExecuteAllAsync();
    public Task<Airport?> GetByIdAsync(int id) => _getUseCase.ExecuteByIdAsync(id);
    public Task<Airport?> GetByNameAsync(string name) => _getUseCase.ExecuteByNameAsync(name);
    public Task<Airport?> GetByIataCodeAsync(string iataCode) => _getUseCase.ExecuteByIataCodeAsync(iataCode);
    public Task<Airport?> GetByIcaoCodeAsync(string? icaoCode) => _getUseCase.ExecuteByIcaoCodeAsync(icaoCode);
    public Task<IEnumerable<Airport>> GetByCityIdAsync(int cityId) => _getUseCase.ExecuteByCityIdAsync(cityId);
    public Task<IEnumerable<Airport>> GetByCityNameAsync(string cityName) => _getUseCase.ExecuteByCityNameAsync(cityName);
    public Task CreateAsync(string name, string iataCode, string? icaoCode, int cityId) => _createUseCase.ExecuteAsync(name, iataCode, icaoCode, cityId);
    public Task UpdateAsync(int id, string name, string iataCode, string? icaoCode, int cityId) => _updateUseCase.ExecuteAsync(id, name, iataCode, icaoCode, cityId);
    public Task DeleteByIdAsync(int id) => _deleteUseCase.ExecuteByIdAsync(id);
    public Task DeleteByNameAsync(string name) => _deleteUseCase.ExecuteByNameAsync(name);
    public Task DeleteByIataCodeAsync(string iataCode) => _deleteUseCase.ExecuteByIataCodeAsync(iataCode);
    public Task DeleteByIcaoCodeAsync(string icaoCode) => _deleteUseCase.ExecuteByIcaoCodeAsync(icaoCode);
}
