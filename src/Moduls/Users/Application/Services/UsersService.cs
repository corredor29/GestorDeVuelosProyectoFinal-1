using BCrypt.Net;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using GestorDeVuelosProyectoFinal.src.Shared.Session;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Application.Services;

public sealed class UsersService : IUsersService
{
    private readonly IUsersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UsersService(
        IUsersRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<User> CreateAsync(
        int id,
        string username,
        string password,
        int roleId,
        int? personId = null,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByUsernameAsync(UsersUsername.Create(username));
        if (existing is not null)
            throw new InvalidOperationException($"Username '{username}' already exists.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = User.Create(id, username, passwordHash, roleId, personId);

        await _repository.SaveAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(UsersId.Create(id));
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByUsernameAsync(UsersUsername.Create(username));
    }

    public Task<IEnumerable<User>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default)
    {
        return _repository.GetByRoleIdAsync(RolesId.Create(roleId));
    }

    public Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetActiveUsersAsync();
    }

    public Task<IEnumerable<User>> GetInactiveUsersAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetInactiveUsersAsync();
    }

    public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }

    public async Task<User> UpdateAsync(
        int id,
        string? newPassword,
        int? newRoleId,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(UsersId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"User with id '{id}' was not found.");

        if (newPassword is not null)
            existing.UpdatePassword(BCrypt.Net.BCrypt.HashPassword(newPassword));

        if (newRoleId is not null)
            existing.UpdateRole(newRoleId.Value);

        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<User> ToggleActiveAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (UserSession.Current?.UserId == id)
            throw new InvalidOperationException("No puedes desactivar tu propia cuenta.");

        var existing = await _repository.GetByIdAsync(UsersId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"User with id '{id}' was not found.");

        if (existing.IsActive.Value)
            existing.Deactivate();
        else
            existing.Activate();

        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return existing;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(UsersId.Create(id));
        if (existing is null)
            return false;

        await _repository.DeleteAsync(UsersId.Create(id));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}