using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Roles.Infrastructure.Repository;

public sealed class SystemRolesRepository : ISystemRolesRepository
{
    private readonly AppDbContext _context;

    public SystemRolesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SystemRole?> GetByIdAsync(RolesId id)
    {
        var entity = await _context.Set<SystemRolesEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<SystemRole?> GetByNameAsync(RolesName name)
    {
        var entity = await _context.Set<SystemRolesEntity>()
            .FirstOrDefaultAsync(x => x.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<SystemRole>> GetAllAsync()
    {
        var entities = await _context.Set<SystemRolesEntity>()
            .ToListAsync();

        return entities.Select(MapToDomain);
    }
    public async Task UpdateAsync(SystemRole role)
{
    var entity = await _context.Set<SystemRolesEntity>()
        .FirstOrDefaultAsync(x => x.Id == role.Id.Value);

    if (entity is null) return;

    entity.Name = role.Name.Value;
    entity.Description = role.Description.Value;

    _context.Set<SystemRolesEntity>().Update(entity);
    await _context.SaveChangesAsync();
}

    public async Task SaveAsync(SystemRole role)
    {
        var entity = MapToEntity(role);
        await _context.Set<SystemRolesEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        role.SetId(entity.Id);
    }

    public async Task DeleteAsync(RolesId id)
    {
        var entity = await _context.Set<SystemRolesEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null) return;

        _context.Set<SystemRolesEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }


    private static SystemRole MapToDomain(SystemRolesEntity entity)
    {
        return SystemRole.Create(entity.Id, entity.Name, entity.Description);
    }

    private static SystemRolesEntity MapToEntity(SystemRole domain)
    {
        return new SystemRolesEntity
        {
            Name = domain.Name.Value,
            Description = domain.Description.Value
        };
    }
}