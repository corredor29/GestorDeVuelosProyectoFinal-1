using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.CabinTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.SeatLocationTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Infrastructure.Entity;

public sealed class FlightSeatEntity
{
    public int Id { get; set; }
    public int FlightId { get; set; }
    public string Code { get; set; } = null!;
    public int CabinTypeId { get; set; }
    public int SeatLocationTypeId { get; set; }
    public bool IsOccupied { get; set; }
    public FlightEntity? Flight { get; set; }
    public CabinTypeEntity? CabinType { get; set; }
    public SeatLocationTypesEntity? SeatLocationType { get; set; }
    public ICollection<CheckinEntity> Checkins { get; set; } = new List<CheckinEntity>();
}
