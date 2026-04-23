using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Infrastructure.Entities;
namespace GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Infrastructure.Persistence.Entities;

public sealed class AvailabilityStateEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public  ICollection<StaffAvailabilityEntity> StaffAvailability { get; set; } = new List<StaffAvailabilityEntity>();
}
