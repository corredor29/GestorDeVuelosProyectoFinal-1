using GestorDeVuelosProyectoFinal.Moduls.Seasons.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.Seasons.Application.Interfaces;

public interface ISeasonsService
{
    Task<IEnumerable<Season>> GetAllAsync();
    Task<Season?> GetByIdAsync(int id);
    Task<Season?> GetByNameAsync(string name);
    Task CreateAsync(string name, string? description, decimal priceFactor);
    Task UpdateAsync(int id, string name, string? description, decimal priceFactor);
    Task DeleteByIdAsync(int id);
    Task DeleteByNameAsync(string name);
}
