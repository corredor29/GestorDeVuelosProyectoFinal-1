using GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.UseCases;

// reservas por estado;
public sealed class GetBookingsByStatusUseCase
{
    private readonly AppDbContext _context;
    public GetBookingsByStatusUseCase(AppDbContext context) => _context = context;
    public async Task<IEnumerable<BookingsByStatusDto>> ExecuteAsync(CancellationToken ct = default)
    {
        var bookings = await _context.Bookings.AsNoTracking().ToListAsync(ct);
        var statuses = await _context.BookingStatuses.AsNoTracking().ToListAsync(ct);

        var result =
            from b in bookings
            join s in statuses on b.BookingStatusId equals s.Id
            group b by s.Name into g
            select new BookingsByStatusDto(
                g.Key,
                g.Count(),
                g.Sum(b => b.TotalAmount));

        return result
            .OrderByDescending(x => x.Count)
            .ToList();
    }
}