using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.Aggregate;

public sealed class AirportAirlineOperation
{
    // Este agregado modela la operación concreta de una aerolínea dentro de un aeropuerto.
    public AirportAirlineId? Id { get; private set; }
    public AirportsId AirportId { get; private set; } = null!;
    public AirlinesId AirlineId { get; private set; } = null!;
    public AirportAirlineTerminal Terminal { get; private set; } = null!;
    public AirportAirlineStartDate StartDate { get; private set; } = null!;
    public AirportAirlineEndDate EndDate { get; private set; } = null!;
    public AirportAirlineIsActive IsActive { get; private set; } = null!;

    private AirportAirlineOperation() { }

    private AirportAirlineOperation(
        AirportAirlineId? id,
        AirportsId airportId,
        AirlinesId airlineId,
        AirportAirlineTerminal terminal,
        AirportAirlineStartDate startDate,
        AirportAirlineEndDate endDate,
        AirportAirlineIsActive isActive)
    {
        Id = id;
        AirportId = airportId;
        AirlineId = airlineId;
        Terminal = terminal;
        StartDate = startDate;
        EndDate = endDate;
        IsActive = isActive;
    }

    public static AirportAirlineOperation Create(
        int airportId,
        int airlineId,
        string? terminal,
        DateTime startDate,
        DateTime? endDate,
        bool isActive = true)
    {
        // La validación del rango de fechas se hace antes de persistir cualquier relación.
        var endDateVo = AirportAirlineEndDate.Create(endDate);
        EnsureDateRange(startDate, endDateVo.Value);

        return new AirportAirlineOperation(
            id: null,
            airportId: AirportsId.Create(airportId),
            airlineId: AirlinesId.Create(airlineId),
            terminal: AirportAirlineTerminal.Create(terminal),
            startDate: AirportAirlineStartDate.Create(startDate),
            endDate: endDateVo,
            isActive: AirportAirlineIsActive.Create(isActive));
    }

    public static AirportAirlineOperation FromPrimitives(
        int id,
        int airportId,
        int airlineId,
        string? terminal,
        DateTime startDate,
        DateTime? endDate,
        bool isActive)
    {
        var endDateVo = AirportAirlineEndDate.Create(endDate);
        EnsureDateRange(startDate, endDateVo.Value);

        return new AirportAirlineOperation(
            id: AirportAirlineId.Create(id),
            airportId: AirportsId.Create(airportId),
            airlineId: AirlinesId.Create(airlineId),
            terminal: AirportAirlineTerminal.Create(terminal),
            startDate: AirportAirlineStartDate.Create(startDate),
            endDate: endDateVo,
            isActive: AirportAirlineIsActive.Create(isActive));
    }

    public void Update(int airportId, int airlineId, string? terminal, DateTime startDate, DateTime? endDate, bool isActive)
    {
        var endDateVo = AirportAirlineEndDate.Create(endDate);
        EnsureDateRange(startDate, endDateVo.Value);

        AirportId = AirportsId.Create(airportId);
        AirlineId = AirlinesId.Create(airlineId);
        Terminal = AirportAirlineTerminal.Create(terminal);
        StartDate = AirportAirlineStartDate.Create(startDate);
        EndDate = endDateVo;
        IsActive = AirportAirlineIsActive.Create(isActive);
    }

    public void Deactivate()
    {
        IsActive = AirportAirlineIsActive.Create(false);
    }

    public void Reactivate()
    {
        IsActive = AirportAirlineIsActive.Create(true);
    }

    private static void EnsureDateRange(DateTime startDate, DateTime? endDate)
    {
        if (endDate.HasValue && endDate.Value.Date < startDate.Date)
            throw new ArgumentException("La fecha de fin no puede ser menor que la fecha de inicio.");
    }
}
