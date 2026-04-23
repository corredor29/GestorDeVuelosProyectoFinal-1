using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Application.UseCases;

public sealed class CreateSystemRoleUseCase
{
    private readonly ISystemRolesRepository _repository;

    public CreateSystemRoleUseCase(ISystemRolesRepository repository)
    {
        _repository = repository;
    }

    public async Task<SystemRole> ExecuteAsync(
        int id,
        string name,
        string? description,
        CancellationToken cancellationToken = default)
    {
        var roleName = RolesName.Create(name);

        var existing = await _repository.GetByNameAsync(roleName);

        if (existing is not null)
            throw new InvalidOperationException($"Role with name '{name}' already exists.");

        var role = SystemRole.Create(id, name, description);

        await _repository.SaveAsync(role);

        return role;
    }
}