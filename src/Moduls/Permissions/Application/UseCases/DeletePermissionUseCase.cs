using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Application.UseCases;

public sealed class DeletePermissionUseCase
{
    private readonly IPermissionsRepository _repository;

    public DeletePermissionUseCase(IPermissionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var permissionId = PermissionsId.Create(id);

        var existing = await _repository.GetByIdAsync(permissionId);
        if (existing is null)
            return false;

        await _repository.DeleteAsync(permissionId);

        return true;
    }
}
