using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Infrastructure.Entity;

public sealed class FlightAssignmentEntity
{
    public int Id { get; set; }
    public int FlightId { get; set; }
    public int StaffId { get; set; }
    public int FlightRoleId { get; set; }
    public FlightEntity? Flight { get; set; }
    public StaffEntity? Staff { get; set; }
    public FlightRolesEntity? FlightRoles { get; set; }
}
