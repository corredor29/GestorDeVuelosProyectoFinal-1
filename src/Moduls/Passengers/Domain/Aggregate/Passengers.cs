using System;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.Aggregate;

public sealed class Passengers
{
    public PassengersId Id { get; private set; } = null!;
    public PassengersPersonId PersonId { get; private set; } = null!;
    public PassengersTypeId PassengerTypeId { get; private set; } = null!;

    private Passengers() { }

    private Passengers(PassengersId id, PassengersPersonId personId, PassengersTypeId passengerTypeId)
    {
        Id = id;
        PersonId = personId;
        PassengerTypeId = passengerTypeId;
    }

    public static Passengers Create(int personId, int passengerTypeId)
    {
        return new Passengers(
            null!, // El ID lo asigna la DB al ser autoincrement
            PassengersPersonId.Create(personId),
            PassengersTypeId.Create(passengerTypeId)
        );
    }
    // Para reconstruir desde la base de datos
    public static Passengers FromPrimitives(int id, int personId, int passengerTypeId)
    {
        return new Passengers(
            PassengersId.Create(id),
            PassengersPersonId.Create(personId),
            PassengersTypeId.Create(passengerTypeId)
        );
    }
    public void Update(int passengerTypeId)
    {
        PassengerTypeId = PassengersTypeId.Create(passengerTypeId);
    }
}