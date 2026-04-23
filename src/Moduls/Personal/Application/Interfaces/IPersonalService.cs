using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.Personal.Application.Interfaces;

public interface IPersonalService
{
    Task<IEnumerable<Staff>> GetAllAsync();
    Task<Staff?> GetByIdAsync(int id);
    Task<Staff?> GetByPersonIdAsync(int personId);
    Task<IEnumerable<Staff>> GetByPersonNameAsync(string personName);
    Task<IEnumerable<Staff>> GetByPositionIdAsync(int positionId);
    Task<IEnumerable<Staff>> GetByPositionNameAsync(string positionName);
    Task<IEnumerable<Staff>> GetByAirlineIdAsync(int airlineId);
    Task<IEnumerable<Staff>> GetByAirlineNameAsync(string airlineName);
    Task<IEnumerable<Staff>> GetByAirportIdAsync(int airportId);
    Task<IEnumerable<Staff>> GetByAirportNameAsync(string airportName);
    Task<IEnumerable<Staff>> GetByIsActiveAsync(bool isActive);
    Task CreateAsync(int personId, int positionId, int? airlineId, int? airportId, DateTime hireDate, bool isActive);
    Task UpdateAsync(int id, int personId, int positionId, int? airlineId, int? airportId, DateTime hireDate, bool isActive);
    Task DeactivateByIdAsync(int id);
    Task<int> DeactivateByPersonNameAsync(string personName);
    Task<int> DeactivateByPositionNameAsync(string positionName);
    Task<int> DeactivateByAirlineNameAsync(string airlineName);
    Task<int> DeactivateByAirportNameAsync(string airportName);
    Task ReactivateAsync(int id);
}
