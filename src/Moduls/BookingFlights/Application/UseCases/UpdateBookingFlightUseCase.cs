using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Application.UseCases;

public sealed class UpdateBookingFlightUseCase
{
    private readonly IBookingFlightsRepository _repository;
    private readonly IBookingsRepository _bookingsRepository;
    private readonly IFlightsRepository _flightsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBookingFlightUseCase(
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

    public async Task ExecuteAsync(int id, int bookingId, int flightId, decimal partialAmount, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(BookingFlightsId.Create(id), cancellationToken);
        if (existing is null)
            throw new InvalidOperationException($"No existe el booking_flight con id {id}.");

        await EnsureReferencesExistAsync(bookingId, flightId, cancellationToken);

        var duplicate = await _repository.GetByBookingAndFlightAsync(
            BookingId.Create(bookingId),
            FlightsId.Create(flightId),
            cancellationToken);

        if (duplicate is not null && duplicate.Id?.Value != id)
            throw new InvalidOperationException("Ya existe otro booking_flight con la misma reserva y vuelo.");

        existing.Update(bookingId, flightId, partialAmount);
        await _repository.UpdateAsync(existing, cancellationToken);
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
