using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.Moduls.AircraftModels.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Application.UseCases;

public sealed class UpdateAircraftModelsUseCase
{
    private readonly IAircraftModelsRepository _repository;
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAircraftModelsUseCase(
        IAircraftModelsRepository repository,
        AppDbContext context,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<AircraftModel> ExecuteAsync(
        int id,
        int manufacturerId,
        string name,
        int maxCapacity,
        decimal? weight,
        decimal? fuelConsumption,
        int? cruiseSpeed,
        int? cruiseAltitude,
        CancellationToken cancellationToken = default)
    {
        var model = await _repository.FindByIdAsync(AircraftModelId.Create(id), cancellationToken)
            ?? throw new KeyNotFoundException($"AircraftModel with id '{id}' was not found.");

        var manufacturerExists = await _context.AircraftManufacturers
            .AsNoTracking()
            .AnyAsync(x => x.Id == manufacturerId, cancellationToken);

        if (!manufacturerExists)
            throw new InvalidOperationException($"El fabricante con ID '{manufacturerId}' no existe.");

        var duplicate = await _repository.FindByNameAsync(AircraftModelName.Create(name), cancellationToken);
        if (duplicate is not null && duplicate.Id.Value != id && duplicate.ManufacturerId.Value == manufacturerId)
            throw new InvalidOperationException($"Ya existe un modelo '{name}' para el fabricante '{manufacturerId}'.");

        model.Update(manufacturerId, name, maxCapacity, weight, fuelConsumption, cruiseSpeed, cruiseAltitude);
        await _repository.UpdateAsync(model, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return model;
    }
}
