using System;
using GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.SeatLocationTypes.Domain.Aggregate;

public sealed class SeatLocationType
{
    public SeatLocationTypesId? Id { get; private set; }
    public SeatLocationTypesName Name { get; private set; } = null!;

    private SeatLocationType() { }

    private SeatLocationType(SeatLocationTypesId? id, SeatLocationTypesName name)
    {
        Id = id;
        Name = name;
    }

    public static SeatLocationType Create(string name)
        => new(null, SeatLocationTypesName.Create(name));
    public static SeatLocationType FromPrimitives(int id, string name)
        => new(SeatLocationTypesId.Create(id), SeatLocationTypesName.Create(name));
    public void UpdateName(string newName)
    {
        Name = SeatLocationTypesName.Create(newName);
    }
}