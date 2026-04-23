using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.Moduls.AircraftModels.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Application.UseCases;

public sealed class CreateAircraftModelsUseCase
{
    private readonly IAircraftModelsRepository _repository;
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAircraftModelsUseCase(
        IAircraftModelsRepository repository,
        AppDbContext context,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<AircraftModel> ExecuteAsync(
        int manufacturerId,
        string name,
        int maxCapacity,
        decimal? weight,
        decimal? fuelConsumption,
        int? cruiseSpeed,
        int? cruiseAltitude,
        CancellationToken cancellationToken = default)
    {
        var manufacturerVo = AircraftManufacturersId.Create(manufacturerId);
        var nameVo = AircraftModelName.Create(name);

        var manufacturerExists = await _context.AircraftManufacturers
            .AsNoTracking()
            .AnyAsync(x => x.Id == manufacturerId, cancellationToken);

        if (!manufacturerExists)
            throw new InvalidOperationException($"El fabricante con ID '{manufacturerId}' no existe.");

        if (await _repository.ExistsByManufacturerAndNameAsync(manufacturerVo, nameVo, cancellationToken))
            throw new InvalidOperationException($"Ya existe un modelo '{nameVo.Value}' para el fabricante '{manufacturerId}'.");

        var model = AircraftModel.Create(manufacturerId, name, maxCapacity, weight, fuelConsumption, cruiseSpeed, cruiseAltitude);
        await _repository.AddAsync(model, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await _repository.FindByNameAsync(nameVo, cancellationToken)
            ?? throw new InvalidOperationException("El modelo fue creado pero no pudo recuperarse.");
    }
}
