using GestorDeVuelosProyectoFinal.Moduls.Routes.Infrastructure.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Entities;
namespace GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Infrastructure.Entity;

public sealed class RouteStopoversEntity
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public int StopoverAirportId { get; set; }
    public int StopOrder { get; set; }
    public int LayoverMin { get; set; }
    public RouteEntity? Route { get; set; }
    public AirportEntity? StopoverAirport { get; set; }
}
