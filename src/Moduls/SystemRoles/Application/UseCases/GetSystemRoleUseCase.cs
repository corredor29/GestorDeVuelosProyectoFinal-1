using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Application.UseCases;

public sealed class GetSystemRoleUseCase
{
    private readonly ISystemRolesRepository _repository;

    public GetSystemRoleUseCase(ISystemRolesRepository repository)
    {
        _repository = repository;
    }

    public async Task<SystemRole?> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var roleId = RolesId.Create(id);

        var result = await _repository.GetByIdAsync(roleId);

        if (result is null)
            throw new KeyNotFoundException($"Role with id '{id}' was not found.");

        return result;
    }
}
