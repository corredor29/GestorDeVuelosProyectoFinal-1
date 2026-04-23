using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.RouteStopovers.Domain.Aggregate;

public sealed class RouteStopover
{
    public RouteStopOversId? Id { get; private set; }
    public RouteId RouteId { get; private set; } = null!;
    public AirportsId StopoverAirportId { get; private set; } = null!;
    public RouteStopOrder StopOrder { get; private set; } = null!;
    public LayoverMinutes LayoverMin { get; private set; } = null!;

    private RouteStopover() { }

    private RouteStopover(
        RouteStopOversId? id,
        RouteId routeId,
        AirportsId stopoverAirportId,
        RouteStopOrder stopOrder,
        LayoverMinutes layoverMin)
    {
        Id = id;
        RouteId = routeId;
        StopoverAirportId = stopoverAirportId;
        StopOrder = stopOrder;
        LayoverMin = layoverMin;
    }

    public static RouteStopover Create(int routeId, int stopoverAirportId, int stopOrder, int layoverMin)
    {
        return new RouteStopover(
            null,
            RouteId.Create(routeId),
            AirportsId.Create(stopoverAirportId),
            RouteStopOrder.Create(stopOrder),
            LayoverMinutes.Create(layoverMin));
    }

    public static RouteStopover FromPrimitives(int id, int routeId, int stopoverAirportId, int stopOrder, int layoverMin)
    {
        return new RouteStopover(
            RouteStopOversId.Create(id),
            RouteId.Create(routeId),
            AirportsId.Create(stopoverAirportId),
            RouteStopOrder.Create(stopOrder),
            LayoverMinutes.Create(layoverMin));
    }

    public void Update(int routeId, int stopoverAirportId, int stopOrder, int layoverMin)
    {
        RouteId = RouteId.Create(routeId);
        StopoverAirportId = AirportsId.Create(stopoverAirportId);
        StopOrder = RouteStopOrder.Create(stopOrder);
        LayoverMin = LayoverMinutes.Create(layoverMin);
    }
}
