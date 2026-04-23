using GestorDeVuelosProyectoFinal.Moduls.Personal.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.Personal.Application.Services;

public sealed class PersonalService : IPersonalService
{
    private readonly GetPersonalUseCase _getUseCase;
    private readonly CreatePersonalUseCase _createUseCase;
    private readonly UpdatePersonalUseCase _updateUseCase;
    private readonly DeletePersonalUseCase _deleteUseCase;
    private readonly ReactivatePersonalUseCase _reactivateUseCase;

    public PersonalService(
        GetPersonalUseCase getUseCase,
        CreatePersonalUseCase createUseCase,
        UpdatePersonalUseCase updateUseCase,
        DeletePersonalUseCase deleteUseCase,
        ReactivatePersonalUseCase reactivateUseCase)
    {
        _getUseCase = getUseCase;
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
        _reactivateUseCase = reactivateUseCase;
    }

    public Task<IEnumerable<Staff>> GetAllAsync() => _getUseCase.ExecuteAllAsync();
    public Task<Staff?> GetByIdAsync(int id) => _getUseCase.ExecuteByIdAsync(id);
    public Task<Staff?> GetByPersonIdAsync(int personId) => _getUseCase.ExecuteByPersonIdAsync(personId);
    public Task<IEnumerable<Staff>> GetByPersonNameAsync(string personName) => _getUseCase.ExecuteByPersonNameAsync(personName);
    public Task<IEnumerable<Staff>> GetByPositionIdAsync(int positionId) => _getUseCase.ExecuteByPositionIdAsync(positionId);
    public Task<IEnumerable<Staff>> GetByPositionNameAsync(string positionName) => _getUseCase.ExecuteByPositionNameAsync(positionName);
    public Task<IEnumerable<Staff>> GetByAirlineIdAsync(int airlineId) => _getUseCase.ExecuteByAirlineIdAsync(airlineId);
    public Task<IEnumerable<Staff>> GetByAirlineNameAsync(string airlineName) => _getUseCase.ExecuteByAirlineNameAsync(airlineName);
    public Task<IEnumerable<Staff>> GetByAirportIdAsync(int airportId) => _getUseCase.ExecuteByAirportIdAsync(airportId);
    public Task<IEnumerable<Staff>> GetByAirportNameAsync(string airportName) => _getUseCase.ExecuteByAirportNameAsync(airportName);
    public Task<IEnumerable<Staff>> GetByIsActiveAsync(bool isActive) => _getUseCase.ExecuteByIsActiveAsync(isActive);
    public Task CreateAsync(int personId, int positionId, int? airlineId, int? airportId, DateTime hireDate, bool isActive) => _createUseCase.ExecuteAsync(personId, positionId, airlineId, airportId, hireDate, isActive);
    public Task UpdateAsync(int id, int personId, int positionId, int? airlineId, int? airportId, DateTime hireDate, bool isActive) => _updateUseCase.ExecuteAsync(id, personId, positionId, airlineId, airportId, hireDate, isActive);
    public Task DeactivateByIdAsync(int id) => _deleteUseCase.ExecuteByIdAsync(id);
    public Task<int> DeactivateByPersonNameAsync(string personName) => _deleteUseCase.ExecuteByPersonNameAsync(personName);
    public Task<int> DeactivateByPositionNameAsync(string positionName) => _deleteUseCase.ExecuteByPositionNameAsync(positionName);
    public Task<int> DeactivateByAirlineNameAsync(string airlineName) => _deleteUseCase.ExecuteByAirlineNameAsync(airlineName);
    public Task<int> DeactivateByAirportNameAsync(string airportName) => _deleteUseCase.ExecuteByAirportNameAsync(airportName);
    public Task ReactivateAsync(int id) => _reactivateUseCase.ExecuteAsync(id);
}
