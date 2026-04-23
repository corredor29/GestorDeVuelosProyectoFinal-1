using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Infrastructure.Persistence.seeders;

public class BookingStatusesSeeder
{
    public static async Task Seed(AppDbContext context)
    {
        // Si ya existen registros, no hacemos nada
        if (context.BookingStatuses.Any()) return;

        var initialStatuses = new List<BookingStatusesEntity>
        {
            new BookingStatusesEntity { Name = "Pending" },
            new BookingStatusesEntity { Name = "Confirmed" },
            new BookingStatusesEntity { Name = "Cancelled" },
            new BookingStatusesEntity { Name = "Expired" }
        };

        await context.BookingStatuses.AddRangeAsync(initialStatuses);
        await context.SaveChangesAsync();
    }
}