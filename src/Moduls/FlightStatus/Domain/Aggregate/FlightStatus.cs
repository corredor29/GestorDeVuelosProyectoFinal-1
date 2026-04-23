using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatus.Domain.Aggregate;

public sealed class FlightStatus
{
    public FlightStatusId? Id { get; private set; }
    public FlightStatusName Name { get; private set; } = null!;

    private FlightStatus() { }

    private FlightStatus(FlightStatusId? id, FlightStatusName name)
    {
        Id = id;
        Name = name;
    }

    public static FlightStatus Create(string name)
        => new(null, FlightStatusName.Create(name));

    public static FlightStatus FromPrimitives(int id, string name)
        => new(FlightStatusId.Create(id), FlightStatusName.Create(name));

    public void UpdateName(string newName)
        => Name = FlightStatusName.Create(newName);
}
