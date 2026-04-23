using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.ValueObject;
using AircraftManufacturerAggregate = GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate.AircraftManufacturers;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Application.UseCases;

public sealed class GetAircraftManufacturersByCountryUseCase
{
    private readonly IAircraftManufacturersRepository _repository;

    public GetAircraftManufacturersByCountryUseCase(IAircraftManufacturersRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyCollection<AircraftManufacturerAggregate>> ExecuteAsync(
        string country,
        CancellationToken cancellationToken = default)
        => _repository.GetByCountryAsync(AircraftManufacturersCountry.Create(country), cancellationToken);
}
