using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Infrastructure.Persistence.seeders;

public class PassengersSeeder
{
    public static async Task Seed(AppDbContext context)
    {
        if (context.Passengers.Any()) return;

        var initialPassengers = new List<PassengersEntity>
        {
            // Asegúrate de que estos IDs de personas y tipos existan en sus respectivas tablas
            new PassengersEntity { PersonId = 1, PassengerTypeId = 1 }, 
            new PassengersEntity { PersonId = 2, PassengerTypeId = 2 }
        };

        await context.Passengers.AddRangeAsync(initialPassengers);
        await context.SaveChangesAsync();
    }
}