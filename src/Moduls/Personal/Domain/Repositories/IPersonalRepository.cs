using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.Repositories;

public interface IPersonalRepository
{
    Task<Staff?> GetByIdAsync(PersonalId id);
    Task<Staff?> GetByPersonIdAsync(PeopleId personId);
    Task<IEnumerable<Staff>> GetByPersonNameAsync(string personName);
    Task<IEnumerable<Staff>> GetByPositionIdAsync(PersonalPositionsId positionId);
    Task<IEnumerable<Staff>> GetByPositionNameAsync(string positionName);
    Task<IEnumerable<Staff>> GetByAirlineIdAsync(AirlinesId airlineId);
    Task<IEnumerable<Staff>> GetByAirlineNameAsync(string airlineName);
    Task<IEnumerable<Staff>> GetByAirportIdAsync(AirportsId airportId);
    Task<IEnumerable<Staff>> GetByAirportNameAsync(string airportName);
    Task<IEnumerable<Staff>> GetByIsActiveAsync(bool isActive);
    Task<IEnumerable<Staff>> GetAllAsync();
    Task SaveAsync(Staff staff);
    Task UpdateAsync(Staff staff);
    Task DeleteAsync(PersonalId id);
    Task<int> DeleteByPersonNameAsync(string personName);
    Task<int> DeleteByPositionNameAsync(string positionName);
    Task<int> DeleteByAirlineNameAsync(string airlineName);
    Task<int> DeleteByAirportNameAsync(string airportName);
    Task ReactivateAsync(PersonalId id);
}
