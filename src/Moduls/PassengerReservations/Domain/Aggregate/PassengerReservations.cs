using System;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightReservations.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.PassengerReservations.Domain.Aggregate;

public sealed class PassengerReservation
{
    public PassengerReservationId Id { get; private set; } = null!;
    public FlightReservationId FlightReservationId { get; private set; } = null!;
    public PassengersId PassengerId { get; private set; } = null!;
    internal void SetId(int id)
    {
        Id = PassengerReservationId.Create(id);
    }

    private PassengerReservation() { }

    public static PassengerReservation Create(int flightReservationId, int passengerId)
    {
        return new PassengerReservation
        {
            FlightReservationId = FlightReservationId.Create(flightReservationId),
            PassengerId = PassengersId.Create(passengerId)
        };
    }
    public void Update(int flightReservationId, int passengerId)
    {
        FlightReservationId = FlightReservationId.Create(flightReservationId);
        PassengerId = PassengersId.Create(passengerId);
    }
}
