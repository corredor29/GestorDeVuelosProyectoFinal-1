using GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Infrastructure.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.src.Moduls.ClientPortal.Application.Models;

public enum TripType
{
    OneWay = 1,
    RoundTrip = 2
}

public sealed record FlightSearchRequest(
    TripType TripType,
    int OriginCityId,
    int DestinationCityId,
    DateOnly? OutboundDate,
    DateOnly? ReturnDate,
    int Adults,
    int Minors,
    int CabinTypeId);

public sealed record FlightSearchResult(
    FlightEntity Flight,
    RouteEntity Route,
    AirportEntity OriginAirport,
    AirportEntity DestinationAirport,
    CityEntity OriginCity,
    CityEntity DestinationCity,
    RegionEntity OriginRegion,
    RegionEntity DestinationRegion,
    CountryEntity OriginCountry,
    CountryEntity DestinationCountry,
    decimal BasePrice);

public sealed record ClientContext(int UserId, int PersonId, int ClientId, string Username, string RoleName);

public sealed record PassengerInput(
    string FirstName,
    string LastName,
    string DocumentNumber,
    int DocumentTypeId,
    string PassengerCategory);

public sealed record BookingTicketRow(
    int TicketId,
    string TicketCode,
    string PassengerFullName,
    string PassengerDocument,
    string FlightCode,
    string RouteLabel,
    DateTime DepartureAt,
    DateTime ArrivalAt,
    int StopoversCount,
    bool HasExtraBaggage,
    string? SeatCode = null);

public sealed record ContactInput(
    string Email,
    string CountryCode,
    string PhoneNumber);

public enum SimulatedPaymentMethod
{
    CreditCard = 1,
    Pse = 2,
    Nequi = 3,
    DebitCard = 4
}

public sealed record SimulatedCardInput(
    string Number,
    string Holder,
    string Expiration,
    string SecurityCode);

public sealed record PurchaseRequest(
    FlightSearchRequest Search,
    int OutboundFlightId,
    int? ReturnFlightId,
    IReadOnlyList<PassengerInput> Passengers,
    ContactInput Contact,
    SimulatedPaymentMethod PaymentMethod,
    SimulatedCardInput? Card,
    decimal TotalAmount,
    int ExtraBaggagePieces = 0,
    bool PrioritySeatSelectionAtPurchase = false);

public sealed record TicketSummary(
    int TicketId,
    string TicketCode,
    int PassengerReservationId,
    int FlightId,
    string FlightCode,
    string OriginIata,
    string DestinationIata,
    DateTime DepartureAt,
    DateTime EstimatedArrivalAt,
    string PassengerName,
    string TicketState,
    string? SeatCode = null);

public sealed record CheckinPassengerRow(
    int PassengerReservationId,
    int PassengerId,
    string PassengerName,
    string PassengerDocument,
    string TicketCode,
    string TicketState,
    string? SeatCode = null,
    string? BoardingPassNumber = null);

public sealed record CheckinCandidatesResult(
    int BookingId,
    int FlightId,
    string FlightCode,
    string OriginIata,
    string DestinationIata,
    DateTime DepartureAt,
    DateTime ArrivalAt,
    IReadOnlyList<CheckinPassengerRow> PendingPassengers,
    IReadOnlyList<CheckinPassengerRow> CheckedInPassengers);

public sealed record PurchaseResult(
    int BookingId,
    string ReservationReference,
    IReadOnlyList<TicketSummary> Tickets);
