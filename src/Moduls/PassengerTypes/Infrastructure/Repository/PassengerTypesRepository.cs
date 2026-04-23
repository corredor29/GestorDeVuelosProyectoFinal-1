using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Infrastructure.Repository;

public sealed class PassengerTypesRepository : IPassengerTypesRepository
{
    private readonly AppDbContext _context;

    public PassengerTypesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PassengerType?> GetByIdAsync(PassengerTypesId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.PassengerTypes.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<PassengerType?> GetByNameAsync(PassengerTypesName name, CancellationToken cancellationToken = default)
    {
        var normalized = name.Value;
        var entity = await _context.PassengerTypes.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == normalized.ToLower(), cancellationToken);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<PassengerType?> GetByAgeAsync(int ageInYears, CancellationToken cancellationToken = default)
    {
        if (ageInYears < 0)
            return null;

        var entity = await _context.PassengerTypes.AsNoTracking()
            .Where(x =>
                (x.MinAge == null || x.MinAge <= ageInYears) &&
                (x.MaxAge == null || x.MaxAge >= ageInYears))
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<PassengerType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.PassengerTypes.AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
        return entities.Select(MapToDomain).ToList();
    }

    public async Task SaveAsync(PassengerType passengerType, CancellationToken cancellationToken = default)
    {
        await _context.PassengerTypes.AddAsync(new PassengerTypeEntity
        {
            Name = passengerType.Name.Value,
            MinAge = passengerType.MinAge.Value,
            MaxAge = passengerType.MaxAge.Value
        }, cancellationToken);
    }

    public async Task UpdateAsync(PassengerType passengerType, CancellationToken cancellationToken = default)
    {
        if (passengerType.Id is null)
            throw new InvalidOperationException("No se puede actualizar un tipo sin id.");

        var entity = await _context.PassengerTypes
            .FirstOrDefaultAsync(x => x.Id == passengerType.Id.Value, cancellationToken);

        if (entity is null)
            throw new InvalidOperationException($"No se encontro el tipo de pasajero con id {passengerType.Id.Value}.");

        entity.Name = passengerType.Name.Value;
        entity.MinAge = passengerType.MinAge.Value;
        entity.MaxAge = passengerType.MaxAge.Value;
    }

    public async Task DeleteAsync(PassengerTypesId id, CancellationToken cancellationToken = default)
    {
        await _context.PassengerTypes
            .Where(x => x.Id == id.Value)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<bool> DeleteByNameAsync(PassengerTypesName name, CancellationToken cancellationToken = default)
    {
        var n = await _context.PassengerTypes
            .Where(x => x.Name.ToLower() == name.Value.ToLower())
            .ExecuteDeleteAsync(cancellationToken);
        return n > 0;
    }

    public Task<int> DeleteByAgeAsync(int ageInYears, CancellationToken cancellationToken = default)
    {
        if (ageInYears < 0)
            return Task.FromResult(0);

        return _context.PassengerTypes
            .Where(x =>
                (x.MinAge == null || x.MinAge <= ageInYears) &&
                (x.MaxAge == null || x.MaxAge >= ageInYears))
            .ExecuteDeleteAsync(cancellationToken);
    }

    private static PassengerType MapToDomain(PassengerTypeEntity e)
        => PassengerType.FromPrimitives(e.Id, e.Name, e.MinAge, e.MaxAge);
}
