using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Application.UseCases;
using DomainBookingFlight = GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.Aggregate.BookingFlight;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Application.Services;

public sealed class BookingFlightsService : IBookingFlightsService
{
    private readonly GetBookingFlightsUseCase _get;
    private readonly CreateBookingFlightUseCase _create;
    private readonly UpdateBookingFlightUseCase _update;
    private readonly DeleteBookingFlightUseCase _delete;

    public BookingFlightsService(
        GetBookingFlightsUseCase get,
        CreateBookingFlightUseCase create,
        UpdateBookingFlightUseCase update,
        DeleteBookingFlightUseCase delete)
    {
        _get = get;
        _create = create;
        _update = update;
        _delete = delete;
    }

    public Task<IEnumerable<DomainBookingFlight>> GetAllAsync(CancellationToken cancellationToken = default)
        => _get.GetAllAsync(cancellationToken);

    public Task<DomainBookingFlight?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _get.GetByIdAsync(id, cancellationToken);

    public Task<IEnumerable<DomainBookingFlight>> GetByBookingIdAsync(int bookingId, CancellationToken cancellationToken = default)
        => _get.GetByBookingIdAsync(bookingId, cancellationToken);

    public Task CreateAsync(int bookingId, int flightId, decimal partialAmount, CancellationToken cancellationToken = default)
        => _create.ExecuteAsync(bookingId, flightId, partialAmount, cancellationToken);

    public Task UpdateAsync(int id, int bookingId, int flightId, decimal partialAmount, CancellationToken cancellationToken = default)
        => _update.ExecuteAsync(id, bookingId, flightId, partialAmount, cancellationToken);

    public Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
        => _delete.ExecuteByIdAsync(id, cancellationToken);
}
