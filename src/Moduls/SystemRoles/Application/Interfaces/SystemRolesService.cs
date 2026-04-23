using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Roles.Application.Services;

public sealed class SystemRolesService : ISystemRolesService
{
    private readonly ISystemRolesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SystemRolesService(
        ISystemRolesRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SystemRole> CreateAsync(
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
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return role;
    }

    public async Task<SystemRole?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var roleId = RolesId.Create(id);
        return await _repository.GetByIdAsync(roleId);
    }

    public Task<IEnumerable<SystemRole>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }

    public async Task<SystemRole> UpdateAsync(
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
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var roleId = RolesId.Create(id);

        var existing = await _repository.GetByIdAsync(roleId);
        if (existing is null)
            return false;

        await _repository.DeleteAsync(roleId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}