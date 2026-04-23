using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Application.UseCases;

public sealed class CreateBookingFlightUseCase
{
    private readonly IBookingFlightsRepository _repository;
    private readonly IBookingsRepository _bookingsRepository;
    private readonly IFlightsRepository _flightsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookingFlightUseCase(
        IBookingFlightsRepository repository,
        IBookingsRepository bookingsRepository,
        IFlightsRepository flightsRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _bookingsRepository = bookingsRepository;
        _flightsRepository = flightsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(int bookingId, int flightId, decimal partialAmount, CancellationToken cancellationToken = default)
    {
        await EnsureReferencesExistAsync(bookingId, flightId, cancellationToken);

        var duplicate = await _repository.GetByBookingAndFlightAsync(
            BookingId.Create(bookingId),
            FlightsId.Create(flightId),
            cancellationToken);

        if (duplicate is not null)
            throw new InvalidOperationException("Ya existe un booking_flight con la misma reserva y vuelo.");

        var bookingFlight = BookingFlight.Create(bookingId, flightId, partialAmount);
        await _repository.SaveAsync(bookingFlight, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureReferencesExistAsync(int bookingId, int flightId, CancellationToken cancellationToken)
    {
        if (await _bookingsRepository.GetByIdAsync(BookingId.Create(bookingId), cancellationToken) is null)
            throw new InvalidOperationException($"No existe la reserva con id {bookingId}.");

        if (await _flightsRepository.GetByIdAsync(FlightsId.Create(flightId), cancellationToken) is null)
            throw new InvalidOperationException($"No existe el vuelo con id {flightId}.");
    }
}
