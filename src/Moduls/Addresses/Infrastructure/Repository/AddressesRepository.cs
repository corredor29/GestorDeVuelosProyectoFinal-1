using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Infrastructure.Persistence.Entities;

namespace GestorDeVuelosProyectoFinal.Moduls.Addresses.Infrastructure.Repository;

// Repositorio de direcciones.
// Aquí también se manejan las columnas duplicadas heredadas de migraciones viejas como CityId1 y StreetTypeId1.
public sealed class AddressRepository : IAddressRepository
{
    private readonly AppDbContext _context;

    public AddressRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Address?> GetByIdAsync(AddressesId id, CancellationToken ct = default)
    {
        // Leemos sin tracking porque este método se usa mucho en consultas simples y listados.
        var entity = await _context.Addresses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, ct);

        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<Address?> GetByStreetAndNumberAsync(string streetName, string? number, CancellationToken ct = default)
    {
        var streetLower = streetName.Trim().ToLower();
        
        var entity = await _context.Addresses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => 
                x.StreetName.ToLower() == streetLower && 
                x.Number == number, ct);

        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Address>> GetAllAsync(CancellationToken ct = default)
    {
        var entities = await _context.Addresses
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync(ct);
        return entities.Select(MapToDomain).ToList();
    }

    public async Task SaveAsync(Address address, CancellationToken ct = default)
    {
        // Al guardar llenamos también las columnas duplicadas para mantener compatibilidad
        // con la estructura que dejaron las migraciones anteriores.
        var entity = new AddressEntity
        {
            StreetTypeId = address.RoadTypeId.Value,
            StreetTypeId1 = address.RoadTypeId.Value,
            StreetName = address.StreetName.Value,
            Number = address.Number.Value,
            Complement = address.Complement.Value,
            CityId = address.CityId.Value,
            CityId1 = address.CityId.Value,
            PostalCode = address.PostalCode.Value
        };

        await _context.Addresses.AddAsync(entity, ct);
    }

    public async Task UpdateAsync(Address address, CancellationToken ct = default)
    {
        // Igual que en create, actualizamos también las columnas espejo para que no fallen las FK.
        var entity = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == address.Id.Value, ct);
        if (entity is null)
            throw new KeyNotFoundException($"La dirección con ID {address.Id.Value} no existe.");

        entity.StreetTypeId = address.RoadTypeId.Value;
        entity.StreetTypeId1 = address.RoadTypeId.Value;
        entity.StreetName = address.StreetName.Value;
        entity.Number = address.Number.Value;
        entity.Complement = address.Complement.Value;
        entity.CityId = address.CityId.Value;
        entity.CityId1 = address.CityId.Value;
        entity.PostalCode = address.PostalCode.Value;
    }

    public async Task<bool> DeleteAsync(AddressesId id, CancellationToken ct = default)
    {
        // FindAsync no acepta CancellationToken de la misma forma que FirstOrDefault
        var entity = await _context.Addresses.FindAsync(new object[] { id.Value }, ct);
        if (entity == null) return false;

        _context.Addresses.Remove(entity);
        return true;
    }

    public async Task<bool> DeleteByStreetAndNumberAsync(string streetName, string? number, CancellationToken ct = default)
    {
        var streetLower = streetName.Trim().ToLower();
        
        var entity = await _context.Addresses
            .FirstOrDefaultAsync(x => 
                x.StreetName.ToLower() == streetLower && 
                x.Number == number, ct);

        if (entity == null) return false;

        _context.Addresses.Remove(entity);
        return true;
    }

    public async Task<IReadOnlyCollection<Address>> GetByCityAsync(CityId cityId, CancellationToken ct = default)
    {
        var entities = await _context.Addresses
            .AsNoTracking()
            .Where(x => x.CityId == cityId.Value)
            .ToListAsync(ct);
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Address?> GetByNameAsync(AddressesNameVia name, CancellationToken ct = default)
    {
        var nameLower = name.Value.ToLower();
        var entity = await _context.Addresses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.StreetName.ToLower() == nameLower, ct);
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Address>> GetByPostalCodeAsync(AddressesPostalCode postalCode, CancellationToken ct = default)
    {
        var entities = await _context.Addresses
            .AsNoTracking()
            .Where(x => x.PostalCode == postalCode.Value)
            .ToListAsync(ct);
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Address>> GetByStreetTypeAsync(int streetTypeId, CancellationToken ct = default)
    {
        var entities = await _context.Addresses
            .AsNoTracking()
            .Where(x => x.StreetTypeId == streetTypeId)
            .ToListAsync(ct);
            
        return entities.Select(MapToDomain).ToList();
    }

    // Estos mappers dejan concentrada la traducción entre entidad de EF y aggregate.

    private static Address MapToDomain(AddressEntity entity)
    {
        return Address.Create(
            entity.Id,
            entity.StreetTypeId,
            entity.StreetName,
            entity.Number,
            entity.Complement,
            entity.PostalCode,
            entity.CityId
        );
    }
}
