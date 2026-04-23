using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Infrastructure.Entity;

public sealed class FlightRolesEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<FlightAssignmentEntity> FlightAssignments { get; set; } = new List<FlightAssignmentEntity>();
}
