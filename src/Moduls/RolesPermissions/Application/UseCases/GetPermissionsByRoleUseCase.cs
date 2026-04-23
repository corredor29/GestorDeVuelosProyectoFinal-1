using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Application.UseCases;

public sealed class GetPermissionsByRoleUseCase
{
    private readonly IRolePermissionsRepository _repository;

    public GetPermissionsByRoleUseCase(IRolePermissionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RolePermission>> ExecuteAsync(
        int roleId,
        CancellationToken cancellationToken = default)
    {
        var rolesId = RolesId.Create(roleId);
        return await _repository.GetByRoleIdAsync(rolesId);
    }
}