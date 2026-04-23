using GestorDeVuelosProyectoFinal.Moduls.Cities.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Infrastructure.Entities;
namespace GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Entities;

public sealed class AirportEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string IataCode { get; set; } = null!;
    public string? IcaoCode { get; set; }
    public int CityId { get; set; }
    public CityEntity City { get; set; } = null!;
    public  ICollection<AirportAirlineEntity> AirportAirline { get; set; } = new List<AirportAirlineEntity>();
    public  ICollection<StaffEntity> Staff { get; set; } = new List<StaffEntity>();
    public ICollection<RouteEntity> OriginRoutes { get; set; } = new List<RouteEntity>();
    public ICollection<RouteEntity> DestinationRoutes { get; set; } = new List<RouteEntity>();
    public ICollection<RouteStopoversEntity> RouteStopovers { get; set; } = new List<RouteStopoversEntity>();
}
