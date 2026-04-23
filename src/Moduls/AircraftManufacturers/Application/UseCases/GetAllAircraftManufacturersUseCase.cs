using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Repositories;
using AircraftManufacturerAggregate = GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate.AircraftManufacturers;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Application.UseCases;

public sealed class GetAllAircraftManufacturersUseCase
{
    private readonly IAircraftManufacturersRepository _repository;

    public GetAllAircraftManufacturersUseCase(IAircraftManufacturersRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyCollection<AircraftManufacturerAggregate>> ExecuteAsync(CancellationToken cancellationToken = default)
        => _repository.GetAllAsync(cancellationToken);
}
