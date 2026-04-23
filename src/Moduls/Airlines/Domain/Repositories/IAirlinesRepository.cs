using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Repositories;

public interface IAirlinesRepository
{
    Task<Airline?> GetByIdAsync(AirlinesId id);
    Task<Airline?> GetByNameAsync(AirlinesName name);
    Task<Airline?> GetByIataCodeAsync(AirlinesIataCode code);
    Task<IEnumerable<Airline>> GetByOriginCountryIdAsync(CountryId countryId);
    Task<IEnumerable<Airline>> GetAllAsync();
    Task<IEnumerable<Airline>> GetActiveAsync();
    Task SaveAsync(Airline airline);
    Task UpdateAsync(Airline airline);
    Task DeleteAsync(AirlinesId id);
    Task DeleteByNameAsync(AirlinesName name);
    Task DeleteByIataCodeAsync(AirlinesIataCode code);
    Task<int> DeleteByOriginCountryIdAsync(CountryId countryId);
    Task ReactivateAsync(AirlinesId id);
}
