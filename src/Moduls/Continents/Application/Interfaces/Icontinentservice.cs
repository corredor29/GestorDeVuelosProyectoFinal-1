using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.Continents.Application.Interfaces;

public interface IContinentService
{
    Task<IEnumerable<Continent>> GetAllAsync();
    Task<Continent?> GetByNameAsync(string name);
    Task CreateAsync(string name);
    Task UpdateAsync(string currentName, string newName);
    Task DeleteAsync(string name);
}