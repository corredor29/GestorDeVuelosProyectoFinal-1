using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.ValueObject;
using AircraftManufacturerAggregate = GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate.AircraftManufacturers;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Application.UseCases;

public sealed class GetAircraftManufacturerByNameUseCase
{
    private readonly IAircraftManufacturersRepository _repository;

    public GetAircraftManufacturerByNameUseCase(IAircraftManufacturersRepository repository)
    {
        _repository = repository;
    }

    public Task<AircraftManufacturerAggregate?> ExecuteAsync(string name, CancellationToken cancellationToken = default)
        => _repository.GetByNameAsync(AircraftManufacturersName.Create(name), cancellationToken);
}
