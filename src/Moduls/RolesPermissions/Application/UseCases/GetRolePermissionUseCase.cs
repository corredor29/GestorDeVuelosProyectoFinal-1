using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Application.UseCases;

public sealed class GetRolePermissionUseCase
{
    private readonly IRolePermissionsRepository _repository;

    public GetRolePermissionUseCase(IRolePermissionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<RolePermission> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var rolePermissionId = RolePermissionsId.Create(id);

        var result = await _repository.GetByIdAsync(rolePermissionId);
        if (result is null)
            throw new KeyNotFoundException($"RolePermission with id '{id}' was not found.");

        return result;
    }
}
