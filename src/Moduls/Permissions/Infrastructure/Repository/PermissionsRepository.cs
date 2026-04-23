using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Infrastructure.Repository;

public sealed class PermissionsRepository : IPermissionsRepository
{
    private readonly AppDbContext _context;

    public PermissionsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Permission?> GetByIdAsync(PermissionsId id)
    {
        var entity = await _context.Set<PermissionsEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Permission?> GetByNameAsync(PermissionsName name)
    {
        var entity = await _context.Set<PermissionsEntity>()
            .FirstOrDefaultAsync(x => x.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Permission>> GetAllAsync()
    {
        var entities = await _context.Set<PermissionsEntity>()
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(Permission permission)
    {
        var entity = MapToEntity(permission);
        await _context.Set<PermissionsEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        permission.SetId(entity.Id);
    }

    public async Task UpdateAsync(Permission permission)
    {
        var entity = await _context.Set<PermissionsEntity>()
            .FirstOrDefaultAsync(x => x.Id == permission.Id.Value);

        if (entity is null) return;

        entity.Name = permission.Name.Value;
        entity.Description = permission.Description.Value;

        _context.Set<PermissionsEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PermissionsId id)
    {
        var entity = await _context.Set<PermissionsEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null) return;

        _context.Set<PermissionsEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }


    private static Permission MapToDomain(PermissionsEntity entity)
    {
        return Permission.Create(entity.Id, entity.Name, entity.Description);
    }

    private static PermissionsEntity MapToEntity(Permission domain)
    {
        return new PermissionsEntity
        {
            Name = domain.Name.Value,
            Description = domain.Description.Value
        };
    }
}