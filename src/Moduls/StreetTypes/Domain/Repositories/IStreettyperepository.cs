using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.Repositories;

public interface IStreetTypesRepository
{
    Task<StreetType?> GetByIdAsync(StreetTypeId id);
    Task<StreetType?> GetByNameAsync(string name); // Nuevo
    Task<IEnumerable<StreetType>> GetAllAsync();
    Task AddAsync(StreetType streetType);
    Task SaveAsync(StreetType streetType);
    Task DeleteAsync(StreetTypeId id);
    Task DeleteByNameAsync(string name); // Nuevo
}