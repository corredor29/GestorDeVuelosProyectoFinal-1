using GestorDeVuelosProyectoFinal.Moduls.People.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.Customers.Infrastructure.Entity;

public sealed class CustomerEntity
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public DateTime CreatedAt { get; set; }
    public PersonEntity Person { get; set; } = null!;
    public ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
}
