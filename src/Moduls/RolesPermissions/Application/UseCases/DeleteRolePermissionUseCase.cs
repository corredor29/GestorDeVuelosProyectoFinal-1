using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Application.UseCases;

public sealed class DeleteRolePermissionUseCase
{
    private readonly IRolePermissionsRepository _repository;

    public DeleteRolePermissionUseCase(IRolePermissionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var rolePermissionId = RolePermissionsId.Create(id);

        var existing = await _repository.GetByIdAsync(rolePermissionId);
        if (existing is null)
            return false;

        await _repository.DeleteAsync(rolePermissionId);

        return true;
    }
}