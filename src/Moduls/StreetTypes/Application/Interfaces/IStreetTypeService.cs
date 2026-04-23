using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Application.Interfaces;

public interface IStreetTypeService
{
    Task<IEnumerable<StreetType>> GetAllAsync();
    Task<StreetType?> GetByIdAsync(int id);
    Task<StreetType?> GetByNameAsync(string name);
    Task CreateAsync(string name);
    Task UpdateAsync(int id, string newName);
    Task DeleteAsync(int id);
    Task DeleteByNameAsync(string name);
}