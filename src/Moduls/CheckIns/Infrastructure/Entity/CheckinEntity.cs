using GestorDeVuelosProyectoFinal.src.Moduls.CheckinStates.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Infrastructure.Entity;

public class CheckinEntity
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public int StaffId { get; set; }
    public int FlightSeatId { get; set; }
    public DateTime CheckedInAt { get; set; }
    public int CheckinStatusId { get; set; }
    public string BoardingPassNumber { get; set; } = null!;
    public TicketEntity? Ticket { get; set; }
    public StaffEntity? Staff { get; set; }
    public CheckinStatesEntity? CheckinStatus { get; set; }
    public FlightSeatEntity? FlightSeat { get; set; }
    public ICollection<BaggageEntity> Baggage { get; set; } = new List<BaggageEntity>();
    
}