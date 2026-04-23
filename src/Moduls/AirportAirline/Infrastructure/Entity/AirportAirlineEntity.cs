using GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Entities;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Infrastructure.Persistence.Entities;
namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Infrastructure.Persistence.Entities;

public sealed class AirportAirlineEntity
{
    public int Id { get; set; }
    public int AirportId { get; set; }
    public int AirlineId { get; set; }
    public string? Terminal { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
    public AirportEntity Airport { get; set; } = null!;
    public AirlineEntity Airline { get; set; } = null!;
}
