using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Application.Services;

public sealed class AvailabilityStatesService : IAvailabilityStatesService
{
    private readonly GetAvailabilityStatesUseCase _getUseCase;
    private readonly CreateAvailabilityStateUseCase _createUseCase;
    private readonly UpdateAvailabilityStateUseCase _updateUseCase;
    private readonly DeleteAvailabilityStateUseCase _deleteUseCase;

    public AvailabilityStatesService(
        GetAvailabilityStatesUseCase getUseCase,
        CreateAvailabilityStateUseCase createUseCase,
        UpdateAvailabilityStateUseCase updateUseCase,
        DeleteAvailabilityStateUseCase deleteUseCase)
    {
        _getUseCase = getUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }

    public Task<IEnumerable<AvailabilityState>> GetAllAsync() => _getUseCase.ExecuteAllAsync();
    public Task<AvailabilityState?> GetByIdAsync(int id) => _getUseCase.ExecuteByIdAsync(id);
    public Task<AvailabilityState?> GetByNameAsync(string name) => _getUseCase.ExecuteByNameAsync(name);
    public Task<IEnumerable<AvailabilityState>> GetByStaffIdAsync(int staffId) => _getUseCase.ExecuteByStaffIdAsync(staffId);
    public Task CreateAsync(string name) => _createUseCase.ExecuteAsync(name);
    public Task UpdateAsync(int id, string name) => _updateUseCase.ExecuteAsync(id, name);
    public Task DeleteByIdAsync(int id) => _deleteUseCase.ExecuteByIdAsync(id);
    public Task DeleteByNameAsync(string name) => _deleteUseCase.ExecuteByNameAsync(name);
}
