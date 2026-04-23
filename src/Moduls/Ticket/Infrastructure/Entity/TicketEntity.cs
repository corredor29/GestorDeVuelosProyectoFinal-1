using System;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Infrastructure.Entity;

public class TicketEntity
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public DateTime IssueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int PassengerReservation_Id { get; set; }
    public int TicketState_Id { get; set; }
    public PassengerReservationsEntity? PassengerReservation { get; set; }
    public TicketStatesEntity? TicketState { get; set; }
    public ICollection<CheckinEntity> Checkins { get; set; } = new List<CheckinEntity>();
}
