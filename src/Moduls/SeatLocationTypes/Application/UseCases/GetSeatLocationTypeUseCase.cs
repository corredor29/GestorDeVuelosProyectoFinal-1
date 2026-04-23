using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Application.UseCases;

public sealed class GetSeatLocationTypeUseCase
{
    private readonly ISeatLocationTypesRepository _repository;

    public GetSeatLocationTypeUseCase(ISeatLocationTypesRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<SeatLocationType>> GetAllAsync(CancellationToken cancellationToken = default)
        => _repository.GetAllAsync(cancellationToken);

    public Task<SeatLocationType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _repository.GetByIdAsync(SeatLocationTypesId.Create(id), cancellationToken);

    public Task<SeatLocationType?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => _repository.GetByNameStringAsync(name, cancellationToken);
}
