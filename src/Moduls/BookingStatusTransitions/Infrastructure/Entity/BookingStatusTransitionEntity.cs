using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Infrastructure.Entity;

public sealed class BookingStatusTransitionEntity
{
    public int Id { get; set; }
    public int FromStatusId { get; set; }
    public int ToStatusId { get; set; }
    public BookingStatusesEntity FromStatus { get; set; } = null!;
    public BookingStatusesEntity ToStatus { get; set; } = null!;
}
