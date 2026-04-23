using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Entities;
namespace GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Infrastructure.Entities;

public sealed class StaffAvailabilityEntity
{
    public int Id { get; set; }
    public int StaffId { get; set; }
    public int AvailabilityStatusId { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public string? Notes { get; set; }
    public AvailabilityStateEntity AvailabilityState { get; set; } = null!;
    public StaffEntity Staff { get; set; } = null!;
}
