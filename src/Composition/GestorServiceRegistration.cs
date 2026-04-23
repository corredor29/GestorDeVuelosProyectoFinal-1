using GestorDeVuelosProyectoFinal.Auth.ui;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.UI;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.ui;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.ui;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.Airports.ui;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.ui;
using GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.CabinConfiguration.Infrastructure.ui;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.ui;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.UI;
using GestorDeVuelosProyectoFinal.Moduls.People.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.People.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.People.ui;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.Personal.ui;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.ui;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.ui;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.Routes.ui;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.ui;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.ui;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.CabinTypes.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.CabinTypes.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.Continents.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.Countries.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.Regions.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.Cities.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.ui;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.ui;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.Continents.Infrastructure.Persistence;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.Addresses.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.StreetTypes.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.DocumentTypes.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.PhoneCodes.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Infrastructure.Entity.persistencia.seeder;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.CabinConfiguration.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.Customers.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.Infrastructure.Persistence.seeders;
using GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.UI;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.Infrastructure.Persistence.Seeders;
using GestorDeVuelosProyectoFinal.Moduls.PeoplePhones.ui;
using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.CardIssuers.UI;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.Application.Services;
using GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.CardTypes.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.Invoices.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.Payments.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.Roles.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.ui;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.UI;
using GestorDeVuelosProyectoFinal.src.Moduls.ClientPortal.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.ClientPortal.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.ClientPortal.UI;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.Services;
using GestorDeVuelosProyectoFinal.src.Moduls.Reports.UI;
using GestorDeVuelosProyectoFinal.src.Shared.UI;

namespace GestorDeVuelosProyectoFinal.Composition;

public static class GestorServiceRegistration
{
    private static readonly string[] ModuleNamespacePrefixes =
    {
        "GestorDeVuelosProyectoFinal.Moduls.Continents",
        "GestorDeVuelosProyectoFinal.Moduls.Countries",
        "GestorDeVuelosProyectoFinal.Moduls.Regions",
        "GestorDeVuelosProyectoFinal.Moduls.Cities",
        "GestorDeVuelosProyectoFinal.Moduls.StreetTypes",
        "GestorDeVuelosProyectoFinal.Moduls.Addresses",
        "GestorDeVuelosProyectoFinal.Moduls.DocumentTypes",
        "GestorDeVuelosProyectoFinal.Moduls.People",
        "GestorDeVuelosProyectoFinal.Moduls.EmailDomains",
        "GestorDeVuelosProyectoFinal.src.Moduls.EmailDomains",
        "GestorDeVuelosProyectoFinal.Moduls.PhoneCodes",
        "GestorDeVuelosProyectoFinal.src.Moduls.PeopleEmails",
        "GestorDeVuelosProyectoFinal.Moduls.PeoplePhones",
        "GestorDeVuelosProyectoFinal.src.Moduls.Customers",
        "GestorDeVuelosProyectoFinal.Moduls.Airlines",
        "GestorDeVuelosProyectoFinal.Moduls.Airports",
        "GestorDeVuelosProyectoFinal.Moduls.AirportAirline",
        "GestorDeVuelosProyectoFinal.Moduls.PersonalPositions",
        "GestorDeVuelosProyectoFinal.Moduls.Personal",
        "GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates",
        "GestorDeVuelosProyectoFinal.Moduls.StaffAvailability",
        "GestorDeVuelosProyectoFinal.src.Moduls.AircraftManufacturers",
        "GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels",
        "GestorDeVuelosProyectoFinal.src.Moduls.Aircraft",
        "GestorDeVuelosProyectoFinal.src.Moduls.CabinConfiguration",
        "GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration",
        "GestorDeVuelosProyectoFinal.Moduls.CabinTypes",
        "GestorDeVuelosProyectoFinal.src.Moduls.CabinTypes",
        "GestorDeVuelosProyectoFinal.Moduls.Routes",
        "GestorDeVuelosProyectoFinal.Moduls.Seasons",
        "GestorDeVuelosProyectoFinal.Moduls.PassengerTypes",
        "GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers",
        "GestorDeVuelosProyectoFinal.src.Moduls.Rates",
        "GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus",
        "GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions",
        "GestorDeVuelosProyectoFinal.Moduls.FlightAssignments",
        "GestorDeVuelosProyectoFinal.Moduls.FlightRoles",
        "GestorDeVuelosProyectoFinal.src.Moduls.Flights",
        "GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses",
        "GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions",
        "GestorDeVuelosProyectoFinal.src.Moduls.Bookings",
        "GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights",
        "GestorDeVuelosProyectoFinal.src.Moduls.Reports",
        "GestorDeVuelosProyectoFinal.src.Moduls.Users",
        "GestorDeVuelosProyectoFinal.src.Moduls.Sessions",
        "GestorDeVuelosProyectoFinal.src.Moduls.Payments",
        "GestorDeVuelosProyectoFinal.src.Moduls.PaymentMethods",
        "GestorDeVuelosProyectoFinal.src.Moduls.PaymentMediumTypes",
        "GestorDeVuelosProyectoFinal.src.Moduls.PaymentStatuses",
        "GestorDeVuelosProyectoFinal.src.Moduls.Invoices",
        "GestorDeVuelosProyectoFinal.src.Moduls.InvoiceItems",
        "GestorDeVuelosProyectoFinal.Moduls.InvoiceItemTypes",
        "GestorDeVuelosProyectoFinal.src.Moduls.Baggage",
        "GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes",
        "GestorDeVuelosProyectoFinal.src.Moduls.Checkins",
        "GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates",
        "GestorDeVuelosProyectoFinal.Moduls.CardIssuers",
        "GestorDeVuelosProyectoFinal.src.Moduls.CardTypes",
        "GestorDeVuelosProyectoFinal.src.Moduls.Permissions",
        "GestorDeVuelosProyectoFinal.src.Moduls.RolePermissions",
        "GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles",
        "GestorDeVuelosProyectoFinal.src.Moduls.Roles",
        "GestorDeVuelosProyectoFinal.src.Moduls.Ticket",
        "GestorDeVuelosProyectoFinal.src.Moduls.TicketStates",
        "GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations",
        "GestorDeVuelosProyectoFinal.src.Moduls.Passengers",
        "GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats",
        "GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes",
    };

    private static bool IsInModuleScope(Type type)
    {
        var ns = type.Namespace ?? "";
        foreach (var p in ModuleNamespacePrefixes)
        {
            if (ns.StartsWith(p, StringComparison.Ordinal))
                return true;
        }

        return false;
    }

    public static IServiceCollection AddGestorSqlModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration);

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            MySqlConnectionOptions.UseGestorMySql(options, config);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        var assembly = typeof(AppDbContext).Assembly;

        foreach (var type in assembly.GetTypes())
        {
            if (type is not { IsClass: true, IsAbstract: false })
                continue;
            if (!type.Name.EndsWith("Repository", StringComparison.Ordinal))
                continue;
            if (!IsInModuleScope(type))
                continue;

            var repoInterfaces = type.GetInterfaces()
                .Where(i => i.Namespace?.Contains("Repositories", StringComparison.Ordinal) == true)
                .ToList();

            foreach (var iface in repoInterfaces)
                services.AddScoped(iface, type);
        }

        foreach (var type in assembly.GetTypes())
        {
            if (type is not { IsClass: true, IsAbstract: false })
                continue;
            if (!type.Name.EndsWith("UseCase", StringComparison.Ordinal))
                continue;
            if (!IsInModuleScope(type))
                continue;
            if (type.Namespace?.Contains("UseCases", StringComparison.Ordinal) != true)
                continue;

            services.AddTransient(type, type);
        }

        services.AddTransient<IContinentService, ContinentService>();
        services.AddTransient<ICountryService, CountryService>();
        services.AddTransient<IRegionService, RegionService>();
        services.AddTransient<ICityService, CityService>();
        services.AddTransient<IStreetTypeService, StreetTypeService>();
        services.AddTransient<IAddressService, AddressService>();
        services.AddTransient<IDocumentTypesService, DocumentTypesService>();
        services.AddTransient<IPersonService, PersonService>();
        services.AddTransient<IEmailDomainService, EmailDomainsService>();
        services.AddTransient<IPhoneCodesService, PhoneCodesService>();
        services.AddTransient<IPersonEmailsService, PersonEmailsService>();
        services.AddTransient<IPeoplePhonesService, PeoplePhonesService>();
        services.AddTransient<ICustomersService, CustomersService>();
        services.AddTransient<IAirlinesService, AirlinesService>();
        services.AddTransient<IAirportsService, AirportsService>();
        services.AddTransient<IAirportAirlineService, AirportAirlineService>();
        services.AddTransient<IPersonalPositionsService, PersonalPositionsService>();
        services.AddTransient<IPersonalService, PersonalService>();
        services.AddTransient<IAvailabilityStatesService, AvailabilityStatesService>();
        services.AddTransient<IStaffAvailabilityService, StaffAvailabilityService>();
        services.AddTransient<IAircraftManufacturersService, AircraftManufacturersService>();
        services.AddTransient<IAircraftModelsService, AircraftModelService>();
        services.AddTransient<IAircraftService, AircraftService>();
        services.AddTransient<ICabinTypeService, CabinTypeService>();
        services.AddTransient<ICabinConfigurationService, CabinConfigurationService>();
        services.AddTransient<IRoutesService, RoutesService>();
        services.AddTransient<IRouteStopoversService, RouteStopoversService>();
        services.AddTransient<ISeasonsService, SeasonsService>();
        services.AddTransient<IPassengerTypesService, PassengerTypesService>();
        services.AddTransient<IRatesService, RatesService>();
        services.AddTransient<IFlightStatusService, FlightStatusService>();
        services.AddTransient<IFlightStatusTransitionsService, FlightStatusTransitionsService>();
        services.AddTransient<IFlightAssignmentsService, FlightAssignmentsService>();
        services.AddTransient<IFlightRolesService, FlightRolesService>();
        services.AddTransient<IFlightsOperationalService, FlightsOperationalService>();
        services.AddTransient<IBookingStatusService, BookingStatusService>();
        services.AddTransient<IBookingStatusTransitionsService, BookingStatusTransitionsService>();
        services.AddTransient<IBookingsService, BookingsService>();
        services.AddTransient<IBookingFlightsService, BookingFlightsService>();
        services.AddTransient<DemoReferenceDataSeeder>();
        services.AddTransient<DevelopmentAuthSeeder>();
        services.AddTransient<ContinentSeeder>();
        services.AddTransient<CountrySeeder>();
        services.AddTransient<RegionSeeder>();
        services.AddTransient<CitySeeder>();
        services.AddTransient<StreetTypeSeeder>();
        services.AddTransient<AddressSeeder>();
        services.AddTransient<DocumentTypeSeeder>();
        services.AddTransient<PhoneCodeSeeder>();
        services.AddTransient<EmailDomainsSeeder>();
        services.AddTransient<PersonEmailsSeeder>();
        services.AddTransient<PeoplePhonesSeeder>();
        services.AddTransient<PersonalPositionsSeeder>();
        services.AddTransient<AirlinesSeeder>();
        services.AddTransient<AirportsSeeder>();
        services.AddTransient<AirportAirlineSeeder>();
        services.AddTransient<RoutesSeeder>();
        services.AddTransient<RouteStopoversSeeder>();
        services.AddTransient<AircraftManufacturersSeeder>();
        services.AddTransient<AircraftModelSeeder>();
        services.AddTransient<CabinTypeSeeder>();
        services.AddTransient<AircraftSeeder>();
        services.AddTransient<CabinConfigurationSeeder>();
        services.AddTransient<FlightStatusTransitionsSeeder>();
        services.AddTransient<FlightRolesSeeder>();
        services.AddTransient<AvailabilityStatesSeeder>();
        services.AddTransient<PersonalSeeder>();
        services.AddTransient<StaffAvailabilitySeeder>();
        services.AddTransient<SeatLocationTypesSeeder>();
        services.AddTransient<SeasonsSeeder>();
        services.AddTransient<RatesSeeder>();
        services.AddTransient<CustomersSeeder>();
        services.AddTransient<FlightSeatsSeeder>();
        services.AddTransient<FlightAssignmentsSeeder>();
        services.AddTransient<BookingStatusTransitionsSeeder>();
        services.AddTransient<BookingsSeeder>();
        services.AddTransient<BookingFlightsSeeder>();

        services.AddTransient<ContinentsMenu>();
        services.AddTransient<CountriesMenu>();
        services.AddTransient<RegionsMenu>();
        services.AddTransient<CitiesMenu>();
        services.AddTransient<StreetTypeMenu>();
        services.AddTransient<AddressMenu>();
        services.AddTransient<DocumentTypesMenu>();
        services.AddTransient<PersonsMenu>();
        services.AddTransient<EmailDomainsMenu>();
        services.AddTransient<PeopleEmailsMenu>();
        services.AddTransient<PhoneCodesMenu>();
        services.AddTransient<PeoplePhonesMenu>();
        services.AddTransient<CustomersMenu>();
        services.AddTransient<AirlinesMenu>();
        services.AddTransient<AirportsMenu>();
        services.AddTransient<AirportAirlineMenu>();
        services.AddTransient<PersonalPositionsMenu>();
        services.AddTransient<PersonalMenu>();
        services.AddTransient<AvailabilityStatesMenu>();
        services.AddTransient<StaffAvailabilityMenu>();
        services.AddTransient<AircraftManufacturersMenu>();
        services.AddTransient<AircraftModelsConsoleUI>();
        services.AddTransient<AircraftMenu>();
        services.AddTransient<CabinTypesMenu>();
        services.AddTransient<CabinConfigurationMenu>();
        services.AddTransient<RoutesMenu>();
        services.AddTransient<RouteStopoversMenu>();
        services.AddTransient<SeasonsMenu>();
        services.AddTransient<PassengerTypesMenu>();
        services.AddTransient<RatesMenu>();
        services.AddTransient<FlightStatusMenu>();
        services.AddTransient<FlightStatusTransitionsMenu>();
        services.AddTransient<FlightAssignmentsMenu>();
        services.AddTransient<FlightRolesMenu>();
        services.AddTransient<FlightsMenu>();
        services.AddTransient<BookingStatusesMenu>();
        services.AddTransient<BookingStatusTransitionsMenu>();
        services.AddTransient<BookingsMenu>();
        services.AddTransient<BookingFlightsMenu>();

        services.AddTransient<IUsersService, UsersService>();
        services.AddTransient<ISessionsService, SessionsService>();
        services.AddTransient<IPaymentsService, PaymentsService>();
        services.AddTransient<IPaymentMethodsService, PaymentMethodsService>();
        services.AddTransient<IPaymentMediumTypesService, PaymentMediumTypesService>();
        services.AddTransient<IPaymentStatusesService, PaymentStatusesService>();
        services.AddTransient<IInvoicesService, InvoicesService>();
        services.AddTransient<IInvoiceItemsService, InvoiceItemsService>();
        services.AddTransient<IInvoiceItemTypesService, InvoiceItemTypesService>();
        services.AddTransient<IBaggageService, BaggageService>();
        services.AddTransient<IBaggageTypesService, BaggageTypesService>();
        services.AddTransient<ICheckinsService, CheckinsService>();
        services.AddTransient<ICheckinStatesService, CheckinStatesService>();
        services.AddTransient<ICardIssuersService, CardIssuersService>();
        services.AddTransient<ICardTypesService, CardTypesService>();
        services.AddTransient<IPermissionsService, PermissionsService>();
        services.AddTransient<IRolePermissionsService, RolePermissionsService>();
        services.AddTransient<ISystemRolesService, SystemRolesService>();
        services.AddTransient<IPassengerService, PassengerService>();
        services.AddTransient<ISeatLocationTypesService, SeatLocationTypesService>();
        services.AddTransient<IFlightSeatsService, FlightSeatsService>();
        services.AddTransient<IPassengerReservationService, PassengerReservationService>();
        services.AddTransient<ITicketService, TicketService>();
        services.AddTransient<ITicketStatesService, TicketStatesService>();

        services.AddTransient<PaymentsConsoleUI>();
        services.AddTransient<PaymentMethodsConsoleUI>();
        services.AddTransient<PaymentMediumTypesConsoleUI>();
        services.AddTransient<PaymentStatusesConsoleUI>();
        services.AddTransient<InvoicesConsoleUI>();
        services.AddTransient<InvoiceItemsConsoleUI>();
        services.AddTransient<InvoiceItemTypesConsoleUI>();
        services.AddTransient<BaggageConsoleUI>();
        services.AddTransient<BaggageTypesConsoleUI>();
        services.AddTransient<CheckinsConsoleUI>();
        services.AddTransient<CardIssuersConsoleUI>();
        services.AddTransient<CardTypesConsoleUI>();
        services.AddTransient<PermissionsConsoleUI>();
        services.AddTransient<RolePermissionsConsoleUI>();
        services.AddTransient<SystemRolesConsoleUI>();
        services.AddTransient<SessionsConsoleUI>();
        services.AddTransient<UsersConsoleUI>();
        services.AddTransient<FlightSeatsMenu>();
        services.AddTransient<SeatLocationTypesMenu>();
        services.AddTransient<PassengersMenu>();

        services.AddTransient<AuthEntryMenu>();
        services.AddTransient<LoginConsoleUI>();
        services.AddTransient<AdminNavigationMenu>();
        services.AddTransient<UserPortalPlaceholderMenu>();
        

        services.AddTransient<IReportsService, ReportsService>();
        services.AddTransient<ReportsMenu>();
        services.AddTransient<IClientPortalService, ClientPortalService>();
        services.AddTransient<ClientPortalMenu>();

        return services;
    }
}
