using GestorDeVuelosProyectoFinal.Moduls.Routes.Infrastructure.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.CabinTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.Seasons.Infrastructure.Entities;
namespace GestorDeVuelosProyectoFinal.src.Moduls.Rates.Infrastructure.Entity;

public sealed class RateEntity
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public int CabinTypeId { get; set; }
    public int PassengerTypeId { get; set; }
    public int SeasonId { get; set; }
    public decimal BasePrice { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidUntil { get; set; }
    public RouteEntity? Route { get; set; }
    public CabinTypeEntity? CabinType { get; set; }
    public PassengerTypeEntity? PassengerType { get; set; }
    public SeasonEntity? Season { get; set; }

}
