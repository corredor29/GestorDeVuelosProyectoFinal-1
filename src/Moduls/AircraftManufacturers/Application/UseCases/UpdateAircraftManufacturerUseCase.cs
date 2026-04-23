using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using AircraftManufacturerAggregate = GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate.AircraftManufacturers;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Application.UseCases;

public sealed class UpdateAircraftManufacturerUseCase
{
    private readonly IAircraftManufacturersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAircraftManufacturerUseCase(
        IAircraftManufacturersRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AircraftManufacturerAggregate> ExecuteAsync(
        int id,
        string name,
        string country,
        CancellationToken cancellationToken = default)
    {
        var manufacturerId = AircraftManufacturersId.Create(id);
        var manufacturer = await _repository.GetByIdAsync(manufacturerId, cancellationToken)
            ?? throw new KeyNotFoundException($"No se encontró el fabricante con ID '{id}'.");

        var duplicate = await _repository.GetByNameAsync(AircraftManufacturersName.Create(name), cancellationToken);
        if (duplicate is not null && duplicate.Id.Value != id)
            throw new InvalidOperationException($"Ya existe otro fabricante con el nombre '{name}'.");

        manufacturer.Update(name, country);

        await _repository.UpdateAsync(manufacturer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return manufacturer;
    }
}
