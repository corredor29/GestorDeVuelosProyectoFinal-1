using System.Collections.Generic;
using System.Threading.Tasks;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.Repositories;

public interface IPermissionsRepository
{
    Task<Permission?> GetByIdAsync(PermissionsId id);
    Task<Permission?> GetByNameAsync(PermissionsName name); // Para el UNIQUE nombre
    Task<IEnumerable<Permission>> GetAllAsync();
    Task UpdateAsync(Permission permission);
    Task SaveAsync(Permission permission);
    Task DeleteAsync(PermissionsId id);
}