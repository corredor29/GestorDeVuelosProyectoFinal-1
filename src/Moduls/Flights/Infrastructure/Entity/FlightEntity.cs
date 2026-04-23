using GestorDeVuelosProyectoFinal.Moduls.Airlines.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Infrastructure.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.BookingFlights.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;

public sealed class FlightEntity
{
    public int Id { get; set; }
    public string FlightCode { get; set; } = null!;
    public int AirlineId { get; set; }
    public int RouteId { get; set; }
    public int AircraftId { get; set; }
    public DateTime DepartureAt { get; set; }
    public DateTime EstimatedArrivalAt { get; set; }
    public int TotalCapacity { get; set; }
    public int AvailableSeats { get; set; }
    public int FlightStatusId { get; set; }
    public DateTime? RescheduledAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public AirlineEntity? Airline { get; set; }
    public RouteEntity? Route { get; set; }
    public AircraftEntity? Aircraft { get; set; }
    public FlightStatusEntity? FlightStatus { get; set; }
    public ICollection<FlightSeatEntity> FlightSeats { get; set; } = new List<FlightSeatEntity>();
    public ICollection<BookingFlightsEntity> BookingFlights { get; set; } = new List<BookingFlightsEntity>();
}
