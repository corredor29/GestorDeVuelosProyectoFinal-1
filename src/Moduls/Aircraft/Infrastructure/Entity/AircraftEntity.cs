using GestorDeVuelosProyectoFinal.src.Moduls.AircraftModels.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.src.Moduls.CabinConfiguration.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Infrastructure.Entity;
namespace GestorDeVuelosProyectoFinal.src.Moduls.Aircraft.Infrastructure.Entity;

public sealed class AircraftEntity
{
    public int Id { get; set; }
    public int AircraftModelId { get; set; }
    public int AirlinesId { get; set; }
    public string Registration { get; set; } = null!;
    public DateTime? DateManufactured { get; set; }
    public bool IsActive { get; set; }
    public AircraftModelsEntity? Model { get; set; }
    public AirlineEntity? Airline { get; set; }
    public ICollection<CabinConfiurationEntity> CabinConfigurations { get; set; } = new List<CabinConfiurationEntity>();
    public ICollection<FlightEntity> Flights { get; set; } = new List<FlightEntity>();

}
