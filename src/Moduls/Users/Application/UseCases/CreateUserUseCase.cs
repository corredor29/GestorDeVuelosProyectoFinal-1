using BCrypt.Net;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Application.UseCases;

public sealed class CreateUserUseCase
{
    private readonly IUsersRepository _repository;

    public CreateUserUseCase(IUsersRepository repository)
    {
        _repository = repository;
    }

    public async Task<User> ExecuteAsync(
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

        return user;
    }
}