using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Infrastructure.Entity;

public sealed class SeatLocationTypesEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<FlightSeatEntity> FlightSeats { get; set; } = new List<FlightSeatEntity>();
}
