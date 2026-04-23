using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Application.UseCases;

public sealed class UpdatePermissionUseCase
{
    private readonly IPermissionsRepository _repository;

    public UpdatePermissionUseCase(IPermissionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Permission> ExecuteAsync(
        int id,
        string? newName,
        string? newDescription,
        CancellationToken cancellationToken = default)
    {
        var permissionId = PermissionsId.Create(id);

        var existing = await _repository.GetByIdAsync(permissionId);
        if (existing is null)
            throw new KeyNotFoundException($"Permission with id '{id}' was not found.");

        if (newName is not null)
        {
            var nameInUse = await _repository.GetByNameAsync(PermissionsName.Create(newName));
            if (nameInUse is not null)
                throw new InvalidOperationException($"Permission with name '{newName}' already exists.");

            existing.UpdateName(newName);
        }

        if (newDescription is not null)
            existing.UpdateDescription(newDescription);

        await _repository.SaveAsync(existing);

        return existing;
    }
}
