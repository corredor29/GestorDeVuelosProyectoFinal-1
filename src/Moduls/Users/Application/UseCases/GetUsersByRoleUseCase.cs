using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Application.UseCases;

public sealed class GetUsersByRoleUseCase
{
    private readonly IUsersRepository _repository;

    public GetUsersByRoleUseCase(IUsersRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<User>> ExecuteAsync(
        int roleId,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetByRoleIdAsync(RolesId.Create(roleId));
    }
}