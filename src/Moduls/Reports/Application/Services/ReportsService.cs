using GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.UseCases;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Reports.Application.Services;

public sealed class ReportsService : IReportsService
{
    private readonly GetFlightOccupancyUseCase _occupancy;
    private readonly GetAvailableFlightsUseCase _available;
    private readonly GetTopClientsUseCase _topClients;
    private readonly GetRegisteredUsersReportUseCase _registeredUsers;
    private readonly GetTopDestinationsUseCase _topDest;
    private readonly GetBookingsByStatusUseCase _byStatus;
    private readonly GetRevenueByAirlineUseCase _revenue;
    private readonly GetTicketsByDateRangeUseCase  _tickets;

    public ReportsService(
        GetFlightOccupancyUseCase occupancy,
        GetAvailableFlightsUseCase available,
        GetTopClientsUseCase topClients,
        GetRegisteredUsersReportUseCase registeredUsers,
        GetTopDestinationsUseCase topDest,
        GetBookingsByStatusUseCase byStatus,
        GetRevenueByAirlineUseCase revenue,
        GetTicketsByDateRangeUseCase tickets)
    {
        _occupancy = occupancy;
        _available = available;
        _topClients = topClients;
        _registeredUsers = registeredUsers;
        _topDest = topDest;
        _byStatus = byStatus;
        _revenue = revenue;
        _tickets = tickets;
    }

    public Task<IEnumerable<FlightOccupancyDto>> GetFlightOccupancyAsync(CancellationToken ct = default)
        => _occupancy.ExecuteAsync(ct);

    public Task<IEnumerable<FlightOccupancyDto>> GetAvailableFlightsAsync(CancellationToken ct = default)
        => _available.ExecuteAsync(ct);

    public Task<IEnumerable<TopClientDto>> GetTopClientsAsync(int top = 5, CancellationToken ct = default)
        => _topClients.ExecuteAsync(top, ct);

    public Task<IEnumerable<TopClientDto>> GetRegisteredUsersReportAsync(CancellationToken ct = default)
        => _registeredUsers.ExecuteAsync(ct);

    public Task<IEnumerable<TopDestinationDto>> GetTopDestinationsAsync(int top = 5, CancellationToken ct = default)
        => _topDest.ExecuteAsync(top, ct);

    public Task<IEnumerable<BookingsByStatusDto>> GetBookingsByStatusAsync(CancellationToken ct = default)
        => _byStatus.ExecuteAsync(ct);

    public Task<IEnumerable<RevenueByAirlineDto>> GetRevenueByAirlineAsync(CancellationToken ct = default)
        => _revenue.ExecuteAsync(ct);

    public Task<IEnumerable<TicketsByDateDto>> GetTicketsByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default)
        => _tickets.ExecuteAsync(from, to, ct);
}