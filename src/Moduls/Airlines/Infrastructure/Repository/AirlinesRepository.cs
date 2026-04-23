using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.Infrastructure.Repository;

public sealed class AirlinesRepository : IAirlinesRepository
{
    private readonly AppDbContext _context;

    public AirlinesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Airline?> GetByIdAsync(AirlinesId id)
    {
        var entity = await _context.Airlines
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<Airline?> GetByNameAsync(AirlinesName name)
    {
        var normalized = name.Value.Trim().ToLower();

        var entity = await _context.Airlines
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == normalized);

        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<Airline?> GetByIataCodeAsync(AirlinesIataCode code)
    {
        var entity = await _context.Airlines
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IataCode == code.Value);

        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Airline>> GetByOriginCountryIdAsync(CountryId countryId)
    {
        var entities = await _context.Airlines
            .AsNoTracking()
            .Where(x => x.OriginCountryId == countryId.Value)
            .OrderBy(x => x.Name)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Airline>> GetAllAsync()
    {
        var entities = await _context.Airlines
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Airline>> GetActiveAsync()
    {
        // Como el borrado es lógico, este listado ayuda a separar lo operativo de lo histórico.
        var entities = await _context.Airlines
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(Airline airline)
    {
        await _context.Airlines.AddAsync(MapToEntity(airline));
    }

    public Task UpdateAsync(Airline airline)
    {
        // Aquí se rehidrata la entidad completa desde el agregado y EF se encarga del update.
        _context.Airlines.Update(MapToEntity(airline));
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(AirlinesId id)
    {
        var entity = await _context.Airlines.FirstOrDefaultAsync(x => x.Id == id.Value);
        if (entity is null)
            return;

        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;
    }

    public async Task DeleteByNameAsync(AirlinesName name)
    {
        var normalized = name.Value.Trim().ToLower();
        var entity = await _context.Airlines.FirstOrDefaultAsync(x => x.Name.ToLower() == normalized);
        if (entity is null)
            return;

        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;
    }

    public async Task DeleteByIataCodeAsync(AirlinesIataCode code)
    {
        var entity = await _context.Airlines.FirstOrDefaultAsync(x => x.IataCode == code.Value);
        if (entity is null)
            return;

        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;
    }

    public async Task<int> DeleteByOriginCountryIdAsync(CountryId countryId)
    {
        // Se desactivan varias filas de una vez porque puede haber varias aerolíneas del mismo país.
        var entities = await _context.Airlines
            .Where(x => x.OriginCountryId == countryId.Value && x.IsActive)
            .ToListAsync();

        foreach (var entity in entities)
        {
            entity.IsActive = false;
            entity.UpdatedAt = DateTime.UtcNow;
        }

        return entities.Count;
    }

    public async Task ReactivateAsync(AirlinesId id)
    {
        var entity = await _context.Airlines.FirstOrDefaultAsync(x => x.Id == id.Value);
        if (entity is null)
            return;

        entity.IsActive = true;
        entity.UpdatedAt = DateTime.UtcNow;
    }

    private static Airline MapToDomain(AirlineEntity entity)
    {
        return Airline.FromPrimitives(
            entity.Id,
            entity.Name,
            entity.IataCode,
            entity.OriginCountryId,
            entity.IsActive,
            entity.CreatedAt,
            entity.UpdatedAt);
    }

    private static AirlineEntity MapToEntity(Airline aggregate)
    {
        return new AirlineEntity
        {
            Id = aggregate.Id?.Value ?? 0,
            Name = aggregate.Name.Value,
            IataCode = aggregate.IataCode.Value,
            OriginCountryId = aggregate.OriginCountryId.Value,
            IsActive = aggregate.IsActive.Value,
            CreatedAt = aggregate.CreatedIn.Value,
            UpdatedAt = aggregate.UpdatedIn.Value
        };
    }
}
