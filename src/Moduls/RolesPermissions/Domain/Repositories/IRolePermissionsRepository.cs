using System.Collections.Generic;
using System.Threading.Tasks;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.Repositories;

public interface IRolePermissionsRepository
{
    Task<RolePermission?> GetByIdAsync(RolePermissionsId id);
    
    // Para obtener todos los permisos asignados a un Rol
    Task<IEnumerable<RolePermission>> GetByRoleIdAsync(RolesId roleId);

    Task SaveAsync(RolePermission rolePermission);
    Task DeleteAsync(RolePermissionsId id);
}