namespace GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.Interfaces;

// ── DTOs ────────────────────────────────────────────────────────────────────
// ( Data Transfer Objects )
public sealed record FlightOccupancyDto(
    string FlightCode,
    string OriginIata,
    string DestinationIata,
    int TotalCapacity,
    int OccupiedSeats,
    int AvailableSeats,
    double OccupancyPercent);

public sealed record TopDestinationDto(
    string AirportName,
    string IataCode,
    int TotalBookings);

public sealed record TopClientDto(
    int ClientId,
    /// <summary>Nombre corto para reportes: login del portal si aplica patrón "Cliente + login", si no nombre y apellidos.</summary>
    string ClientDisplayName,
    string? AccountUsername,
    /// <summary>Tipo de documento + número (persona en sistema).</summary>
    string IdentityDocument,
    int TotalBookings,
    decimal TotalSpent);

public sealed record BookingsByStatusDto(
    string StatusName,
    int Count,
    decimal TotalAmount);

public sealed record RevenueByAirlineDto(
    string AirlineName,
    string IataCode,
    decimal TotalRevenue,
    int TotalFlights);

public sealed record TicketsByDateDto(
    string TicketCode,
    string FlightCode,
    string OriginIata,
    string DestinationIata,
    DateTime IssuedAt,
    string TicketStatus);

// ── Interface ────────────────────────────────────────────────────────────────

public interface IReportsService
{
    Task<IEnumerable<FlightOccupancyDto>> GetFlightOccupancyAsync(CancellationToken ct = default);
    Task<IEnumerable<FlightOccupancyDto>> GetAvailableFlightsAsync(CancellationToken ct = default);
    Task<IEnumerable<TopClientDto>> GetTopClientsAsync(int top = 5, CancellationToken ct = default);

    /// <summary>Todos los usuarios con persona asociada + reservas/gasto (incluye 0 reservas).</summary>
    Task<IEnumerable<TopClientDto>> GetRegisteredUsersReportAsync(CancellationToken ct = default);
    Task<IEnumerable<TopDestinationDto>> GetTopDestinationsAsync(int top = 5, CancellationToken ct = default);
    Task<IEnumerable<BookingsByStatusDto>> GetBookingsByStatusAsync(CancellationToken ct = default);
    Task<IEnumerable<RevenueByAirlineDto>> GetRevenueByAirlineAsync(CancellationToken ct = default);
    Task<IEnumerable<TicketsByDateDto>> GetTicketsByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default);
}