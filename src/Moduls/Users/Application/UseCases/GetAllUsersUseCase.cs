using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Application.UseCases;

public sealed class GetAllUsersUseCase
{
    private readonly IUsersRepository _repository;

    public GetAllUsersUseCase(IUsersRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<User>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }
}
