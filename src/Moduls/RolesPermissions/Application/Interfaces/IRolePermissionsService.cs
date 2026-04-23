using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Application.Interfaces;

public interface IRolePermissionsService
{
    Task<RolePermission> CreateAsync(int id, int roleId, int permissionId, CancellationToken cancellationToken = default);
    Task<RolePermission?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<RolePermission>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}