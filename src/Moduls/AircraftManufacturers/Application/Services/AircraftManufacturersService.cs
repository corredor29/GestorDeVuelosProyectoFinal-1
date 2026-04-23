using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Application.UseCases;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate;
using AircraftManufacturerAggregate = GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate.AircraftManufacturers;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Application.Services;

public sealed class AircraftManufacturersService : IAircraftManufacturersService
{
    private readonly CreateAircraftManufacturerUseCase _create;
    private readonly GetAllAircraftManufacturersUseCase _getAll;
    private readonly GetAircraftManufacturerByIdUseCase _getById;
    private readonly GetAircraftManufacturerByNameUseCase _getByName;
    private readonly GetAircraftManufacturersByCountryUseCase _getByCountry;
    private readonly UpdateAircraftManufacturerUseCase _update;
    private readonly DeleteAircraftManufacturerByIdUseCase _deleteById;
    private readonly DeleteAircraftManufacturerByNameUseCase _deleteByName;
    private readonly DeleteAircraftManufacturersByCountryUseCase _deleteByCountry;

    public AircraftManufacturersService(
        CreateAircraftManufacturerUseCase create,
        GetAllAircraftManufacturersUseCase getAll,
        GetAircraftManufacturerByIdUseCase getById,
        GetAircraftManufacturerByNameUseCase getByName,
        GetAircraftManufacturersByCountryUseCase getByCountry,
        UpdateAircraftManufacturerUseCase update,
        DeleteAircraftManufacturerByIdUseCase deleteById,
        DeleteAircraftManufacturerByNameUseCase deleteByName,
        DeleteAircraftManufacturersByCountryUseCase deleteByCountry)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByName = getByName;
        _getByCountry = getByCountry;
        _update = update;
        _deleteById = deleteById;
        _deleteByName = deleteByName;
        _deleteByCountry = deleteByCountry;
    }

    public Task<AircraftManufacturerAggregate> CreateAsync(string name, string country, CancellationToken cancellationToken = default)
        => _create.ExecuteAsync(name, country, cancellationToken);

    public Task<IReadOnlyCollection<AircraftManufacturerAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
        => _getAll.ExecuteAsync(cancellationToken);

    public Task<AircraftManufacturerAggregate?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _getById.ExecuteAsync(id, cancellationToken);

    public Task<AircraftManufacturerAggregate?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => _getByName.ExecuteAsync(name, cancellationToken);

    public Task<IReadOnlyCollection<AircraftManufacturerAggregate>> GetByCountryAsync(string country, CancellationToken cancellationToken = default)
        => _getByCountry.ExecuteAsync(country, cancellationToken);

    public Task<AircraftManufacturerAggregate> UpdateAsync(int id, string name, string country, CancellationToken cancellationToken = default)
        => _update.ExecuteAsync(id, name, country, cancellationToken);

    public Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
        => _deleteById.ExecuteAsync(id, cancellationToken);

    public Task<bool> DeleteByNameAsync(string name, CancellationToken cancellationToken = default)
        => _deleteByName.ExecuteAsync(name, cancellationToken);

    public Task<int> DeleteByCountryAsync(string country, CancellationToken cancellationToken = default)
        => _deleteByCountry.ExecuteAsync(country, cancellationToken);
}
