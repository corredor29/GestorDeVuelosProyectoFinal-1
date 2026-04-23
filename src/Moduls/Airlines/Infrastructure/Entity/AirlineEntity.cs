using GestorDeVuelosProyectoFinal.Moduls.Countries.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.Infrastructure.Persistence.Entities;

public sealed class AirlineEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string IataCode { get; set; } = null!;
    public int OriginCountryId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public CountryEntity Country { get; set; } = null!;
    public  ICollection<AirportAirlineEntity> AirportAirline { get; set; } = new List<AirportAirlineEntity>();
    public  ICollection<StaffEntity> Staff { get; set; } = new List<StaffEntity>();
    public  ICollection<AircraftEntity> Aircraft { get; set; } = new List<AircraftEntity>();
    public ICollection<FlightEntity> Flights { get; set; } = new List<FlightEntity>();
}
