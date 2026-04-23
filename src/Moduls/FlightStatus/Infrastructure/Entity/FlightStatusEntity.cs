using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Infrastructure.Entity;

public sealed class FlightStatusEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<FlightStatusTransitionEntity> FromTransitions { get; set; } = new List<FlightStatusTransitionEntity>();
    public ICollection<FlightStatusTransitionEntity> ToTransitions { get; set; } = new List<FlightStatusTransitionEntity>();
    public ICollection<FlightEntity> Flights { get; set; } = new List<FlightEntity>();
}
