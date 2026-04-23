using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.Application.Interfaces;

public interface IAirlinesService
{
    Task<IEnumerable<Airline>> GetAllAsync();
    Task<IEnumerable<Airline>> GetActiveAsync();
    Task<Airline?> GetByIdAsync(int id);
    Task<Airline?> GetByNameAsync(string name);
    Task<Airline?> GetByIataCodeAsync(string iataCode);
    Task<IEnumerable<Airline>> GetByOriginCountryIdAsync(int originCountryId);
    Task CreateAsync(string name, string iataCode, int originCountryId, bool isActive);
    Task UpdateAsync(int id, string name, string iataCode, int originCountryId, bool isActive);
    Task DeactivateByIdAsync(int id);
    Task DeactivateByNameAsync(string name);
    Task DeactivateByIataCodeAsync(string iataCode);
    Task<int> DeactivateByOriginCountryIdAsync(int originCountryId);
    Task ReactivateAsync(int id);
}
