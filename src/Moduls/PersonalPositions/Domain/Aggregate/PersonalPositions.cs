using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.Aggregate;

public sealed class PersonalPosition
{
    public PersonalPositionsId? Id { get; private set; }
    public PersonalPositionsName Name { get; private set; } = null!;

    private PersonalPosition() { }

    private PersonalPosition(PersonalPositionsId? id, PersonalPositionsName name)
    {
        Id = id;
        Name = name;
    }

    public static PersonalPosition Create(string name)
    {
        return new PersonalPosition(
            id: null,
            name: PersonalPositionsName.Create(name));
    }

    public static PersonalPosition FromPrimitives(int id, string name)
    {
        return new PersonalPosition(
            id: PersonalPositionsId.Create(id),
            name: PersonalPositionsName.Create(name));
    }

    public void Update(string name)
    {
        Name = PersonalPositionsName.Create(name);
    }
}
