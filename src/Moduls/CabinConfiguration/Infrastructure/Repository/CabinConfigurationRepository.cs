using System;
using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.CabinConfiguration.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using CabinConfigurationAggregate = GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.Aggregate.CabinConfiguration;

namespace GestorDeVuelosProyectoFinal.src.Moduls.CabinConfiguration.Infrastructure.Repository;

public sealed class CabinConfigurationRepository : ICabinConfigurationRepository
{
    private readonly AppDbContext _context;

    public CabinConfigurationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CabinConfigurationAggregate?> GetByIdAsync(CabinConfigurationId id)
    {
        var entity = await _context.CabinConfigurations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<CabinConfigurationAggregate?> GetByAircraftAndCabinTypeAsync(int aircraftId, int cabinTypeId)
    {
        var entity = await _context.CabinConfigurations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AircraftId == aircraftId && x.CabinTypeId == cabinTypeId);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<CabinConfigurationAggregate>> GetByAircraftIdAsync(int aircraftId)
    {
        var entities = await _context.CabinConfigurations
            .AsNoTracking()
            .Where(x => x.AircraftId == aircraftId)
            .OrderBy(x => x.CabinTypeId)
            .ThenBy(x => x.RowStart)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<CabinConfigurationAggregate>> GetAllAsync()
    {
        var entities = await _context.CabinConfigurations
            .AsNoTracking()
            .OrderBy(x => x.AircraftId)
            .ThenBy(x => x.CabinTypeId)
            .ThenBy(x => x.RowStart)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(CabinConfigurationAggregate configuration)
    {
        var entity = MapToEntity(configuration);
        await _context.CabinConfigurations.AddAsync(entity);
    }

    public async Task UpdateAsync(CabinConfigurationAggregate configuration)
    {
        var entity = await _context.CabinConfigurations
            .FirstOrDefaultAsync(x => x.Id == configuration.Id.Value);

        if (entity is null)
            throw new InvalidOperationException($"Cabin configuration with id '{configuration.Id.Value}' not found.");

        entity.AircraftId = configuration.AircraftId;
        entity.CabinTypeId = configuration.CabinTypeId.Value;
        entity.RowStart = configuration.RowRange.StartRow;
        entity.RowEnd = configuration.RowRange.EndRow;
        entity.SeatsPerRow = configuration.SeatsPerRow.Value;
        entity.SeatLetters = configuration.SeatLetters.Value;
    }

    public async Task DeleteAsync(CabinConfigurationId id)
    {
        var entity = await _context.CabinConfigurations
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null)
            throw new InvalidOperationException($"Cabin configuration with id '{id.Value}' not found.");

        _context.CabinConfigurations.Remove(entity);
    }

    private static CabinConfigurationAggregate MapToDomain(CabinConfiurationEntity entity)
    {
        return CabinConfigurationAggregate.FromPrimitives(
            entity.Id,
            entity.AircraftId,
            entity.CabinTypeId,
            entity.RowStart,
            entity.RowEnd,
            entity.SeatsPerRow,
            entity.SeatLetters);
    }

    private static CabinConfiurationEntity MapToEntity(CabinConfigurationAggregate configuration)
    {
        return new CabinConfiurationEntity
        {
            Id = configuration.Id.Value,
            AircraftId = configuration.AircraftId,
            CabinTypeId = configuration.CabinTypeId.Value,
            RowStart = configuration.RowRange.StartRow,
            RowEnd = configuration.RowRange.EndRow,
            SeatsPerRow = configuration.SeatsPerRow.Value,
            SeatLetters = configuration.SeatLetters.Value
        };
    }
}

