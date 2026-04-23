using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Application.UseCases;

public sealed class GetActiveSessionsByUserUseCase
{
    private readonly ISessionsRepository _repository;

    public GetActiveSessionsByUserUseCase(ISessionsRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Session>> ExecuteAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetActiveSessionsByUserIdAsync(UsersId.Create(userId));
    }
}
