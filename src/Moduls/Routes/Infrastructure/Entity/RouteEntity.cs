using GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Rates.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.Moduls.Routes.Infrastructure.Entities;

public sealed class RouteEntity
{
    public int Id { get; set; }
    public int OriginAirportId { get; set; }
    public int DestinationAirportId { get; set; }
    public int? DistanceKm { get; set; }
    public int? EstimatedDurationMin { get; set; }
    public AirportEntity? OriginAirport { get; set; }
    public AirportEntity? DestinationAirport { get; set; }
    public ICollection<RouteStopoversEntity> RouteStopovers { get; set; } = new List<RouteStopoversEntity>();
    public ICollection<RateEntity> Rates { get; set; } = new List<RateEntity>();
    public ICollection<FlightEntity> Flights { get; set; } = new List<FlightEntity>();
}
