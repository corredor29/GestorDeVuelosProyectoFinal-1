using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.ValueObject;
using AircraftManufacturerAggregate = GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate.AircraftManufacturers;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Application.UseCases;

public sealed class GetAircraftManufacturerByIdUseCase
{
    private readonly IAircraftManufacturersRepository _repository;

    public GetAircraftManufacturerByIdUseCase(IAircraftManufacturersRepository repository)
    {
        _repository = repository;
    }

    public Task<AircraftManufacturerAggregate?> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        => _repository.GetByIdAsync(AircraftManufacturersId.Create(id), cancellationToken);
}
