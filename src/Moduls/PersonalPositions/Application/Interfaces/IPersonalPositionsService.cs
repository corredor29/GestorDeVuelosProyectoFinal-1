using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Application.Interfaces;

public interface IPersonalPositionsService
{
    Task<IEnumerable<PersonalPosition>> GetAllAsync();
    Task<PersonalPosition?> GetByIdAsync(int id);
    Task<PersonalPosition?> GetByNameAsync(string name);
    Task CreateAsync(string name);
    Task UpdateAsync(int id, string name);
    Task DeleteByIdAsync(int id);
    Task DeleteByNameAsync(string name);
}
