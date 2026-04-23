using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Application.UseCases;

public sealed class UpdateUserUseCase
{
    private readonly IUsersRepository _repository;

    public UpdateUserUseCase(IUsersRepository repository)
    {
        _repository = repository;
    }

    public async Task<User> ExecuteAsync(
        int id,
        string? newPassword = null,
        int? newRoleId = null,
        CancellationToken cancellationToken = default)
    {
        var userId = UsersId.Create(id);

        var existing = await _repository.GetByIdAsync(userId);
        if (existing is null)
            throw new KeyNotFoundException($"User with id '{id}' was not found.");

        if (newPassword is not null)
        {
            var newHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            existing.UpdatePassword(newHash);
        }

        if (newRoleId is not null)
            existing.UpdateRole(newRoleId.Value);

        await _repository.UpdateAsync(existing);

        return existing;
    }
}
