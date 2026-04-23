using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Infrastructure.Repository;

public sealed class UsersRepository : IUsersRepository
{
    private readonly AppDbContext _context;

    public UsersRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(UsersId id)
    {
        var entity = await _context.Set<UsersEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<User?> GetByUsernameAsync(UsersUsername username)
    {
        var entity = await _context.Set<UsersEntity>()
            .FirstOrDefaultAsync(x => x.Username == username.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<User?> GetByPersonIdAsync(PeopleId personId)
    {
        var entity = await _context.Set<UsersEntity>()
            .FirstOrDefaultAsync(x => x.Person_Id == personId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<User>> GetByRoleIdAsync(RolesId roleId)
    {
        var entities = await _context.Set<UsersEntity>()
            .Where(x => x.Role_Id == roleId.Value)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        var entities = await _context.Set<UsersEntity>()
            .Where(x => x.IsActive)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<User>> GetInactiveUsersAsync()
    {
        var entities = await _context.Set<UsersEntity>()
            .Where(x => !x.IsActive)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var entities = await _context.Set<UsersEntity>()
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(User user)
    {
        var entity = MapToEntity(user);
        await _context.Set<UsersEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        user.SetId(entity.Id);
    }

    public async Task UpdateAsync(User user)
    {
        var entity = await _context.Set<UsersEntity>()
            .FirstOrDefaultAsync(x => x.Id == user.Id.Value);

        if (entity is null) return;

        entity.PasswordHash = user.PasswordHash.Value;
        entity.Role_Id = user.RolId.Value;
        entity.IsActive = user.IsActive.Value;
        entity.LastAccess = user.LastAccess;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.Set<UsersEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(UsersId id)
    {
        var entity = await _context.Set<UsersEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null) return;

        _context.Set<UsersEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static User MapToDomain(UsersEntity entity)
    {
        return User.Create(
            entity.Id,
            entity.Username,
            entity.PasswordHash,
            entity.Role_Id,
            entity.Person_Id,
            entity.IsActive
        );
    }

    private static UsersEntity MapToEntity(User domain)
    {
        return new UsersEntity
        {
            Username = domain.Username.Value,
            PasswordHash = domain.PasswordHash.Value,
            Person_Id = domain.PersonId?.Value,
            Role_Id = domain.RolId.Value,
            IsActive = domain.IsActive.Value,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}