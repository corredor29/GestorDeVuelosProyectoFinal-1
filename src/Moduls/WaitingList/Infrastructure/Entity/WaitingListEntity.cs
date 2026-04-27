using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.WaitingList.Infrastructure.Entity;

public sealed class WaitingListEntity
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public int FlightId { get; set; }
    public DateTime RequestedAt { get; set; }
    public int Priority { get; set; }
    public string Status { get; set; } = null!;
    public BookingEntity Booking {get; set;} = null!;
    public FlightEntity Flight {get; set;} = null!;
}