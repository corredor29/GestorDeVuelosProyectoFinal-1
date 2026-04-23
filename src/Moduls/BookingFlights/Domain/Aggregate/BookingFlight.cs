using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Domain.Aggregate;

public sealed class BookingFlight
{
    public BookingFlightsId? Id { get; private set; }
    public BookingId BookingId { get; private set; } = null!;
    public FlightsId FlightId { get; private set; } = null!;
    public BookingFlightsPartialAmount PartialAmount { get; private set; } = null!;

    private BookingFlight() { }

    private BookingFlight(
        BookingFlightsId? id,
        BookingId bookingId,
        FlightsId flightId,
        BookingFlightsPartialAmount partialAmount)
    {
        Id = id;
        BookingId = bookingId;
        FlightId = flightId;
        PartialAmount = partialAmount;
    }

    public static BookingFlight Create(int bookingId, int flightId, decimal partialAmount)
    {
        return new BookingFlight(
            id: null,
            bookingId: BookingId.Create(bookingId),
            flightId: FlightsId.Create(flightId),
            partialAmount: BookingFlightsPartialAmount.Create(partialAmount));
    }

    public static BookingFlight FromPrimitives(int id, int bookingId, int flightId, decimal partialAmount)
    {
        return new BookingFlight(
            id: BookingFlightsId.Create(id),
            bookingId: BookingId.Create(bookingId),
            flightId: FlightsId.Create(flightId),
            partialAmount: BookingFlightsPartialAmount.Create(partialAmount));
    }

    public void Update(int bookingId, int flightId, decimal partialAmount)
    {
        BookingId = BookingId.Create(bookingId);
        FlightId = FlightsId.Create(flightId);
        PartialAmount = BookingFlightsPartialAmount.Create(partialAmount);
    }
}
