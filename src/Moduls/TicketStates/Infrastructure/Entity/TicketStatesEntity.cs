using System;
using GestorDeVuelosProyectoFinal.src.Moduls.Ticket.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.TicketStates.Infrastructure.Entity;

public class TicketStatesEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<TicketEntity> Tickets { get; set; } = new List<TicketEntity>();
}
