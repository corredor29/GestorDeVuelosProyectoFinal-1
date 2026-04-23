using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Application.UseCases;

public sealed class CreatePermissionUseCase
{
    private readonly IPermissionsRepository _repository;

    public CreatePermissionUseCase(IPermissionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Permission> ExecuteAsync(
        int id,
        string name,
        string? description,
        CancellationToken cancellationToken = default)
    {
        var permissionName = PermissionsName.Create(name);

        var existing = await _repository.GetByNameAsync(permissionName);
        if (existing is not null)
            throw new InvalidOperationException($"Permission with name '{name}' already exists.");

        var permission = Permission.Create(id, name, description);

        await _repository.SaveAsync(permission);

        return permission;
    }
}