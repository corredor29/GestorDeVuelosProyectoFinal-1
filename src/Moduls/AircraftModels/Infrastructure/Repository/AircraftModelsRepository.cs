using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.Moduls.AircraftModels.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Infrastructure.Repository;

public sealed class AircraftModelsRepository : IAircraftModelsRepository
{
    private readonly AppDbContext _context;

    public AircraftModelsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AircraftModel model, CancellationToken cancellationToken = default)
    {
        await _context.AircraftModels.AddAsync(MapToEntity(model), cancellationToken);
    }

    public async Task<AircraftModel?> FindByIdAsync(AircraftModelId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.AircraftModels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<AircraftModel?> FindByNameAsync(AircraftModelName name, CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Value.Trim();

        var entity = await _context.AircraftModels
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(x => x.ModelName.ToLower() == normalizedName.ToLower(), cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<AircraftModel>> FindByManufacturerIdAsync(AircraftManufacturersId manufacturerId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.AircraftModels
            .AsNoTracking()
            .Where(x => x.AircraftManufacturerId == manufacturerId.Value)
            .OrderBy(x => x.ModelName)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<AircraftModel>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.AircraftModels
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain).ToList();
    }

    public Task<bool> ExistsByManufacturerAndNameAsync(
        AircraftManufacturersId manufacturerId,
        AircraftModelName name,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Value.Trim();

        return _context.AircraftModels.AnyAsync(
            x => x.AircraftManufacturerId == manufacturerId.Value &&
                 x.ModelName.ToLower() == normalizedName.ToLower(),
            cancellationToken);
    }

    public Task<bool> HasAircraftAsync(AircraftModelId id, CancellationToken cancellationToken = default)
        => _context.Aircrafts.AnyAsync(x => x.AircraftModelId == id.Value, cancellationToken);

    public async Task UpdateAsync(AircraftModel model, CancellationToken cancellationToken = default)
    {
        var entity = await _context.AircraftModels
            .FirstOrDefaultAsync(x => x.Id == model.Id.Value, cancellationToken);

        if (entity is null)
            throw new InvalidOperationException($"Aircraft model with id '{model.Id.Value}' not found.");

        entity.AircraftManufacturerId = model.ManufacturerId.Value;
        entity.ModelName = model.ModelName.Value;
        entity.MaxCapacity = model.MaxCapacity.Value;
        entity.MaxTakeoffWeightKg = model.MaxTakeoffWeight.Value;
        entity.FuelConsumptionKgH = model.FuelConsumption.Value;
        entity.CruiseSpeedKmh = model.CruiseSpeed.Value;
        entity.CruiseAltitudeFt = model.CruiseAltitude.Value;
    }

    public async Task<bool> DeleteByIdAsync(AircraftModelId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.AircraftModels
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        if (entity is null)
            return false;

        _context.AircraftModels.Remove(entity);
        return true;
    }

    private static AircraftModel MapToDomain(AircraftModelsEntity entity)
        => AircraftModel.FromPrimitives(
            entity.Id,
            entity.AircraftManufacturerId,
            entity.ModelName,
            entity.MaxCapacity,
            entity.MaxTakeoffWeightKg,
            entity.FuelConsumptionKgH,
            entity.CruiseSpeedKmh,
            entity.CruiseAltitudeFt);

    private static AircraftModelsEntity MapToEntity(AircraftModel model)
        => new()
        {
            AircraftManufacturerId = model.ManufacturerId.Value,
            ModelName = model.ModelName.Value,
            MaxCapacity = model.MaxCapacity.Value,
            MaxTakeoffWeightKg = model.MaxTakeoffWeight.Value,
            FuelConsumptionKgH = model.FuelConsumption.Value,
            CruiseSpeedKmh = model.CruiseSpeed.Value,
            CruiseAltitudeFt = model.CruiseAltitude.Value
        };
}
