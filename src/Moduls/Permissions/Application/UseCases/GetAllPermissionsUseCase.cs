using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Application.UseCases;

public sealed class GetAllPermissionsUseCase
{
    private readonly IPermissionsRepository _repository;

    public GetAllPermissionsUseCase(IPermissionsRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Permission>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }
}
