using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Application.UseCases;

public sealed class DeleteUserUseCase
{
    private readonly IUsersRepository _repository;

    public DeleteUserUseCase(IUsersRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = UsersId.Create(id);

        var existing = await _repository.GetByIdAsync(userId);
        if (existing is null)
            return false;

        await _repository.DeleteAsync(userId);

        return true;
    }
}