using System;
using GestorDeVuelosProyectoFinal.src.Moduls.Bookings.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingStatusTransitions.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.BookingStatuses.Infrastructure.Entity;

public sealed class BookingStatusesEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<BookingStatusTransitionEntity> FromTransitions { get; set; } = new List<BookingStatusTransitionEntity>();
    public ICollection<BookingStatusTransitionEntity> ToTransitions { get; set; } = new List<BookingStatusTransitionEntity>();
    public ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
}