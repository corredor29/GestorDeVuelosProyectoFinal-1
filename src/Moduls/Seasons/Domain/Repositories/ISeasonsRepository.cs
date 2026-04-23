using System.Collections.Generic;
using System.Threading.Tasks;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.Repositories;

public interface ISeasonsRepository
{
    Task<Season?> GetByIdAsync(SeasonsId id);
    Task<Season?> GetByNameAsync(SeasonsName name);
    Task<IEnumerable<Season>> GetAllAsync();
    Task SaveAsync(Season season);
    Task UpdateAsync(Season season);
    Task DeleteByIdAsync(SeasonsId id);
    Task DeleteByNameAsync(SeasonsName name);
}
