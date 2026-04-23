using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Application.UseCases;

public sealed class GetFlightRolesUseCase
{
    private readonly IFlightRolesRepository _repository;

    public GetFlightRolesUseCase(IFlightRolesRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<FlightRole>> GetAllAsync(CancellationToken cancellationToken = default)
        => _repository.GetAllAsync(cancellationToken);

    public Task<FlightRole?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _repository.GetByIdAsync(FlightRolesId.Create(id), cancellationToken);

    public Task<FlightRole?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => _repository.GetByNameStringAsync(name, cancellationToken);
}
