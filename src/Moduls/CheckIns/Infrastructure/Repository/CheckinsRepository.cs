using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Infrastructure.Repository;

public sealed class CheckinsRepository : ICheckinsRepository
{
    private readonly AppDbContext _context;

    public CheckinsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Checkin?> GetByIdAsync(CheckinsId id)
    {
        var entity = await _context.Set<CheckinEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Checkin?> GetByBoardingPassAsync(CheckinsBoardingPassNumber boardingPassNumber)
    {
        var entity = await _context.Set<CheckinEntity>()
            .FirstOrDefaultAsync(x => x.BoardingPassNumber == boardingPassNumber.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Checkin?> GetByTicketIdAsync(TicketId ticketId)
    {
        var entity = await _context.Set<CheckinEntity>()
            .FirstOrDefaultAsync(x => x.TicketId == ticketId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<Checkin>> GetAllAsync()
    {
        var entities = await _context.Set<CheckinEntity>().ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(Checkin checkin)
    {
        var entity = MapToEntity(checkin);
        await _context.Set<CheckinEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Checkin checkin)
    {
        var entity = await _context.Set<CheckinEntity>()
            .FirstOrDefaultAsync(x => x.Id == checkin.Id.Value);

        if (entity is null) return;

        entity.CheckinStatusId = checkin.CheckinStatusId.Value;

        _context.Set<CheckinEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CheckinsId id)
    {
        var entity = await _context.Set<CheckinEntity>()
            .FirstOrDefaultAsync(x => x.Id == id.Value);

        if (entity is null) return;

        _context.Set<CheckinEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }


    private static Checkin MapToDomain(CheckinEntity entity)
    {
        return Checkin.Create(
            entity.Id,
            entity.TicketId,
            entity.StaffId,
            entity.FlightSeatId,
            entity.CheckedInAt,
            entity.CheckinStatusId,
            entity.BoardingPassNumber);
    }

    private static CheckinEntity MapToEntity(Checkin domain)
    {
        return new CheckinEntity
        {
            TicketId          = domain.TicketId.Value,
            StaffId           = domain.StaffId.Value,
            FlightSeatId      = domain.FlightSeatId.Value,
            CheckedInAt       = domain.CheckedInAt.Value,
            CheckinStatusId   = domain.CheckinStatusId.Value,
            BoardingPassNumber = domain.BoardingPassNumber.Value
        };
    }
}