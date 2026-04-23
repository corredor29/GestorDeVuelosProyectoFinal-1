using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using AircraftManufacturerAggregate = GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate.AircraftManufacturers;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Application.UseCases;

public sealed class CreateAircraftManufacturerUseCase
{
    private readonly IAircraftManufacturersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAircraftManufacturerUseCase(
        IAircraftManufacturersRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AircraftManufacturerAggregate> ExecuteAsync(
        string name,
        string country,
        CancellationToken cancellationToken = default)
    {
        var manufacturerName = AircraftManufacturersName.Create(name);

        if (await _repository.ExistsByNameAsync(manufacturerName, cancellationToken))
            throw new InvalidOperationException($"Ya existe un fabricante con el nombre '{manufacturerName.Value}'.");

        var manufacturer = AircraftManufacturerAggregate.Create(name, country);

        await _repository.AddAsync(manufacturer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await _repository.GetByNameAsync(manufacturerName, cancellationToken)
            ?? throw new InvalidOperationException("El fabricante fue creado pero no pudo recuperarse después de persistirse.");
    }
}
