using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Countries.Infrastructure.Persistence.Repositories;

public sealed class CountryRepository : ICountriesRepository
{
    private readonly AppDbContext _context;

    public CountryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Country?> GetByIdAsync(CountryId id)
    {
        var entity = await _context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id.Value);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Country?> GetByNameAsync(string name)
    {
        var entity = await _context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == name.Trim());

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Country?> GetByIsoCodeAsync(CountryIsoCode isoCode)
    {
        var entity = await _context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IsoCode == isoCode.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Country>> GetByContinentAsync(ContinentsId continentId)
    {
        // Este filtro es muy usado desde el admin cuando se navega continente -> países.
        var entities = await _context.Countries
            .AsNoTracking()
            .Where(c => c.ContinentId == continentId.Value)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Country>> GetAllAsync()
    {
        var entities = await _context.Countries
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(Country country)
    {
        var entity = MapToEntity(country);
        await _context.Countries.AddAsync(entity);

    }

    public async Task UpdateAsync(Country country)
    {
        // Se cambia la fila existente para mantener simple el trabajo de EF.
        var entity = await _context.Countries
            .FirstOrDefaultAsync(c => c.Id == country.Id.Value);

        if (entity is null)
            throw new InvalidOperationException($"Country with id '{country.Id.Value}' not found.");

        entity.Name = country.Name.Value;
        entity.IsoCode = country.IsoCode.Value;
        entity.ContinentId = country.ContinentId.Value;
    }

    public async Task DeleteAsync(CountryId id)
    {
        var entity = await _context.Countries
            .FirstOrDefaultAsync(c => c.Id == id.Value);
        if (entity is null)
            throw new InvalidOperationException($"Country with id '{id.Value}' not found.");

        _context.Countries.Remove(entity);

    }

    public async Task DeleteByNameAsync(string name)
    {
        var entity = await _context.Countries
            .FirstOrDefaultAsync(c => c.Name == name.Trim());

        if (entity is null)
            throw new InvalidOperationException($"Country '{name}' not found.");

        _context.Countries.Remove(entity);

    }

    public async Task DeleteByIsoCodeAsync(string isoCode)
    {
        var entity = await _context.Countries
            .FirstOrDefaultAsync(c => c.IsoCode == isoCode.Trim().ToUpper());

        if (entity is null)
            throw new InvalidOperationException($"Country with ISO code '{isoCode}' not found.");

        _context.Countries.Remove(entity);
        
    }


    // Los mappers dejan claro qué viene de EF y qué entra al dominio.

    private static Country MapToDomain(CountryEntity entity)
    {
        return Country.Create(entity.Id, entity.Name, entity.IsoCode, entity.ContinentId);
    }

    private static CountryEntity MapToEntity(Country country)
    {
        return new CountryEntity
        {
            Name = country.Name.Value,
            IsoCode = country.IsoCode.Value,
            ContinentId = country.ContinentId.Value
        };
    }
}
