using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Session;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Application.UseCases;

public sealed class ToggleUserActiveUseCase
{
    private readonly IUsersRepository _repository;

    public ToggleUserActiveUseCase(IUsersRepository repository)
    {
        _repository = repository;
    }

    public async Task<User> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var userId = UsersId.Create(id);

        if (UserSession.Current?.UserId == id)
            throw new InvalidOperationException("No puedes desactivar tu propia cuenta.");

        var existing = await _repository.GetByIdAsync(userId);
        if (existing is null)
            throw new KeyNotFoundException($"User with id '{id}' was not found.");

        if (existing.IsActive.Value)
            existing.Deactivate();
        else
            existing.Activate();

        await _repository.UpdateAsync(existing);

        return existing;
    }
}