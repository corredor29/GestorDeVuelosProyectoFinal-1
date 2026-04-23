using GestorDeVuelosProyectoFinal.Auth.Application;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.UI;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.ui;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.ui;
using GestorDeVuelosProyectoFinal.Moduls.Airports.ui;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.ui;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.ui;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.UI;
using GestorDeVuelosProyectoFinal.Moduls.People.ui;
using GestorDeVuelosProyectoFinal.Moduls.Personal.ui;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.ui;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.ui;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.ui;
using GestorDeVuelosProyectoFinal.Moduls.Routes.ui;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.ui;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.ui;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.CabinConfiguration.Infrastructure.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.CabinTypes.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.Continents.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.Countries.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.Regions.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.Cities.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.UI;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.ui;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.UI;
using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.UI;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.Reports.UI;

namespace GestorDeVuelosProyectoFinal.Auth.ui;

// Este menú es el hub principal del administrador.
// Agrupa los módulos por áreas para que el panel no se sienta como una lista infinita.
public sealed class AdminNavigationMenu
{
    private readonly IServiceScopeFactory _scopes;

    public AdminNavigationMenu(IServiceScopeFactory scopes)
    {
        _scopes = scopes;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(new FigletText("Administración").Color(Color.Green1));
            var userLabel = Markup.Escape(ApplicationSession.Username ?? "—");
            var roleLabel = Markup.Escape(ApplicationSession.Role ?? "—");
            AnsiConsole.Write(
                new Rule($"[bold green]Panel de administración[/]  [dim]·[/]  [white]{userLabel}[/]  [grey]({roleLabel})[/]")
                    .RuleStyle(new Style(foreground: Color.Green1))
                    .LeftJustified());

            AnsiConsole.WriteLine();

            // En vez de tirar todos los módulos juntos, primero agrupamos por áreas grandes.
            var section = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold white]Elige un área[/]  [dim]— cada opción abre sus módulos[/]")
                    .HighlightStyle(new Style(foreground: Color.Green1, decoration: Decoration.Bold))
                    .PageSize(18)
                    .AddChoices(
                        "Geografía (catálogos base)",
                        "Direcciones",
                        "Personas",
                        "Contacto (correos y teléfonos)",
                        "Clientes",
                        "Aerolíneas y aeropuertos",
                        "Personal",
                        "Configuración de vuelos (estados y transiciones)",
                        "Aeronaves (catálogos y flota)",
                        "Rutas y tarifas",
                        "Vuelos y tripulación (operación)",
                        "Asientos y pasajeros",
                        "Reservas",
                        "Pagos y facturación",
                        "Equipaje y check-in",
                        "Reportes",
                        "Usuarios y permisos",
                        "Cerrar sesión"));

            if (section == "Cerrar sesión")
                return;

            await RunSectionAsync(section, cancellationToken);
        }
    }

    private async Task RunSectionAsync(string section, CancellationToken cancellationToken)
    {
        using var scope = _scopes.CreateScope();
        var sp = scope.ServiceProvider;

        // Cada sección abre un segundo nivel más acotado con sus módulos relacionados.
        switch (section)
        {
            case "Geografía (catálogos base)":
                await PickAndRunAsync(
                    cancellationToken,
                    ("Continentes", () => sp.GetRequiredService<ContinentsMenu>().RunAsync(cancellationToken)),
                    ("Países", () => sp.GetRequiredService<CountriesMenu>().RunAsync(cancellationToken)),
                    ("Regiones", () => sp.GetRequiredService<RegionsMenu>().RunAsync(cancellationToken)),
                    ("Ciudades", () => sp.GetRequiredService<CitiesMenu>().RunAsync(cancellationToken)));
                break;

            case "Direcciones":
                await PickAndRunAsync(
                    cancellationToken,
                    ("Tipos de vía", () => sp.GetRequiredService<StreetTypeMenu>().RunAsync(cancellationToken)),
                    ("Direcciones", () => sp.GetRequiredService<AddressMenu>().RunAsync(cancellationToken)));
                break;

            case "Personas":
                await PickAndRunAsync(
                    cancellationToken,
                    ("Tipos de documento", () => sp.GetRequiredService<DocumentTypesMenu>().RunAsync(cancellationToken)),
                    ("Personas", () => sp.GetRequiredService<PersonsMenu>().RunAsync(cancellationToken)));
                break;

            case "Contacto (correos y teléfonos)":
                await PickAndRunAsync(
                    cancellationToken,
                    ("Dominios de correo", () => sp.GetRequiredService<EmailDomainsMenu>().RunAsync(cancellationToken)),
                    ("Correos de persona", () => sp.GetRequiredService<PeopleEmailsMenu>().RunAsync(cancellationToken)),
                    ("Códigos telefónicos", () => sp.GetRequiredService<PhoneCodesMenu>().RunAsync(cancellationToken)),
                    ("Teléfonos de persona", () => sp.GetRequiredService<PeoplePhonesMenu>().RunAsync(cancellationToken)));
                break;

            case "Clientes":
                await PickAndRunAsync(
                    cancellationToken,
                    ("Clientes", () => sp.GetRequiredService<CustomersMenu>().RunAsync(cancellationToken)));
                break;

            case "Aerolíneas y aeropuertos":
                await PickAndRunAsync(
                    cancellationToken,
                    ("Aerolíneas", () => sp.GetRequiredService<AirlinesMenu>().RunAsync(cancellationToken)),
                    ("Aeropuertos", () => sp.GetRequiredService<AirportsMenu>().RunAsync(cancellationToken)),
                    ("Aerolínea en aeropuerto", () => sp.GetRequiredService<AirportAirlineMenu>().RunAsync(cancellationToken)));
                break;

            case "Personal":
                await PickAndRunAsync(
                    cancellationToken,
                    ("Cargos / posiciones", () => sp.GetRequiredService<PersonalPositionsMenu>().RunAsync(cancellationToken)),
                    ("Personal", () => sp.GetRequiredService<PersonalMenu>().RunAsync(cancellationToken)),
                    ("Estados de disponibilidad", () => sp.GetRequiredService<AvailabilityStatesMenu>().RunAsync(cancellationToken)),
                    ("Disponibilidad del personal", () => sp.GetRequiredService<StaffAvailabilityMenu>().RunAsync(cancellationToken)));
                break;

            case "Configuración de vuelos (estados y transiciones)":
                await PickAndRunAsync(
                    cancellationToken,
                    ("Estados de vuelo", () => sp.GetRequiredService<FlightStatusMenu>().RunAsync(cancellationToken)),
                    ("Transiciones permitidas", () => sp.GetRequiredService<FlightStatusTransitionsMenu>().RunAsync(cancellationToken)));
                break;

            case "Aeronaves (catálogos y flota)":
                await PickAndRunAsync(
                    cancellationToken,
                    ("Fabricantes", () => sp.GetRequiredService<AircraftManufacturersMenu>().RunAsync(cancellationToken)),
                    ("Modelos", () => sp.GetRequiredService<AircraftModelsConsoleUI>().RunAsync(cancellationToken)),
                    ("Aeronaves", () => sp.GetRequiredService<AircraftMenu>().RunAsync(cancellationToken)),
                    ("Tipos de cabina", () => sp.GetRequiredService<CabinTypesMenu>().RunAsync(cancellationToken)),
                    ("Configuración de cabina", () => sp.GetRequiredService<CabinConfigurationMenu>().RunAsync(cancellationToken)));
                break;

            case "Rutas y tarifas":
                await PickAndRunAsync(
                    cancellationToken,
                    ("Rutas", () => sp.GetRequiredService<RoutesMenu>().RunAsync(cancellationToken)),
                    ("Escalas", () => sp.GetRequiredService<RouteStopoversMenu>().RunAsync(cancellationToken)),
                    ("Temporadas", () => sp.GetRequiredService<SeasonsMenu>().RunAsync(cancellationToken)),
                    ("Tipos de pasajero", () => sp.GetRequiredService<PassengerTypesMenu>().RunAsync(cancellationToken)),
                    ("Tarifas", () => sp.GetRequiredService<RatesMenu>().RunAsync(cancellationToken)));
                break;

            case "Vuelos y tripulación (operación)":
                await PickAndRunAsync(
                    cancellationToken,
                    ("Asignaciones de tripulación", () => sp.GetRequiredService<FlightAssignmentsMenu>().RunAsync(cancellationToken)),
                    ("Roles de tripulación", () => sp.GetRequiredService<FlightRolesMenu>().RunAsync(cancellationToken)),
                    ("Vuelos", () => sp.GetRequiredService<FlightsMenu>().RunAsync(cancellationToken)));
                break;

            case "Reservas":
                await PickAndRunAsync(
                    cancellationToken,
                    ("Estados de reserva", () => sp.GetRequiredService<BookingStatusesMenu>().Show()),
                    ("Transiciones de estado", () => sp.GetRequiredService<BookingStatusTransitionsMenu>().Show(cancellationToken)),
                    ("Reservas", () => sp.GetRequiredService<BookingsMenu>().RunAsync(cancellationToken)),
                    ("Vuelos por reserva", () => sp.GetRequiredService<BookingFlightsMenu>().RunAsync(cancellationToken)));
                break;

            case "Pagos y facturación":
                await PickAndRunAsync(
                    cancellationToken,
                    ("Estados de pago", () => sp.GetRequiredService<PaymentStatusesConsoleUI>().ShowAsync()),
                    ("Tipos de medio de pago", () => sp.GetRequiredService<PaymentMediumTypesConsoleUI>().ShowAsync()),
                    ("Métodos de pago", () => sp.GetRequiredService<PaymentMethodsConsoleUI>().ShowAsync()),
                    ("Emisores de tarjeta", () => sp.GetRequiredService<CardIssuersConsoleUI>().ShowAsync()),
                    ("Tipos de tarjeta", () => sp.GetRequiredService<CardTypesConsoleUI>().ShowAsync()),
                    ("Pagos", () => sp.GetRequiredService<PaymentsConsoleUI>().ShowAsync()),
                    ("Tipos de ítem de factura", () => sp.GetRequiredService<InvoiceItemTypesConsoleUI>().ShowAsync()),
                    ("Ítems de factura", () => sp.GetRequiredService<InvoiceItemsConsoleUI>().ShowAsync()),
                    ("Facturas", () => sp.GetRequiredService<InvoicesConsoleUI>().ShowAsync()));
                break;

            case "Equipaje y check-in":
                await PickAndRunAsync(
                    cancellationToken,
                    ("Tipos de equipaje", () => sp.GetRequiredService<BaggageTypesConsoleUI>().ShowAsync()),
                    ("Equipaje", () => sp.GetRequiredService<BaggageConsoleUI>().ShowAsync()),
                    ("Check-in", () => sp.GetRequiredService<CheckinsConsoleUI>().ShowAsync()));
                break;

            case "Reportes":
                await PickAndRunAsync(
                    cancellationToken,
                    ("Reportes y estadísticas", () => sp.GetRequiredService<ReportsMenu>().RunAsync(cancellationToken)));
                break;

            case "Usuarios y permisos":
                await PickAndRunAsync(
                    cancellationToken,
                    ("Usuarios", () => sp.GetRequiredService<UsersConsoleUI>().ShowAsync()),
                    ("Sesiones", () => sp.GetRequiredService<SessionsConsoleUI>().ShowAsync()),
                    ("Roles de sistema", () => sp.GetRequiredService<SystemRolesConsoleUI>().ShowAsync()),
                    ("Permisos", () => sp.GetRequiredService<PermissionsConsoleUI>().ShowAsync()),
                    ("Permisos por rol", () => sp.GetRequiredService<RolePermissionsConsoleUI>().ShowAsync()));
                break;

            case "Asientos y pasajeros":
                await PickAndRunAsync(
                    cancellationToken,
                    ("Pasajeros", () => sp.GetRequiredService<PassengersMenu>().Show()),
                    ("Tipos de ubicación de asiento", () => sp.GetRequiredService<SeatLocationTypesMenu>().RunAsync(cancellationToken)),
                    ("Asientos de vuelo", () => sp.GetRequiredService<FlightSeatsMenu>().RunAsync(cancellationToken)));
                break;
        }
    }

    private static async Task PickAndRunAsync(CancellationToken ct, params (string Title, Func<Task> Run)[] items)
    {
        var back = "Volver al menú principal";
        var labels = items.Select(i => i.Title).Append(back).ToList();
        var pick = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold white]Módulos de esta área[/]  [dim]—[/]  [grey]volver al panel principal al final[/]")
                .HighlightStyle(new Style(foreground: Color.DeepSkyBlue1, decoration: Decoration.Bold))
                .AddChoices(labels));

        if (pick == back)
            return;

        var chosen = items.First(i => i.Title == pick);
        await chosen.Run();
    }
}
