using System.Collections.Generic;
using System.Threading.Tasks;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.Repositories;

public interface ISystemRolesRepository
{
    Task<SystemRole?> GetByIdAsync(RolesId id);
    
    // Método para validar el UNIQUE de la columna 'nombre'
    Task<SystemRole?> GetByNameAsync(RolesName name); 
    Task UpdateAsync(SystemRole role); 
    Task<IEnumerable<SystemRole>> GetAllAsync();
    Task SaveAsync(SystemRole role);
    Task DeleteAsync(RolesId id);
}