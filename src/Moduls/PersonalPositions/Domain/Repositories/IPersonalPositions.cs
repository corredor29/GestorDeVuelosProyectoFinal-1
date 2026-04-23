using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.Repositories;

public interface IPersonalPositionsRepository
{
    Task<PersonalPosition?> GetByIdAsync(PersonalPositionsId id);
    Task<PersonalPosition?> GetByNameAsync(PersonalPositionsName name);
    Task<IEnumerable<PersonalPosition>> GetAllAsync();
    Task SaveAsync(PersonalPosition position);
    Task UpdateAsync(PersonalPosition position);
    Task DeleteAsync(PersonalPositionsId id);
    Task DeleteByNameAsync(PersonalPositionsName name);
}
