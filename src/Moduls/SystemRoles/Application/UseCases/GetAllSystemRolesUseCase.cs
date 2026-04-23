using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Application.UseCases;

public sealed class GetAllSystemRolesUseCase
{
    private readonly ISystemRolesRepository _repository;

    public GetAllSystemRolesUseCase(ISystemRolesRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<SystemRole>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }
}