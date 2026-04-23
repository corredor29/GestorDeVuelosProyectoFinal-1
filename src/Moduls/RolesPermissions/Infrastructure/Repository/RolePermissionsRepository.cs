using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Infrastructure.Repository;

public sealed class RolePermissionsRepository : IRolePermissionsRepository
{
    private readonly AppDbContext _context;

    public RolePermissionsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RolePermission?> GetByIdAsync(RolePermissionsId id)
    {
        var entity = await _context.Set<RolePermissionsEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<RolePermission>> GetByRoleIdAsync(RolesId roleId)
    {
        var entities = await _context.Set<RolePermissionsEntity>()
            .Where(x => x.Role_Id == roleId.Value)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(RolePermission rolePermission)
    {
        var entity = MapToEntity(rolePermission);
        await _context.Set<RolePermissionsEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        rolePermission.SetId(entity.Id);
    }

    public async Task DeleteAsync(RolePermissionsId id)
    {
        var entity = await _context.Set<RolePermissionsEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null) return;

        _context.Set<RolePermissionsEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }


    private static RolePermission MapToDomain(RolePermissionsEntity entity)
    {
        return RolePermission.Create(entity.Id, entity.Role_Id, entity.Permission_Id);
    }

    private static RolePermissionsEntity MapToEntity(RolePermission domain)
    {
        return new RolePermissionsEntity
        {
            Role_Id = domain.RoleId.Value,
            Permission_Id = domain.PermissionId.Value
        };
    }
}