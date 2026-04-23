using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Application.Services;

public sealed class PermissionsService : IPermissionsService
{
    private readonly IPermissionsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PermissionsService(
        IPermissionsRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Permission> CreateAsync(
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
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return permission;
    }

    public async Task<Permission?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var permissionId = PermissionsId.Create(id);
        return await _repository.GetByIdAsync(permissionId);
    }

    public Task<IEnumerable<Permission>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }

    public async Task<Permission> UpdateAsync(
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

        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var permissionId = PermissionsId.Create(id);

        var existing = await _repository.GetByIdAsync(permissionId);
        if (existing is null)
            return false;

        await _repository.DeleteAsync(permissionId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}