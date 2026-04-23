using System;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.Aggregate;

public sealed class RolePermission
{
    public RolePermissionsId Id { get; private set; } = null!;
    public RolesId RoleId { get; private set; } = null!;
    public PermissionsId PermissionId { get; private set; } = null!;

    private RolePermission() { }

    public static RolePermission Create(int id, int roleId, int permissionId)
    {
        return new RolePermission
        {
            Id = RolePermissionsId.Create(id),
            RoleId = RolesId.Create(roleId),
            PermissionId = PermissionsId.Create(permissionId)
        };
    }
    internal void SetId(int id)
    {
        Id = RolePermissionsId.Create(id);
    }
}