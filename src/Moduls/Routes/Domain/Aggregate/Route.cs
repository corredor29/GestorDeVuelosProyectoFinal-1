using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.Aggregate;

public sealed class Route
{
    // La ruta une dos aeropuertos y guarda distancia/duración como datos operativos opcionales.
    public RouteId? Id { get; private set; }
    public AirportsId OriginAirportId { get; private set; } = null!;
    public AirportsId DestinationAirportId { get; private set; } = null!;
    public RouteDistance Distance { get; private set; } = null!;
    public RouteDuration Duration { get; private set; } = null!;

    private Route() { }

    private Route(
        RouteId? id,
        AirportsId originAirportId,
        AirportsId destinationAirportId,
        RouteDistance distance,
        RouteDuration duration)
    {
        Id = id;
        OriginAirportId = originAirportId;
        DestinationAirportId = destinationAirportId;
        Distance = distance;
        Duration = duration;
    }

    public static Route Create(
        int originAirportId,
        int destinationAirportId,
        int? distanceKm,
        int? estimatedDurationMin)
    {
        // Antes de crear la ruta se valida que origen y destino no sean el mismo aeropuerto.
        EnsureAirportsAreDifferent(originAirportId, destinationAirportId);

        return new Route(
            id: null,
            originAirportId: AirportsId.Create(originAirportId),
            destinationAirportId: AirportsId.Create(destinationAirportId),
            distance: RouteDistance.Create(distanceKm),
            duration: RouteDuration.Create(estimatedDurationMin));
    }

    public static Route FromPrimitives(
        int id,
        int originAirportId,
        int destinationAirportId,
        int? distanceKm,
        int? estimatedDurationMin)
    {
        EnsureAirportsAreDifferent(originAirportId, destinationAirportId);

        return new Route(
            id: RouteId.Create(id),
            originAirportId: AirportsId.Create(originAirportId),
            destinationAirportId: AirportsId.Create(destinationAirportId),
            distance: RouteDistance.Create(distanceKm),
            duration: RouteDuration.Create(estimatedDurationMin));
    }

    public void Update(int originAirportId, int destinationAirportId, int? distanceKm, int? estimatedDurationMin)
    {
        EnsureAirportsAreDifferent(originAirportId, destinationAirportId);

        OriginAirportId = AirportsId.Create(originAirportId);
        DestinationAirportId = AirportsId.Create(destinationAirportId);
        Distance = RouteDistance.Create(distanceKm);
        Duration = RouteDuration.Create(estimatedDurationMin);
    }

    private static void EnsureAirportsAreDifferent(int originAirportId, int destinationAirportId)
    {
        if (originAirportId == destinationAirportId)
            throw new ArgumentException("El aeropuerto de origen no puede ser igual al de destino.");
    }
}
