using System;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightReservations.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Infrastructure.Entity;

public class PassengerReservationsEntity
{
    public int Id { get; set; }

    public int Flight_Reservation_Id { get; set; }

    public int Passenger_Id { get; set; }
    public FlightReservationsEntity? FlightReservation { get; set; }
    public PassengersEntity? Passenger { get; set; }
    public ICollection<TicketEntity> Tickets { get; set; } = new List<TicketEntity>();
}