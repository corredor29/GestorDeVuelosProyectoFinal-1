using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using AircraftManufacturerAggregate = GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Domain.Aggregate.AircraftManufacturers;

namespace GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Infrastructure.Repository;

public sealed class AircraftManufacturersRepository : IAircraftManufacturersRepository
{
    private readonly AppDbContext _context;

    public AircraftManufacturersRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AircraftManufacturerAggregate?> GetByIdAsync(
        AircraftManufacturersId id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.AircraftManufacturers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<AircraftManufacturerAggregate?> GetByNameAsync(
        AircraftManufacturersName name,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Value.Trim();

        var entity = await _context.AircraftManufacturers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == normalizedName.ToLower(), cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<AircraftManufacturerAggregate>> GetByCountryAsync(
        AircraftManufacturersCountry country,
        CancellationToken cancellationToken = default)
    {
        var normalizedCountry = country.Value.Trim();

        var entities = await _context.AircraftManufacturers
            .AsNoTracking()
            .Where(x => x.Country.ToLower() == normalizedCountry.ToLower())
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<AircraftManufacturerAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.AircraftManufacturers
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ExistsByNameAsync(
        AircraftManufacturersName name,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Value.Trim();

        return await _context.AircraftManufacturers
            .AnyAsync(x => x.Name.ToLower() == normalizedName.ToLower(), cancellationToken);
    }

    public async Task AddAsync(AircraftManufacturerAggregate aircraftManufacturer, CancellationToken cancellationToken = default)
    {
        var entity = new AircraftManufacturerEntity
        {
            Name = aircraftManufacturer.Name.Value,
            Country = aircraftManufacturer.Country.Value
        };

        await _context.AircraftManufacturers.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(AircraftManufacturerAggregate aircraftManufacturer, CancellationToken cancellationToken = default)
    {
        var entity = await _context.AircraftManufacturers
            .FirstOrDefaultAsync(x => x.Id == aircraftManufacturer.Id.Value, cancellationToken);

        if (entity is null)
            throw new InvalidOperationException($"No se encontró el fabricante con id '{aircraftManufacturer.Id.Value}'.");

        entity.Name = aircraftManufacturer.Name.Value;
        entity.Country = aircraftManufacturer.Country.Value;
    }

    public async Task<bool> DeleteByIdAsync(AircraftManufacturersId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.AircraftManufacturers
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        if (entity is null)
            return false;

        _context.AircraftManufacturers.Remove(entity);
        return true;
    }

    public async Task<int> DeleteByCountryAsync(
        AircraftManufacturersCountry country,
        CancellationToken cancellationToken = default)
    {
        var normalizedCountry = country.Value.Trim();

        var entities = await _context.AircraftManufacturers
            .Where(x => x.Country.ToLower() == normalizedCountry.ToLower())
            .ToListAsync(cancellationToken);

        if (entities.Count == 0)
            return 0;

        _context.AircraftManufacturers.RemoveRange(entities);
        return entities.Count;
    }

    public Task<bool> HasAircraftModelsAsync(
        AircraftManufacturersId id,
        CancellationToken cancellationToken = default)
    {
        return _context.AircraftModels
            .AnyAsync(x => x.AircraftManufacturerId == id.Value, cancellationToken);
    }

    private static AircraftManufacturerAggregate MapToDomain(AircraftManufacturerEntity entity)
        => AircraftManufacturerAggregate.FromPrimitives(entity.Id, entity.Name, entity.Country);
}
