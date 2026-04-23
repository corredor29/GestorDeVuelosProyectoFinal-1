using GestorDeVuelosProyectoFinal.src.Moduls.ClientPortal.Application.Models;

namespace GestorDeVuelosProyectoFinal.src.Moduls.ClientPortal.Application.Interfaces;

public interface IClientPortalService
{
    Task<ClientContext> EnsureClientContextAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<(int Id, string Label)>> SearchCitiesAsync(string query, CancellationToken cancellationToken);

    Task<IReadOnlyList<(int Id, string City, string Region, string Country)>> ListAllCitiesAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<(int Id, string Name)>> GetCabinTypesAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<(int Id, string Name)>> GetDocumentTypesAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<(string FlightCode, string Origin, string Destination, DateTime DepartureAt, DateTime ArrivalAt, int AvailableSeats)>> ListAvailableFlightsAsync(
        CancellationToken cancellationToken);

    Task<IReadOnlyList<FlightSearchResult>> SearchFlightsAsync(
        int originCityId,
        int destinationCityId,
        DateOnly? date,
        int cabinTypeId,
        CancellationToken cancellationToken);

    Task<PurchaseResult> PurchaseAsync(PurchaseRequest request, CancellationToken cancellationToken);

    Task<(decimal TotalAmount, IReadOnlyList<(string Leg, decimal Amount)> Lines)> PreviewPurchaseAsync(
        PurchaseRequest request,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<(int BookingId, string ReservationReference, DateTime BookedAt, string Status, int FlightsCount, int TicketsCount, decimal TotalAmount)>> GetMyBookingsAsync(
        int clientId,
        CancellationToken cancellationToken);

    Task<(int BookingId, string Status, decimal TotalAmount,
        IReadOnlyList<(string FlightCode, string RouteLabel, DateTime DepartureAt, DateTime ArrivalAt, int AvailableSeats)> Flights,
        IReadOnlyList<(string FullName, string Document)> Passengers,
        string PaymentStatus,
        bool HasAnyCheckin,
        IReadOnlyList<int> TicketIds,
        IReadOnlyList<BookingTicketRow> Tickets)> GetBookingDetailsAsync(int bookingId, int clientId, CancellationToken cancellationToken);

    Task CancelBookingAsync(int bookingId, int clientId, CancellationToken cancellationToken);

    Task<IReadOnlyList<TicketSummary>> GetMyTicketsAsync(int clientId, CancellationToken cancellationToken);

    Task<(TicketSummary Ticket, string? BoardingPassNumber, string? SeatCode, string? CabinTypeName, bool IsCheckedIn)> GetTicketDetailsAsync(
        int ticketId,
        int clientId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<(int BookingId, IReadOnlyList<(int FlightId, string FlightCode, string RouteLabel, DateTime DepartureAt, DateTime ArrivalAt)> Flights)>> FindBookingsForCheckinAsync(
        int clientId,
        string? bookingLookup,
        string? passengerLastName,
        DateTime utcNow,
        CancellationToken cancellationToken);

    Task<CheckinCandidatesResult> GetCheckinCandidatesAsync(
        int clientId,
        int bookingId,
        int flightId,
        DateTime utcNow,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<string>> GetAvailableSeatCodesAsync(
        int flightId,
        CancellationToken cancellationToken);

    Task<(string BoardingPassNumber, string SeatCode, string CabinTypeName, string PassengerFullName, string FlightCode,
        string OriginIata, string DestinationIata, DateTime DepartureAt, DateTime ArrivalAt,
        decimal AdditionalSeatChoiceCharge)> PerformOnlineCheckinAsync(
        int clientId,
        int passengerReservationId,
        int flightId,
        string? desiredSeatCode,
        bool allowRandomSeat,
        DateTime utcNow,
        CancellationToken cancellationToken);
}
