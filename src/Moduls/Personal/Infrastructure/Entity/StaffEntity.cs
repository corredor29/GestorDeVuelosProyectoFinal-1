using GestorDeVuelosProyectoFinal.Moduls.Airlines.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.People.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Infrastructure.Entities;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Infrastructure.Entity;

namespace GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Entities;

public sealed class StaffEntity
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public int PositionId { get; set; }
    public int? AirlineId { get; set; }
    public int? AirportId { get; set; }
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public PersonEntity Person { get; set; } = null!;
    public PersonalPositionEntity Position { get; set; } = null!;
    public AirlineEntity? Airline { get; set; }
    public AirportEntity? Airport { get; set; }
    public  ICollection<StaffAvailabilityEntity> StaffAvailability { get; set; } = new List<StaffAvailabilityEntity>();
    public ICollection<FlightAssignmentEntity> FlightAssignments { get; set; } = new List<FlightAssignmentEntity>();
    public ICollection<CheckinEntity> Checkins { get; set; } = new List<CheckinEntity>();
}
