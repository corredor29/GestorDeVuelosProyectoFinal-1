using System;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.Aggregate;

public sealed class FlightAssignment
{
    public FlightAssignmentId? Id { get; private set; }
    public FlightsId FlightId { get; private set; } = null!;
    public PersonalId StaffId { get; private set; } = null!;
    public FlightRolesId FlightRoleId { get; private set; } = null!;

    private FlightAssignment() { }

    private FlightAssignment(
        FlightAssignmentId? id,
        FlightsId flightId,
        PersonalId staffId,
        FlightRolesId flightRoleId)
    {
        Id = id;
        FlightId = flightId;
        StaffId = staffId;
        FlightRoleId = flightRoleId;
    }

    public static FlightAssignment Create(int flightId, int staffId, int flightRoleId)
        => new(
            id: null,
            flightId: FlightsId.Create(flightId),
            staffId: PersonalId.Create(staffId),
            flightRoleId: FlightRolesId.Create(flightRoleId));

    public static FlightAssignment FromPrimitives(int id, int flightId, int staffId, int flightRoleId)
        => new(
            id: FlightAssignmentId.Create(id),
            flightId: FlightsId.Create(flightId),
            staffId: PersonalId.Create(staffId),
            flightRoleId: FlightRolesId.Create(flightRoleId));

    public void Update(int flightId, int staffId, int flightRoleId)
    {
        FlightId = FlightsId.Create(flightId);
        StaffId = PersonalId.Create(staffId);
        FlightRoleId = FlightRolesId.Create(flightRoleId);
    }
}
