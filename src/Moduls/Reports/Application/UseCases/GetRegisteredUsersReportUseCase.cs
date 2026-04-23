using GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.Support;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.UseCases;

/// <summary>
/// Todos los usuarios del sistema con persona asociada, más reservas/total (incluye 0 reservas).
/// Parte de <see cref="Shared.Context.AppDbContext.Users"/>, no solo de bookings.
/// </summary>
public sealed class GetRegisteredUsersReportUseCase
{
    private readonly AppDbContext _context;

    public GetRegisteredUsersReportUseCase(AppDbContext context) => _context = context;

    public async Task<IEnumerable<TopClientDto>> ExecuteAsync(CancellationToken ct = default)
    {
        var cancelledId = await _context.BookingStatuses.AsNoTracking()
            .Where(s => s.Name == "Cancelled")
            .Select(s => (int?)s.Id)
            .FirstOrDefaultAsync(ct);

        var usersData = await (
            from u in _context.Users.AsNoTracking()
            where u.Person_Id != null
            join p in _context.Persons.AsNoTracking() on u.Person_Id equals p.Id
            join dt in _context.DocumentTypes.AsNoTracking() on p.DocumentTypeId equals dt.Id into dtg
            from dt in dtg.DefaultIfEmpty()
            select new
            {
                u.Username,
                p.FirstName,
                p.LastName,
                p.DocumentNumber,
                DocTypeCode = dt != null ? dt.Code : "",
                PersonId = p.Id
            }
        ).ToListAsync(ct);

        var personToCustomerId = await _context.Customers.AsNoTracking()
            .ToDictionaryAsync(c => c.PersonId, c => c.Id, ct);

        var bookingRows = await _context.Bookings.AsNoTracking()
            .Where(b => !cancelledId.HasValue || b.BookingStatusId != cancelledId.Value)
            .Select(b => new { b.ClientId, b.TotalAmount })
            .ToListAsync(ct);

        var aggregates = bookingRows
            .GroupBy(x => x.ClientId)
            .ToDictionary(
                g => g.Key,
                g => (Count: g.Count(), Sum: g.Sum(x => x.TotalAmount)));

        var rows = usersData.Select(u =>
        {
            personToCustomerId.TryGetValue(u.PersonId, out var customerId);
            var count = 0;
            decimal sum = 0;
            if (customerId != 0 && aggregates.TryGetValue(customerId, out var agg))
            {
                count = agg.Count;
                sum = agg.Sum;
            }

            return new TopClientDto(
                customerId,
                ReportClientFormatting.BuildClientDisplayName(u.FirstName, u.LastName, u.Username),
                u.Username,
                ReportClientFormatting.BuildIdentityDocument(u.DocTypeCode, u.DocumentNumber),
                count,
                sum);
        });

        return rows
            .OrderByDescending(x => x.TotalBookings)
            .ThenByDescending(x => x.TotalSpent)
            .ThenBy(x => x.AccountUsername, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}
