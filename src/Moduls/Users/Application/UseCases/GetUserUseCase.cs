using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Application.UseCases;

public sealed class GetUserUseCase
{
    private readonly IUsersRepository _repository;

    public GetUserUseCase(IUsersRepository repository)
    {
        _repository = repository;
    }

    public async Task<User> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = UsersId.Create(id);

        var result = await _repository.GetByIdAsync(userId);
        if (result is null)
            throw new KeyNotFoundException($"User with id '{id}' was not found.");

        return result;
    }
}