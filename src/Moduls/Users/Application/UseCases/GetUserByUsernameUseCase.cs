using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Application.UseCases;

public sealed class GetUserByUsernameUseCase
{
    private readonly IUsersRepository _repository;

    public GetUserByUsernameUseCase(IUsersRepository repository)
    {
        _repository = repository;
    }

    public async Task<User> ExecuteAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByUsernameAsync(UsersUsername.Create(username));
        if (result is null)
            throw new KeyNotFoundException($"User with username '{username}' was not found.");

        return result;
    }
}
