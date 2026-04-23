using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.Aggregate;

public sealed class Flights
{
    public FlightsId? Id { get; private set; }
    public FlightCode Code { get; private set; } = null!;
    public FlightsAirlineId AirlineId { get; private set; } = null!;
    public FlightsRouteId RouteId { get; private set; } = null!;
    public FlightsAircraftId AircraftId { get; private set; } = null!;
    public FlightDepartureTime DepartureAt { get; private set; } = null!;
    public FlightEstimatedArrivalTime EstimatedArrivalAt { get; private set; } = null!;
    public FlightTotalCapacity TotalCapacity { get; private set; } = null!;
    public FlightAvailableSeats AvailableSeats { get; private set; } = null!;
    public FlightsFlightStatusId FlightStatusId { get; private set; } = null!;
    public FlightRescheduledAt RescheduledAt { get; private set; } = null!;
    public FlightsCreatedAt CreatedAt { get; private set; } = null!;
    public FlightUpdatedAt UpdatedAt { get; private set; } = null!;

    private Flights() { }

    private Flights(
        FlightsId? id,
        FlightCode code,
        FlightsAirlineId airlineId,
        FlightsRouteId routeId,
        FlightsAircraftId aircraftId,
        FlightDepartureTime departureAt,
        FlightEstimatedArrivalTime estimatedArrivalAt,
        FlightTotalCapacity totalCapacity,
        FlightAvailableSeats availableSeats,
        FlightsFlightStatusId flightStatusId,
        FlightRescheduledAt rescheduledAt,
        FlightsCreatedAt createdAt,
        FlightUpdatedAt updatedAt)
    {
        Id = id;
        Code = code;
        AirlineId = airlineId;
        RouteId = routeId;
        AircraftId = aircraftId;
        DepartureAt = departureAt;
        EstimatedArrivalAt = estimatedArrivalAt;
        TotalCapacity = totalCapacity;
        AvailableSeats = availableSeats;
        FlightStatusId = flightStatusId;
        RescheduledAt = rescheduledAt;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Flights Create(
        string flightCode,
        int airlineId,
        int routeId,
        int aircraftId,
        DateTime departureAt,
        DateTime estimatedArrivalAt,
        int totalCapacity,
        int availableSeats,
        int flightStatusId,
        DateTime? rescheduledAt)
    {
        var total = FlightTotalCapacity.Create(totalCapacity);
        var available = FlightAvailableSeats.Create(availableSeats);
        EnsureSeatsNotOverCapacity(total, available);

        return new Flights(
            null,
            FlightCode.Create(flightCode),
            FlightsAirlineId.Create(airlineId),
            FlightsRouteId.Create(routeId),
            FlightsAircraftId.Create(aircraftId),
            FlightDepartureTime.Create(departureAt),
            FlightEstimatedArrivalTime.Create(estimatedArrivalAt),
            total,
            available,
            FlightsFlightStatusId.Create(flightStatusId),
            FlightRescheduledAt.CreateNullable(rescheduledAt),
            FlightsCreatedAt.Create(DateTime.UtcNow),
            FlightUpdatedAt.Create(DateTime.UtcNow));
    }

    public static Flights FromPersistence(
        int id,
        string flightCode,
        int airlineId,
        int routeId,
        int aircraftId,
        DateTime departureAt,
        DateTime estimatedArrivalAt,
        int totalCapacity,
        int availableSeats,
        int flightStatusId,
        DateTime? rescheduledAt,
        DateTime createdAt,
        DateTime updatedAt)
    {
        var total = FlightTotalCapacity.Create(totalCapacity);
        var available = FlightAvailableSeats.Create(availableSeats);
        EnsureSeatsNotOverCapacity(total, available);

        return new Flights(
            FlightsId.Create(id),
            FlightCode.Create(flightCode),
            FlightsAirlineId.Create(airlineId),
            FlightsRouteId.Create(routeId),
            FlightsAircraftId.Create(aircraftId),
            FlightDepartureTime.FromPersistence(departureAt),
            FlightEstimatedArrivalTime.FromPersistence(estimatedArrivalAt),
            total,
            available,
            FlightsFlightStatusId.Create(flightStatusId),
            FlightRescheduledAt.FromPersistence(rescheduledAt),
            FlightsCreatedAt.FromPersistence(createdAt),
            FlightUpdatedAt.FromPersistence(updatedAt));
    }

    public void UpdateFlightCode(string newCode) => Code = FlightCode.Create(newCode);

    public void UpdateDepartureAt(DateTime value) => DepartureAt = FlightDepartureTime.Create(value);

    public void UpdateEstimatedArrivalAt(DateTime value) => EstimatedArrivalAt = FlightEstimatedArrivalTime.Create(value);

    public void UpdateTotalCapacity(int value)
    {
        var total = FlightTotalCapacity.Create(value);
        EnsureSeatsNotOverCapacity(total, AvailableSeats);
        TotalCapacity = total;
    }

    public void UpdateAvailableSeats(int value)
    {
        var available = FlightAvailableSeats.Create(value);
        EnsureSeatsNotOverCapacity(TotalCapacity, available);
        AvailableSeats = available;
    }

    public void DecrementAvailableSeats(int seats = 1)
    {
        var next = AvailableSeats.Value - seats;
        UpdateAvailableSeats(next);
    }

    public void IncrementAvailableSeats(int seats = 1)
    {
        var next = AvailableSeats.Value + seats;
        UpdateAvailableSeats(next);
    }

    public void UpdateFlightStatusId(int flightStatusId) =>
        FlightStatusId = FlightsFlightStatusId.Create(flightStatusId);

    public void UpdateRescheduledAt(DateTime? value) =>
        RescheduledAt = FlightRescheduledAt.CreateNullable(value);

    public void TouchUpdatedAt() => UpdatedAt = FlightUpdatedAt.Create(DateTime.UtcNow);

    public bool HasAvailableSeats(int seats = 1) => AvailableSeats.Value >= seats;

    private static void EnsureSeatsNotOverCapacity(FlightTotalCapacity total, FlightAvailableSeats available)
    {
        if (available.Value > total.Value)
            throw new ArgumentException("available_seats no puede superar total_capacity.");
    }
}
