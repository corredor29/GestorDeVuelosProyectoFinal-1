using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Infrastructure.Repository;

public sealed class FlightRolesRepository : IFlightRolesRepository
{
    private readonly AppDbContext _context;

    public FlightRolesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FlightRole?> GetByIdAsync(FlightRolesId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.FlightRoles.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<FlightRole?> GetByNameAsync(FlightRolesName name, CancellationToken cancellationToken = default)
    {
        var normalized = name.Value;
        var entity = await _context.FlightRoles.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name.ToLower() == normalized.ToLower(), cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public Task<FlightRole?> GetByNameStringAsync(string name, CancellationToken cancellationToken = default)
        => GetByNameAsync(FlightRolesName.Create(name), cancellationToken);

    public async Task<IEnumerable<FlightRole>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.FlightRoles.AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(FlightRole flightRole, CancellationToken cancellationToken = default)
    {
        await _context.FlightRoles.AddAsync(new FlightRolesEntity
        {
            Name = flightRole.Name.Value
        }, cancellationToken);
    }

    public async Task UpdateAsync(FlightRole flightRole, CancellationToken cancellationToken = default)
    {
        if (flightRole.Id is null)
            throw new InvalidOperationException("No se puede actualizar un rol de tripulacion sin id.");

        var entity = await _context.FlightRoles
            .FirstOrDefaultAsync(x => x.Id == flightRole.Id.Value, cancellationToken);

        if (entity is null)
            throw new InvalidOperationException($"No se encontro el rol de tripulacion con id {flightRole.Id.Value}.");

        entity.Name = flightRole.Name.Value;
    }

    public async Task DeleteAsync(FlightRolesId id, CancellationToken cancellationToken = default)
    {
        await _context.FlightRoles
            .Where(x => x.Id == id.Value)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<bool> DeleteByNameAsync(FlightRolesName name, CancellationToken cancellationToken = default)
    {
        var deleted = await _context.FlightRoles
            .Where(x => x.Name.ToLower() == name.Value.ToLower())
            .ExecuteDeleteAsync(cancellationToken);

        return deleted > 0;
    }

    private static FlightRole MapToDomain(FlightRolesEntity entity)
        => FlightRole.FromPrimitives(entity.Id, entity.Name);
}
