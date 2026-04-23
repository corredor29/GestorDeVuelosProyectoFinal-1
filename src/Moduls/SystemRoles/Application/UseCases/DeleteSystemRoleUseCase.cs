using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Application.UseCases;

public sealed class DeleteSystemRoleUseCase
{
    private readonly ISystemRolesRepository _repository;

    public DeleteSystemRoleUseCase(ISystemRolesRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var roleId = RolesId.Create(id);

        var existing = await _repository.GetByIdAsync(roleId);

        if (existing is null)
            return false;

        await _repository.DeleteAsync(roleId);

        return true;
    }
}