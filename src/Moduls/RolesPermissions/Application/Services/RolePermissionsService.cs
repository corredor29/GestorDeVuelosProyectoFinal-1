using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Application.Services;

public sealed class RolePermissionsService : IRolePermissionsService
{
    private readonly IRolePermissionsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RolePermissionsService(
        IRolePermissionsRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RolePermission> CreateAsync(
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
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return rolePermission;
    }

    public async Task<RolePermission?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var rolePermissionId = RolePermissionsId.Create(id);
        return await _repository.GetByIdAsync(rolePermissionId);
    }

    public async Task<IEnumerable<RolePermission>> GetByRoleIdAsync(
        int roleId,
        CancellationToken cancellationToken = default)
    {
        var rolesId = RolesId.Create(roleId);
        return await _repository.GetByRoleIdAsync(rolesId);
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var rolePermissionId = RolePermissionsId.Create(id);

        var existing = await _repository.GetByIdAsync(rolePermissionId);
        if (existing is null)
            return false;

        await _repository.DeleteAsync(rolePermissionId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
