using GestorDeVuelosProyectoFinal.Moduls.Airlines.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Infrastructure.Persistence;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.CabinConfiguration.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Infrastructure.Entity.persistencia.seeder;
using GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightReservations.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.Extensions.DependencyInjection;

namespace GestorDeVuelosProyectoFinal.Composition;

/// <summary>
/// Ejecuta todos los seeders de demostración en orden de dependencias (después de migraciones).
/// </summary>
public static class DevelopmentDataInitializer
{
    public static async Task EnsureDevelopmentDataAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        // Primero dejamos listas las cuentas demo para que siempre se pueda entrar
        // al sistema en una base recién creada.
        await Invoke(services, async s => await s.GetRequiredService<DevelopmentAuthSeeder>().SeedAsync(cancellationToken));

        // Esta tanda construye la base geográfica y los catálogos principales.
        await Invoke(services, async s => await s.GetRequiredService<ContinentSeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<CountrySeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<RegionSeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<CitySeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<StreetTypeSeeder>().SeedAsync(cancellationToken));
        await Invoke(services, async s => await s.GetRequiredService<AddressSeeder>().SeedAsync(cancellationToken));
        await Invoke(services, async s => await s.GetRequiredService<DocumentTypeSeeder>().SeedAsync(cancellationToken));
        await Invoke(services, async s => await s.GetRequiredService<PhoneCodeSeeder>().SeedAsync(cancellationToken));

        await Invoke(services, async s =>
            await s.GetRequiredService<DemoReferenceDataSeeder>().SeedFlightStatusesPassengerTypesAndDemoPersonsAsync(cancellationToken));

        await Invoke(services, async s => await s.GetRequiredService<EmailDomainsSeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<PersonEmailsSeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<PeoplePhonesSeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<PersonalPositionsSeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<AirlinesSeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<AirportsSeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<AirportAirlineSeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<RoutesSeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<RouteStopoversSeeder>().SeedAsync(cancellationToken));
        await Invoke(services, async s => await s.GetRequiredService<AircraftManufacturersSeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<AircraftModelSeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<CabinTypeSeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<AircraftSeeder>().SeedAsync(cancellationToken));
        await Invoke(services, async s => await s.GetRequiredService<CabinConfigurationSeeder>().SeedAsync());

        await Invoke(services, async s => await s.GetRequiredService<FlightStatusTransitionsSeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<FlightRolesSeeder>().SeedAsync(cancellationToken));
        await Invoke(services, async s => await s.GetRequiredService<AvailabilityStatesSeeder>().SeedAsync());

        await Invoke(services, async s => await s.GetRequiredService<DemoReferenceDataSeeder>().SeedDemoFlightsAsync(cancellationToken));
        await Invoke(services, async s => await s.GetRequiredService<DemoReferenceDataSeeder>().SeedPairedReturnFlightsAsync(cancellationToken));

        await Invoke(services, async s => await s.GetRequiredService<PersonalSeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<StaffAvailabilitySeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<SeatLocationTypesSeeder>().SeedAsync(cancellationToken));
        await Invoke(services, async s => await s.GetRequiredService<SeasonsSeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<RatesSeeder>().SeedAsync());

        // Estos catálogos mínimos evitan que pagos y facturación queden vacíos al probar.
        await Invoke(services, async s =>
        {
            var db = s.GetRequiredService<AppDbContext>();
            await PaymentStatusesSeeder.SeedAsync(db, cancellationToken);
            await PaymentMediumTypesSeeder.SeedAsync(db, cancellationToken);
            await CardIssuersSeeder.SeedAsync(db, cancellationToken);
            await CardTypesSeeder.SeedAsync(db, cancellationToken);
            await PaymentMethodsSeeder.SeedAsync(db, cancellationToken);
            await InvoiceItemTypesSeeder.SeedAsync(db, cancellationToken);
        });

        // Hacemos lo mismo para check-in y equipaje.
        await Invoke(services, async s =>
        {
            var db = s.GetRequiredService<AppDbContext>();
            await CheckinStatesSeeder.SeedAsync(db, cancellationToken);
            await BaggageTypesSeeder.SeedAsync(db, cancellationToken);
        });

        await Invoke(services, async s =>
        {
            var db = s.GetRequiredService<AppDbContext>();
            await BookingStatusesSeeder.Seed(db);
        });

        await Invoke(services, async s => await s.GetRequiredService<BookingStatusTransitionsSeeder>().SeedAsync(cancellationToken));
        await Invoke(services, async s => await s.GetRequiredService<CustomersSeeder>().SeedAsync());
        await Invoke(services, async s => await s.GetRequiredService<BookingsSeeder>().SeedAsync(cancellationToken));
        await Invoke(services, async s => await s.GetRequiredService<BookingFlightsSeeder>().SeedAsync(cancellationToken));

        // Esta parte ya depende de reservas previas, por eso va más abajo.
        await Invoke(services, async s =>
        {
            var db = s.GetRequiredService<AppDbContext>();
            await PaymentsSeeder.SeedAsync(db, cancellationToken);
            await InvoicesSeeder.SeedAsync(db, cancellationToken);
            await InvoiceItemsSeeder.SeedAsync(db, cancellationToken);
        });

        await Invoke(services, async s =>
        {
            var db = s.GetRequiredService<AppDbContext>();
            await PassengersSeeder.Seed(db);
        });

        await Invoke(services, async s => await s.GetRequiredService<FlightSeatsSeeder>().SeedAsync(cancellationToken));
        await Invoke(services, async s => await s.GetRequiredService<FlightAssignmentsSeeder>().SeedAsync(cancellationToken));

        // Cerramos con la cadena demo de tickets, check-ins y equipaje.
        await Invoke(services, async s =>
        {
            var db = s.GetRequiredService<AppDbContext>();
            await TicketStatesSeeder.SeedAsync(db, cancellationToken);
            await FlightReservationsSeeder.SeedAsync(db, cancellationToken);
            await PassengerReservationsSeeder.SeedAsync(db, cancellationToken);
            await TicketsSeeder.SeedAsync(db, cancellationToken);
            await CheckinsSeeder.SeedAsync(db, cancellationToken);
            await BaggageSeeder.SeedAsync(db, cancellationToken);
        });
    }

    private static async Task Invoke(IServiceProvider root, Func<IServiceProvider, Task> work)
    {
        // Cada seeder corre en su propio scope para no mezclar entidades rastreadas
        // entre una ejecución y la siguiente.
        var scopeFactory = root.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        await work(scope.ServiceProvider);
    }
}
