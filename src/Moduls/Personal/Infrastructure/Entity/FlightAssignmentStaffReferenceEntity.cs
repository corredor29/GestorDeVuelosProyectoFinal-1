namespace GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Entities;

public sealed class FlightAssignmentStaffReferenceEntity
{
    public int Id { get; set; }
    public int FlightId { get; set; }
    public int StaffId { get; set; }
}
