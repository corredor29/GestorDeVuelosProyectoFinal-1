using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Application.UseCases;

public sealed class GetBookingFlightsUseCase
{
    private readonly IBookingFlightsRepository _repository;

    public GetBookingFlightsUseCase(IBookingFlightsRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<BookingFlight>> GetAllAsync(CancellationToken cancellationToken = default)
        => _repository.GetAllAsync(cancellationToken);

    public Task<BookingFlight?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _repository.GetByIdAsync(BookingFlightsId.Create(id), cancellationToken);

    public Task<IEnumerable<BookingFlight>> GetByBookingIdAsync(int bookingId, CancellationToken cancellationToken = default)
        => _repository.GetByBookingIdAsync(BookingId.Create(bookingId), cancellationToken);
}
