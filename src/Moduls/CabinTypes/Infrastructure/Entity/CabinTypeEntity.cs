using System;
using GestorDeVuelosProyectoFinal.src.Moduls.CabinConfiguration.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.CabinTypes.Infrastructure.Entity;

public class CabinTypeEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public  ICollection<CabinConfiurationEntity> CabinType { get; set; } = new List<CabinConfiurationEntity>();
    public ICollection<RateEntity> Rates { get; set; } = new List<RateEntity>();
    public ICollection<FlightSeatEntity> FlightSeats { get; set; } = new List<FlightSeatEntity>();
}
