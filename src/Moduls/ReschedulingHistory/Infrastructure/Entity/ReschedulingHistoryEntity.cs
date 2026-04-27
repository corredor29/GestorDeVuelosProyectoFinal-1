using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.src.Moduls.ReschedulingHistory.Infrastructure.Entity;

public sealed class ReschedulingHistoryEntity
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public int PreviousFlightId { get; set; }
    public int NewFlightId { get; set; }
    public DateTime ChangedAt { get; set; }
    public string Reason { get; set; } = null!;
    public BookingEntity Booking { get; set; } = null!;
    public FlightEntity PreviousFlight { get; set; } = null!;
    public FlightEntity NewFlight { get; set; } = null!;
}