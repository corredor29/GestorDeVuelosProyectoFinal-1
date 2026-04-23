using GestorDeVuelosProyectoFinal.Moduls.People.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Infrastructure.Entity;

public sealed class PassengersEntity
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public int PassengerTypeId { get; set; }
    public PersonEntity? Person { get; set; }
    public PassengerTypeEntity? PassengerType { get; set; }
    public ICollection<PassengerReservationsEntity> PassengerReservations { get; set; } = new List<PassengerReservationsEntity>();
}