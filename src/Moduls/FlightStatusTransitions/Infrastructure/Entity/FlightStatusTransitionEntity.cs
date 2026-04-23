using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Infrastructure.Entity;

public sealed class FlightStatusTransitionEntity
{
    public int Id { get; set; }
    public int FromStatusId { get; set; }
    public int ToStatusId { get; set; }
    public FlightStatusEntity? FromStatus { get; set; }
    public FlightStatusEntity? ToStatus { get; set; } 
}
