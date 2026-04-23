using GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.Support;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.UseCases;

/// <summary>
/// Clientes con más reservas (no canceladas). Nombre mostrado sin "Cliente {login}" duplicado;
/// documento con código de tipo + número.
/// </summary>
public sealed class GetTopClientsUseCase
{
    private readonly AppDbContext _context;

    public GetTopClientsUseCase(AppDbContext context) => _context = context;

    public async Task<IEnumerable<TopClientDto>> ExecuteAsync(int top = 5, CancellationToken ct = default)
    {
        var cancelledId = await _context.BookingStatuses.AsNoTracking()
            .Where(s => s.Name == "Cancelled")
            .Select(s => (int?)s.Id)
            .FirstOrDefaultAsync(ct);

        var bookingsQuery = _context.Bookings.AsNoTracking();
        if (cancelledId.HasValue)
            bookingsQuery = bookingsQuery.Where(b => b.BookingStatusId != cancelledId.Value);

        var rows = await (
            from b in bookingsQuery
            join c in _context.Customers.AsNoTracking() on b.ClientId equals c.Id
            join p in _context.Persons.AsNoTracking() on c.PersonId equals p.Id
            join dt in _context.DocumentTypes.AsNoTracking() on p.DocumentTypeId equals dt.Id into dtg
            from dt in dtg.DefaultIfEmpty()
            join u in _context.Users.AsNoTracking() on p.Id equals u.Person_Id into ug
            from u in ug.DefaultIfEmpty()
            select new
            {
                b.ClientId,
                b.TotalAmount,
                p.FirstName,
                p.LastName,
                p.DocumentNumber,
                DocTypeCode = dt != null ? dt.Code : "",
                Username = u != null ? u.Username : null
            }
        ).ToListAsync(ct);

        return rows
            .GroupBy(x => x.ClientId)
            .Select(g =>
            {
                var head = g.First();
                var username = g.Select(x => x.Username).FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
                var displayName = ReportClientFormatting.BuildClientDisplayName(head.FirstName, head.LastName, username);
                var identityDoc = ReportClientFormatting.BuildIdentityDocument(head.DocTypeCode, head.DocumentNumber);

                return new TopClientDto(
                    g.Key,
                    displayName,
                    username,
                    identityDoc,
                    g.Count(),
                    g.Sum(x => x.TotalAmount));
            })
            .OrderByDescending(x => x.TotalBookings)
            .ThenByDescending(x => x.TotalSpent)
            .Take(top)
            .ToList();
    }
}
