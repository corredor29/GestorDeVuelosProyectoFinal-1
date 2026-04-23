using Microsoft.EntityFrameworkCore;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using StatusAggregate = GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Domain.Aggregate.BookingStatuses;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Infrastructure.Repository;

public sealed class BookingStatusesRepository : IBookingStatuseRepository
{
    private readonly AppDbContext _context;

    public BookingStatusesRepository(AppDbContext context) => _context = context;

    public async Task<StatusAggregate?> GetByIdAsync(BookingStatusesId id)
    {
        var entity = await _context.BookingStatuses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id.Value);
        return entity == null ? null : StatusAggregate.FromPrimitives(entity.Id, entity.Name);
    }

    public async Task<StatusAggregate?> GetByNameAsync(string name)
    {
        var normalized = name.Trim();
        var entity = await _context.BookingStatuses.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == normalized);
        return entity == null ? null : StatusAggregate.FromPrimitives(entity.Id, entity.Name);
    }

    public async Task<IEnumerable<StatusAggregate>> GetAllAsync()
    {
        var entities = await _context.BookingStatuses.AsNoTracking().ToListAsync();
        return entities.Select(e => StatusAggregate.FromPrimitives(e.Id, e.Name));
    }

    public async Task SaveAsync(StatusAggregate status)
    {
        var entity = new BookingStatusesEntity { Name = status.Name.Value };
        await _context.BookingStatuses.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(StatusAggregate status)
    {
        var entity = await _context.BookingStatuses.FirstOrDefaultAsync(x => x.Id == status.Id.Value);
        if (entity != null)
        {
            entity.Name = status.Name.Value;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(BookingStatusesId id)
    {
        var entity = await _context.BookingStatuses.FirstOrDefaultAsync(x => x.Id == id.Value);
        if (entity != null)
        {
            _context.BookingStatuses.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
