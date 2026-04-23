using System;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.Aggregate;

public sealed class FlightRole
{
    public FlightRolesId? Id { get; private set; }
    public FlightRolesName Name { get; private set; } = null!;

    private FlightRole() { }

    private FlightRole(FlightRolesId? id, FlightRolesName name)
    {
        Id = id;
        Name = name;
    }

    public static FlightRole Create(string name)
        => new(null, FlightRolesName.Create(name));

    public static FlightRole FromPrimitives(int id, string name)
        => new(FlightRolesId.Create(id), FlightRolesName.Create(name));

    public void UpdateName(string newName)
        => Name = FlightRolesName.Create(newName);
}
