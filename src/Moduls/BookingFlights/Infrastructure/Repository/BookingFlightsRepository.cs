using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Infrastructure.Repository;

public sealed class BookingFlightsRepository : IBookingFlightsRepository
{
    private readonly AppDbContext _context;

    public BookingFlightsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BookingFlight?> GetByIdAsync(BookingFlightsId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.BookingFlights
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<BookingFlight>> GetByBookingIdAsync(BookingId bookingId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.BookingFlights
            .AsNoTracking()
            .Where(x => x.BookingId == bookingId.Value)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task<BookingFlight?> GetByBookingAndFlightAsync(BookingId bookingId, FlightsId flightId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.BookingFlights
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.BookingId == bookingId.Value && x.FlightId == flightId.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<BookingFlight>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.BookingFlights
            .AsNoTracking()
            .OrderBy(x => x.BookingId)
            .ThenBy(x => x.FlightId)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(BookingFlight bookingFlight, CancellationToken cancellationToken = default)
    {
        await _context.BookingFlights.AddAsync(new BookingFlightsEntity
        {
            BookingId = bookingFlight.BookingId.Value,
            FlightId = bookingFlight.FlightId.Value,
            PartialAmount = bookingFlight.PartialAmount.Value
        }, cancellationToken);
    }

    public async Task UpdateAsync(BookingFlight bookingFlight, CancellationToken cancellationToken = default)
    {
        if (bookingFlight.Id is null)
            throw new InvalidOperationException("No se puede actualizar un booking_flight sin id.");

        var entity = await _context.BookingFlights
            .FirstOrDefaultAsync(x => x.Id == bookingFlight.Id.Value, cancellationToken);

        if (entity is null)
            throw new InvalidOperationException($"No se encontro el booking_flight con id {bookingFlight.Id.Value}.");

        entity.BookingId = bookingFlight.BookingId.Value;
        entity.FlightId = bookingFlight.FlightId.Value;
        entity.PartialAmount = bookingFlight.PartialAmount.Value;
    }

    public async Task DeleteByIdAsync(BookingFlightsId id, CancellationToken cancellationToken = default)
    {
        await _context.BookingFlights
            .Where(x => x.Id == id.Value)
            .ExecuteDeleteAsync(cancellationToken);
    }

    private static BookingFlight MapToDomain(BookingFlightsEntity entity)
        => BookingFlight.FromPrimitives(entity.Id, entity.BookingId, entity.FlightId, entity.PartialAmount);
}
