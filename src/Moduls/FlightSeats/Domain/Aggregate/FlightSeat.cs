using GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightSeats.Domain.Aggregate;

public sealed class FlightSeat
{
        public FlightSeatsId Id { get; private set; } = null!;
        public FlightsId FlightId { get; private set; } = null!;
        public CabinTypesId CabinTypeId { get; private set; } = null!;
        public SeatLocationTypesId SeatLocationTypeId { get; private set; } = null!;
        public FlightSeatsIsOccupied IsOccupied { get; private set; } = null!;
        public FlightSeatsCode Code { get; private set; } = null!;

        private FlightSeat() { }

        private FlightSeat(
            FlightSeatsId id,
            FlightsId flightId,
            CabinTypesId cabinTypeId,
            SeatLocationTypesId seatLocationTypeId,
            FlightSeatsIsOccupied isOccupied,
            FlightSeatsCode code)
    {
        Id = id;
        FlightId = flightId;
        CabinTypeId = cabinTypeId;
        SeatLocationTypeId = seatLocationTypeId;
        IsOccupied = isOccupied;
        Code = code;
    }

    public static FlightSeat Create(
        int flightId,
        int cabinTypeId,
        int seatLocationTypeId,
        bool isOccupied,
        string code)
    {
        return new FlightSeat
    {
        FlightId = FlightsId.Create(flightId),
        CabinTypeId = CabinTypesId.Create(cabinTypeId),
        SeatLocationTypeId = SeatLocationTypesId.Create(seatLocationTypeId),
        IsOccupied = FlightSeatsIsOccupied.Create(isOccupied),
        Code = FlightSeatsCode.Create(code)
    };
    }

    public static FlightSeat FromPrimitives(
        int id,
        int flightId,
        int cabinTypeId,
        int seatLocationTypeId,
        bool isOccupied,
        string code)
    {
        return new FlightSeat(
            FlightSeatsId.Create(id),
            FlightsId.Create(flightId),
            CabinTypesId.Create(cabinTypeId),
            SeatLocationTypesId.Create(seatLocationTypeId),
            FlightSeatsIsOccupied.Create(isOccupied),
            FlightSeatsCode.Create(code));
    }

    public void Update(int cabinTypeId, int seatLocationTypeId, string code, bool isOccupied)
    {
    CabinTypeId = CabinTypesId.Create(cabinTypeId);
    SeatLocationTypeId = SeatLocationTypesId.Create(seatLocationTypeId);
    Code = FlightSeatsCode.Create(code);
    IsOccupied = FlightSeatsIsOccupied.Create(isOccupied);
    }

    public void Occupy() 
    {
        IsOccupied = FlightSeatsIsOccupied.Create(true);
    }
    public void Release() 
    {
        IsOccupied = FlightSeatsIsOccupied.Create(false);
    }
}