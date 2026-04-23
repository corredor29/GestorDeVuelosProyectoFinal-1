using GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Application.UseCases;
using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Application.Services;

public sealed class SeatLocationTypesService : ISeatLocationTypesService
{
    private readonly GetSeatLocationTypeUseCase _get;
    private readonly CreateSeatLocationTypeUseCase _create;
    private readonly UpdateSeatLocationTypeUseCase _update;
    private readonly DeleteSeatLocationTypeUseCase _delete;

    public SeatLocationTypesService(
        GetSeatLocationTypeUseCase get,
        CreateSeatLocationTypeUseCase create,
        UpdateSeatLocationTypeUseCase update,
        DeleteSeatLocationTypeUseCase delete)
    {
        _get = get;
        _create = create;
        _update = update;
        _delete = delete;
    }

    public Task<IEnumerable<SeatLocationType>> GetAllAsync(CancellationToken cancellationToken = default)
        => _get.GetAllAsync(cancellationToken);

    public Task<SeatLocationType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _get.GetByIdAsync(id, cancellationToken);

    public Task<SeatLocationType?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => _get.GetByNameAsync(name, cancellationToken);

    public Task CreateAsync(string name, CancellationToken cancellationToken = default)
        => _create.ExecuteAsync(name, cancellationToken);

    public Task UpdateAsync(int id, string name, CancellationToken cancellationToken = default)
        => _update.ExecuteAsync(id, name, cancellationToken);
    public Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
        => _delete.ExecuteByIdAsync(id, cancellationToken);

    public Task DeleteByNameAsync(string name, CancellationToken cancellationToken = default)
        => _delete.ExecuteByNameAsync(name, cancellationToken);
}
