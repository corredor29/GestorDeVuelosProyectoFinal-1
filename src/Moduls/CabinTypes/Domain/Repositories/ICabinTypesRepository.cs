using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.Repositories;

public interface ICabinTypesRepository
{
    Task<CabinType?> GetByIdAsync(CabinTypesId id);
    Task<CabinType?> GetByNameAsync(CabinTypesName name);
    Task<CabinType?> GetByNameStringAsync(string name);
    Task<IEnumerable<CabinType>> GetAllAsync();
    Task SaveAsync(CabinType cabinType);
    Task UpdateAsync(CabinType cabinType);
    Task DeleteAsync(CabinTypesId id);
}
