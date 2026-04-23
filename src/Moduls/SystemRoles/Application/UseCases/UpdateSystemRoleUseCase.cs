using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Application.UseCases;

public sealed class UpdateSystemRoleUseCase
{
    private readonly ISystemRolesRepository _repository;

    public UpdateSystemRoleUseCase(ISystemRolesRepository repository)
    {
        _repository = repository;
    }

    public async Task<SystemRole?> ExecuteAsync(
        int id,
        string? newName,
        string? newDescription,
        CancellationToken cancellationToken = default)
    {
        var roleId = RolesId.Create(id);

        var existing = await _repository.GetByIdAsync(roleId);

        if (existing is null)
            throw new KeyNotFoundException($"Role with id '{id}' was not found.");

        if (newName is not null)
        {
            var nameInUse = await _repository.GetByNameAsync(RolesName.Create(newName));
            if (nameInUse is not null)
                throw new InvalidOperationException($"Role with name '{newName}' already exists.");

            existing.UpdateName(newName);
        }

        if (newDescription is not null)
            existing.UpdateDescription(newDescription);

        await _repository.SaveAsync(existing);

        return existing;
    }
}