using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Application.UseCases;

public sealed class CreateRolePermissionUseCase
{
    private readonly IRolePermissionsRepository _repository;

    public CreateRolePermissionUseCase(IRolePermissionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<RolePermission> ExecuteAsync(
        int id,
        int roleId,
        int permissionId,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByRoleIdAsync(RolesId.Create(roleId));
        var alreadyExists = existing.Any(x => x.PermissionId.Value == permissionId);

        if (alreadyExists)
            throw new InvalidOperationException($"El rol '{roleId}' ya tiene asignado el permiso '{permissionId}'.");

        var rolePermission = RolePermission.Create(id, roleId, permissionId);

        await _repository.SaveAsync(rolePermission);

        return rolePermission;
    }
}
