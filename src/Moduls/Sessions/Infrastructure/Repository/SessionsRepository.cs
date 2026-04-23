using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Infrastructure.Repository;

public sealed class SessionsRepository : ISessionsRepository
{
    private readonly AppDbContext _context;

    public SessionsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Session?> GetByIdAsync(SessionsId id)
    {
        var entity = await _context.Set<SessionsEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Session>> GetActiveSessionsByUserIdAsync(UsersId userId)
    {
        var entities = await _context.Set<SessionsEntity>()
            .Where(x => x.User_Id == userId.Value && x.IsActive)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(Session session)
    {
        var entity = MapToEntity(session);
        await _context.Set<SessionsEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        session.SetId(entity.Id);
    }

    public async Task UpdateAsync(Session session)
    {
        var entity = await _context.Set<SessionsEntity>()
            .FirstOrDefaultAsync(x => x.Id == session.Id.Value);

        if (entity is null) return;

        entity.EndedAt = session.CerradaEn;
        entity.IsActive = session.Activa.IsActive;

        _context.Set<SessionsEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }


    private static Session MapToDomain(SessionsEntity entity)
    {
        var session = Session.Create(
            entity.Id,
            entity.User_Id,
            entity.IpAddress,
            entity.StartedAt
        );

        if (!entity.IsActive)
            session.CloseSession();

        return session;
    }

    private static SessionsEntity MapToEntity(Session domain)
    {
        return new SessionsEntity
        {
            User_Id = domain.UsuarioId.Value,
            StartedAt = domain.IniciadaEn,
            EndedAt = domain.CerradaEn,
            IpAddress = domain.IpOrigen.Value,
            IsActive = domain.Activa.IsActive
        };
    }
}