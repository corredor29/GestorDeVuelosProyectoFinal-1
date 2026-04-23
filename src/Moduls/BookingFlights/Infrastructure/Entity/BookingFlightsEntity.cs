using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Infrastructure.Entity;

public sealed class BookingFlightsEntity
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public int FlightId { get; set; }
    public decimal PartialAmount { get; set; }
    public BookingEntity? Booking { get; set; }
    public FlightEntity? Flight { get; set; }
}
