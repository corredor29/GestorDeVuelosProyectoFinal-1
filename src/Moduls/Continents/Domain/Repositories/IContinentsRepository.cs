using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.Repositories;

public interface IContinentsRepository
{
    Task<Continent?> GetByIdAsync(ContinentsId id);
    Task<IEnumerable<Continent>> GetAllAsync();
    Task<Continent?> GetByNameAsync(string name);
    Task SaveAsync(Continent continent);
    Task UpdateAsync(Continent continent);
    Task DeleteByNameAsync(string name);
    Task DeleteAsync(ContinentsId id);
}