using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Application.UseCases;

public sealed class GetPermissionUseCase
{
    private readonly IPermissionsRepository _repository;

    public GetPermissionUseCase(IPermissionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Permission> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var permissionId = PermissionsId.Create(id);

        var result = await _repository.GetByIdAsync(permissionId);
        if (result is null)
            throw new KeyNotFoundException($"Permission with id '{id}' was not found.");

        return result;
    }
}
