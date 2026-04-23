using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Application.Services;

public sealed class AirportAirlineService : IAirportAirlineService
{
    // Este servicio centraliza búsquedas por aeropuerto, aerolínea, fechas y terminal.
    private readonly GetAirportAirlineUseCase _getUseCase;
    private readonly CreateAirportAirlineUseCase _createUseCase;
    private readonly UpdateAirportAirlineUseCase _updateUseCase;
    private readonly DeleteAirportAirlineUseCase _deleteUseCase;
    private readonly ReactivateAirportAirlineUseCase _reactivateUseCase;

    public AirportAirlineService(
        GetAirportAirlineUseCase getUseCase,
        CreateAirportAirlineUseCase createUseCase,
        UpdateAirportAirlineUseCase updateUseCase,
        DeleteAirportAirlineUseCase deleteUseCase,
        ReactivateAirportAirlineUseCase reactivateUseCase)
    {
        _getUseCase = getUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
        _reactivateUseCase = reactivateUseCase;
    }

    public Task<IEnumerable<AirportAirlineOperation>> GetAllAsync() => _getUseCase.ExecuteAllAsync();
    public Task<IEnumerable<AirportAirlineOperation>> GetActiveAsync() => _getUseCase.ExecuteActiveAsync();
    public Task<AirportAirlineOperation?> GetByIdAsync(int id) => _getUseCase.ExecuteByIdAsync(id);
    public Task<IEnumerable<AirportAirlineOperation>> GetByTerminalAsync(string terminal) => _getUseCase.ExecuteByTerminalAsync(terminal);
    public Task<IEnumerable<AirportAirlineOperation>> GetByAirportIdAsync(int airportId) => _getUseCase.ExecuteByAirportIdAsync(airportId);
    public Task<IEnumerable<AirportAirlineOperation>> GetByAirlineIdAsync(int airlineId) => _getUseCase.ExecuteByAirlineIdAsync(airlineId);
    public Task<IEnumerable<AirportAirlineOperation>> GetByStartDateAsync(DateTime startDate) => _getUseCase.ExecuteByStartDateAsync(startDate);
    public Task<IEnumerable<AirportAirlineOperation>> GetByEndDateAsync(DateTime endDate) => _getUseCase.ExecuteByEndDateAsync(endDate);
    public Task CreateAsync(int airportId, int airlineId, string? terminal, DateTime startDate, DateTime? endDate, bool isActive) => _createUseCase.ExecuteAsync(airportId, airlineId, terminal, startDate, endDate, isActive);
    public Task UpdateAsync(int id, int airportId, int airlineId, string? terminal, DateTime startDate, DateTime? endDate, bool isActive) => _updateUseCase.ExecuteAsync(id, airportId, airlineId, terminal, startDate, endDate, isActive);
    public Task DeactivateByIdAsync(int id) => _deleteUseCase.ExecuteByIdAsync(id);
    public Task<int> DeactivateByTerminalAsync(string terminal) => _deleteUseCase.ExecuteByTerminalAsync(terminal);
    public Task<int> DeactivateByAirportIdAsync(int airportId) => _deleteUseCase.ExecuteByAirportIdAsync(airportId);
    public Task<int> DeactivateByAirlineIdAsync(int airlineId) => _deleteUseCase.ExecuteByAirlineIdAsync(airlineId);
    public Task<int> DeactivateByStartDateAsync(DateTime startDate) => _deleteUseCase.ExecuteByStartDateAsync(startDate);
    public Task<int> DeactivateByEndDateAsync(DateTime endDate) => _deleteUseCase.ExecuteByEndDateAsync(endDate);
    public Task ReactivateAsync(int id) => _reactivateUseCase.ExecuteAsync(id);
}
