using GestorDeVuelosProyectoFinal.Moduls.Airlines.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.Application.Services;

public sealed class AirlinesService : IAirlinesService
{
    // Este servicio solo orquesta casos de uso para dejar la UI lo más directa posible.
    private readonly GetAirlinesUseCase _getUseCase;
    private readonly CreateAirlineUseCase _createUseCase;
    private readonly UpdateAirlineUseCase _updateUseCase;
    private readonly DeleteAirlineUseCase _deleteUseCase;
    private readonly ReactivateAirlineUseCase _reactivateUseCase;

    public AirlinesService(
        GetAirlinesUseCase getUseCase,
        CreateAirlineUseCase createUseCase,
        UpdateAirlineUseCase updateUseCase,
        DeleteAirlineUseCase deleteUseCase,
        ReactivateAirlineUseCase reactivateUseCase)
    {
        _getUseCase = getUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
        _reactivateUseCase = reactivateUseCase;
    }

    // Se exponen listados completos y activos porque el admin usa ambos con frecuencia.
    public Task<IEnumerable<Airline>> GetAllAsync() => _getUseCase.ExecuteAllAsync();
    public Task<IEnumerable<Airline>> GetActiveAsync() => _getUseCase.ExecuteActiveAsync();
    public Task<Airline?> GetByIdAsync(int id) => _getUseCase.ExecuteByIdAsync(id);
    public Task<Airline?> GetByNameAsync(string name) => _getUseCase.ExecuteByNameAsync(name);
    public Task<Airline?> GetByIataCodeAsync(string iataCode) => _getUseCase.ExecuteByIataCodeAsync(iataCode);
    public Task<IEnumerable<Airline>> GetByOriginCountryIdAsync(int originCountryId) => _getUseCase.ExecuteByOriginCountryIdAsync(originCountryId);
    public Task CreateAsync(string name, string iataCode, int originCountryId, bool isActive) => _createUseCase.ExecuteAsync(name, iataCode, originCountryId, isActive);
    public Task UpdateAsync(int id, string name, string iataCode, int originCountryId, bool isActive) => _updateUseCase.ExecuteAsync(id, name, iataCode, originCountryId, isActive);
    public Task DeactivateByIdAsync(int id) => _deleteUseCase.ExecuteByIdAsync(id);
    public Task DeactivateByNameAsync(string name) => _deleteUseCase.ExecuteByNameAsync(name);
    public Task DeactivateByIataCodeAsync(string iataCode) => _deleteUseCase.ExecuteByIataCodeAsync(iataCode);
    public Task<int> DeactivateByOriginCountryIdAsync(int originCountryId) => _deleteUseCase.ExecuteByOriginCountryIdAsync(originCountryId);
    public Task ReactivateAsync(int id) => _reactivateUseCase.ExecuteAsync(id);
}
