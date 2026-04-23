namespace GestorDeVuelosProyectoFinal.Moduls.Airports.Infrastructure.Persistence.Entities;

public sealed class AirportRouteReferenceEntity
{
    public int Id { get; set; }
    public int OriginAirportId { get; set; }
    public int DestinationAirportId { get; set; }
}
