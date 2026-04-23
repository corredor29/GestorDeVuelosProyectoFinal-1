using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Infrastructure.Repository;

// Repositorio de códigos telefónicos por país.
// Aquí se concentran búsquedas por ID, prefijo y nombre del país.
public sealed class PhoneCodesRepository : IPhoneCodesRepository
{
    private readonly AppDbContext _context;

    public PhoneCodesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PhoneCode?> GetByIdAsync(PhoneCodesId id)
    {
        var entity = await _context.PhoneCodes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id.Value);
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<PhoneCode?> GetByCountryCodeAsync(PhoneCodesCountryCode code)
    {
        var entity = await _context.PhoneCodes.AsNoTracking().FirstOrDefaultAsync(x => x.CountryCode == code.Value);
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<PhoneCode?> GetByCountryNameAsync(PhoneCodesCountryName countryName)
    {
        var normalized = countryName.Value.Trim().ToLower();
        var entity = await _context.PhoneCodes.AsNoTracking().FirstOrDefaultAsync(x => x.CountryName.ToLower() == normalized);
        return entity == null ? null : MapToDomain(entity);
    }

    public Task<PhoneCode?> GetByCountryCodeStringAsync(string code) => GetByCountryCodeAsync(PhoneCodesCountryCode.Create(code));

    public Task<PhoneCode?> GetByCountryNameStringAsync(string countryName) => GetByCountryNameAsync(PhoneCodesCountryName.Create(countryName));

    public async Task<IEnumerable<PhoneCode>> GetAllAsync()
    {
        var entities = await _context.PhoneCodes.AsNoTracking().OrderBy(x => x.CountryName).ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(PhoneCode phoneCode)
    {
        await _context.PhoneCodes.AddAsync(MapToEntity(phoneCode));
    }

    public Task UpdateAsync(PhoneCode phoneCode)
    {
        // El aggregate ya trae el estado completo, así que EF reemplaza el registro mapeado.
        _context.PhoneCodes.Update(MapToEntity(phoneCode));
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(PhoneCodesId id)
    {
        var entity = await _context.PhoneCodes.FindAsync(id.Value);
        if (entity is not null)
            _context.PhoneCodes.Remove(entity);
    }

    public async Task DeleteByCountryCodeAsync(PhoneCodesCountryCode code)
    {
        var entity = await _context.PhoneCodes.FirstOrDefaultAsync(x => x.CountryCode == code.Value);
        if (entity is not null)
            _context.PhoneCodes.Remove(entity);
    }

    public async Task DeleteByCountryNameAsync(PhoneCodesCountryName countryName)
    {
        var normalized = countryName.Value.Trim().ToLower();
        var entity = await _context.PhoneCodes.FirstOrDefaultAsync(x => x.CountryName.ToLower() == normalized);
        if (entity is not null)
            _context.PhoneCodes.Remove(entity);
    }

    private static PhoneCode MapToDomain(PhoneCodeEntity entity) =>
        PhoneCode.FromPrimitives(entity.Id, entity.CountryCode, entity.CountryName);

    private static PhoneCodeEntity MapToEntity(PhoneCode aggregate) =>
        new()
        {
            Id = aggregate.Id?.Value ?? 0,
            CountryCode = aggregate.Code.Value,
            CountryName = aggregate.CountryName.Value
        };
}
